// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.ObjectStorage.Config;

namespace Voting.Lib.ObjectStorage;

/// <inheritdoc />
public class BucketObjectStorageClient : IBucketObjectStorageClient
{
    private readonly ObjectStorageBucketConfig _bucketConfig;
    private readonly IObjectStorageClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="BucketObjectStorageClient"/> class.
    /// </summary>
    /// <param name="bucketConfig">The configuration for this bucket.</param>
    /// <param name="client">The storage client.</param>
    public BucketObjectStorageClient(ObjectStorageBucketConfig bucketConfig, IObjectStorageClient client)
    {
        _bucketConfig = bucketConfig;
        _client = client;
    }

    /// <inheritdoc />
    public Task EnsureBucketExists(CancellationToken ct = default)
        => _client.EnsureBucketExists(_bucketConfig.BucketName, ct);

    /// <inheritdoc />
    public Task Store(
        string objectName,
        Stream data,
        string? contentType = null,
        long? fileSize = null,
        Dictionary<string, string>? meta = null,
        CancellationToken ct = default)
        => _client.Store(_bucketConfig.BucketName, ObjectName(objectName), data, contentType, fileSize, meta, ct);

    /// <inheritdoc />
    public Task Delete(string objectName, CancellationToken ct = default)
        => _client.Delete(_bucketConfig.BucketName, ObjectName(objectName), ct);

    /// <inheritdoc />
    public Task<Uri> GetPublicDownloadUrl(
        string objectName,
        TimeSpan? expiry = null,
        Dictionary<string, string>? reqParams = null)
        => _client.GetPublicDownloadUrl(
            _bucketConfig.BucketName,
            ObjectName(objectName),
            expiry ?? _bucketConfig.DefaultPublicDownloadLinkTtl,
            reqParams ?? _bucketConfig.DefaultPublicDownloadLinkParameters);

    /// <inheritdoc />
    public Task Fetch(string objectName, Action<Stream> callback)
        => _client.Fetch(_bucketConfig.BucketName, ObjectName(objectName), callback);

    /// <inheritdoc />
    public Task FetchAsBase64(string objectName, Action<Stream> callbackBase64)
        => _client.FetchAsBase64(_bucketConfig.BucketName, ObjectName(objectName), callbackBase64);

    /// <inheritdoc />
    public Task<string> FetchAsBase64(string objectName)
        => _client.FetchAsBase64(_bucketConfig.BucketName, ObjectName(objectName));

    private string ObjectName(string objectName) => _bucketConfig.ObjectPrefix + objectName;
}
