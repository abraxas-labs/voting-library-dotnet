// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Minio;
using Voting.Lib.ObjectStorage.Config;

namespace Voting.Lib.ObjectStorage;

/// <inheritdoc />
public class ObjectStorageClient : IObjectStorageClient
{
    private readonly MinioClient _client;
    private readonly ILogger<ObjectStorageClient> _logger;
    private readonly string? _publicUrlHostReplacement;
    private readonly TimeSpan _publicUrlDefaultTtl;
    private readonly Dictionary<string, string> _publicUrlDefaultParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectStorageClient"/> class.
    /// </summary>
    /// <param name="config">The configuration for this client.</param>
    /// <param name="logger">The logger.</param>
    public ObjectStorageClient(ObjectStorageConfig config, ILogger<ObjectStorageClient> logger)
    {
        _publicUrlHostReplacement = config.PublicUrlHostReplacement;
        _publicUrlDefaultTtl = config.DefaultPublicDownloadLinkTtl;
        _publicUrlDefaultParameters = config.DefaultPublicDownloadLinkParameters;
        _logger = logger;
        _client = new MinioClient(config.Endpoint, config.AccessKey, config.SecretKey);

        if (config.UseSsl)
        {
            _client.WithSSL();
        }
    }

    /// <inheritdoc />
    public async Task EnsureBucketExists(string bucketName, CancellationToken ct = default)
    {
        if (!await _client.BucketExistsAsync(bucketName, ct).ConfigureAwait(false))
        {
            _logger.LogInformation("Bucket {BucketName} does not exist yet, creating...", bucketName);
            await _client.MakeBucketAsync(bucketName, cancellationToken: ct).ConfigureAwait(false);
            _logger.LogInformation("Bucket {BucketName} created", bucketName);
        }
    }

    /// <inheritdoc />
    public async Task Store(
        string bucketName,
        string objectName,
        Stream data,
        string? contentType = null,
        long? fileSize = null,
        Dictionary<string, string>? meta = null,
        CancellationToken ct = default)
    {
        fileSize ??= data.Length;
        await _client.PutObjectAsync(bucketName, objectName, data, fileSize.Value, contentType, meta, cancellationToken: ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task Delete(string bucketName, string objectName, CancellationToken ct = default)
        => _client.RemoveObjectAsync(bucketName, objectName, ct);

    /// <inheritdoc />
    public async Task<Uri> GetPublicDownloadUrl(
        string bucketName,
        string objectName,
        TimeSpan? expiry = null,
        Dictionary<string, string>? reqParams = null)
    {
        reqParams ??= _publicUrlDefaultParameters;
        expiry ??= _publicUrlDefaultTtl;
        var url = await _client.PresignedGetObjectAsync(bucketName, objectName, (int)expiry.Value.TotalSeconds, reqParams).ConfigureAwait(false);
        var uri = new Uri(url);
        return _publicUrlHostReplacement == null
            ? uri
            : new UriBuilder(_publicUrlHostReplacement + uri.PathAndQuery).Uri;
    }

    /// <inheritdoc />
    public Task Fetch(string bucketName, string objectName, Action<Stream> callback)
        => _client.GetObjectAsync(bucketName, objectName, callback);

    /// <inheritdoc />
    public Task FetchAsBase64(string bucketName, string objectName, Action<Stream> callbackBase64)
    {
        return Fetch(bucketName, objectName, stream =>
        {
            using var b64Stream = new CryptoStream(stream, new ToBase64Transform(), CryptoStreamMode.Read);
            callbackBase64(b64Stream);
        });
    }

    /// <inheritdoc />
    public async Task<string> FetchAsBase64(string bucketName, string objectName)
    {
        var b64Object = string.Empty;
        await FetchAsBase64(bucketName, objectName, stream =>
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            b64Object = Encoding.UTF8.GetString(memoryStream.ToArray());
        }).ConfigureAwait(false);
        return b64Object;
    }
}
