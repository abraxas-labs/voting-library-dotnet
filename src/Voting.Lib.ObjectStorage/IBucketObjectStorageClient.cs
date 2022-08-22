// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.ObjectStorage;

/// <summary>
/// A bucket specific object storage (S3) client.
/// </summary>
public interface IBucketObjectStorageClient
{
    /// <summary>
    /// Ensures that this bucket exists.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EnsureBucketExists(CancellationToken ct = default);

    /// <summary>
    /// Store an object in the bucket.
    /// </summary>
    /// <param name="objectName">The object name.</param>
    /// <param name="data">The data stream.</param>
    /// <param name="contentType">The optional content type.</param>
    /// <param name="fileSize">The optional file size.</param>
    /// <param name="meta">Optional metadata to store alongside the object.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Store(
        string objectName,
        Stream data,
        string? contentType = null,
        long? fileSize = null,
        Dictionary<string, string>? meta = null,
        CancellationToken ct = default);

    /// <summary>
    /// Delete an object.
    /// </summary>
    /// <param name="objectName">The object name to delete.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Delete(string objectName, CancellationToken ct = default);

    /// <summary>
    /// Creates a public URL, which allows to download the object.
    /// </summary>
    /// <param name="objectName">The name of the object to download.</param>
    /// <param name="expiry">The expiration of the download URL.</param>
    /// <param name="reqParams">Additional parameters which will be included in the generated URL.</param>
    /// <returns>Returns the download URL.</returns>
    Task<Uri> GetPublicDownloadUrl(
        string objectName,
        TimeSpan? expiry = null,
        Dictionary<string, string>? reqParams = null);

    /// <summary>
    /// Fetches the content of an object.
    /// </summary>
    /// <param name="objectName">The name of the object to fetch.</param>
    /// <param name="callback">The callback action for the object stream.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Fetch(string objectName, Action<Stream> callback);

    /// <summary>
    /// Fetches the content of an object as Base64.
    /// </summary>
    /// <param name="objectName">The name of the object to fetch.</param>
    /// <param name="callbackBase64">The callback action for the Base64 content.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task FetchAsBase64(string objectName, Action<Stream> callbackBase64);

    /// <summary>
    /// Fetches the object as a base64 string.
    /// Try to avoid this method since it buffers the entire object in memory.
    /// </summary>
    /// <param name="objectName">The name of the object.</param>
    /// <returns>The base64 encoded object as a string.</returns>
    Task<string> FetchAsBase64(string objectName);
}
