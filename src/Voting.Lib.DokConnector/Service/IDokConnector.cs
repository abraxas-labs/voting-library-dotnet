// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.DokConnector.Models;

namespace Voting.Lib.DokConnector.Service;

/// <summary>
/// Dok connector service.
/// </summary>
public interface IDokConnector
{
    /// <summary>
    /// Uploads a file to dok connect.
    /// </summary>
    /// <param name="messageType">The message type as provided by dok connect.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="fileContent">The content of the file.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The upload response.</returns>
    Task<UploadResponse> Upload(string messageType, string fileName, Stream fileContent, CancellationToken ct);

    /// <summary>
    /// Uploads a file to dok connect.
    /// </summary>
    /// <param name="messageType">The message type as provided by dok connect.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="writer">The content provider of the file.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The upload response.</returns>
    Task<UploadResponse> Upload(
        string messageType,
        string fileName,
        Func<PipeWriter, CancellationToken, Task> writer,
        CancellationToken ct);
}
