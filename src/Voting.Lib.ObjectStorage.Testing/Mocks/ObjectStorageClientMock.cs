// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.ObjectStorage.Testing.Mocks;

/// <summary>
/// A mock implementation which stores objects in memory.
/// </summary>
public class ObjectStorageClientMock : IObjectStorageClient
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>> _storage = new();

    /// <inheritdoc />
    public Task EnsureBucketExists(string bucketName, CancellationToken ct = default)
    {
        if (!_storage.ContainsKey(bucketName))
        {
            _storage[bucketName] = new ConcurrentDictionary<string, byte[]>();
        }

        return Task.CompletedTask;
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
        await using var ms = new MemoryStream();
        await data.CopyToAsync(ms, ct).ConfigureAwait(false);
        _storage[bucketName][objectName] = ms.ToArray();
    }

    /// <inheritdoc />
    public Task Delete(string bucketName, string objectName, CancellationToken ct = default)
    {
        _storage[bucketName].Remove(objectName, out _);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<Uri> GetPublicDownloadUrl(
        string bucketName,
        string objectName,
        TimeSpan? expiry = null,
        Dictionary<string, string>? reqParams = null)
    {
        return Task.FromResult(new Uri($"http://localhost:9000/{bucketName}/{objectName}?ttl=" + (int)(expiry?.TotalSeconds ?? 0)));
    }

    /// <inheritdoc />
    public async Task Fetch(string bucketName, string objectName, Action<Stream> callback)
    {
        var data = Get(bucketName, objectName);
        await using var stream = new MemoryStream(data);
        callback(stream);
    }

    /// <inheritdoc />
    public Task FetchAsBase64(string bucketName, string objectName, Action<Stream> callbackBase64)
    {
        return Fetch(bucketName, objectName, s =>
        {
            using var b64Stream = new CryptoStream(s, new ToBase64Transform(), CryptoStreamMode.Read);
            callbackBase64(b64Stream);
        });
    }

    /// <inheritdoc />
    public async Task<string> FetchAsBase64(string bucketName, string objectName)
    {
        var b64Object = string.Empty;
        await FetchAsBase64(bucketName, objectName, stream =>
        {
            using var memoryStream = new MemoryStream((int)(stream.Length * 1.3)); // +30% avg. b64 increase
            stream.CopyTo(memoryStream);
            b64Object = Encoding.UTF8.GetString(memoryStream.ToArray());
        }).ConfigureAwait(false);
        return b64Object;
    }

    /// <summary>
    /// Gets the object content.
    /// </summary>
    /// <param name="bucketName">The bucket name.</param>
    /// <param name="objectName">The object name.</param>
    /// <returns>Returns the object content.</returns>
    public byte[] Get(string bucketName, string objectName)
        => _storage[bucketName][objectName];

    /// <summary>
    /// Checks whether the object exists in the bucket.
    /// </summary>
    /// <param name="bucketName">The bucket name.</param>
    /// <param name="objectName">The object name.</param>
    /// <returns>Returns whether the object exists in the bucket.</returns>
    public bool Exists(string bucketName, string objectName)
        => _storage.TryGetValue(bucketName, out var bucket) && bucket.ContainsKey(objectName);
}
