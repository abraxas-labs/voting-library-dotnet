// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.DokConnector.Configuration;
using Voting.Lib.DokConnector.Models;

namespace Voting.Lib.DokConnector.Service;

/// <summary>
/// The implementation of the EAI DOK connector.
/// </summary>
public class EaiDokConnector : IDokConnector
{
    private readonly DokConnectorConfig _config;
    private readonly HttpClient _client;
    private readonly ILogger<EaiDokConnector> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EaiDokConnector"/> class.
    /// </summary>
    /// <param name="config">The DOK connector config.</param>
    /// <param name="client">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    public EaiDokConnector(DokConnectorConfig config, HttpClient client, ILogger<EaiDokConnector> logger)
    {
        _config = config;
        _client = client;
        _logger = logger;
    }

    /// <summary>
    /// Upload a file to the DOK connector.
    /// </summary>
    /// <param name="messageType">The message type.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="writer">The pipe writer for the file content.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the upload response.</returns>
    public async Task<UploadResponse> Upload(
        string messageType,
        string fileName,
        Func<PipeWriter, CancellationToken, Task> writer,
        CancellationToken ct)
    {
        var pipe = new Pipe();
        var writerTask = writer(pipe.Writer, ct);
        var uploadTask = Upload(messageType, fileName, pipe.Reader.AsStream(), ct);
        await Task.WhenAll(writerTask, uploadTask).ConfigureAwait(false);
        return await uploadTask.ConfigureAwait(false);
    }

    /// <summary>
    /// Upload a file to the DOK connector.
    /// </summary>
    /// <param name="messageType">The message type.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="fileContent">The file content as a stream.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the upload response.</returns>
    public async Task<UploadResponse> Upload(
        string messageType,
        string fileName,
        Stream fileContent,
        CancellationToken ct)
    {
        using var requestContent = new MultipartContent();
        using var jsonContent = JsonContent.Create(new FileUploadRequest
        {
            FileName = fileName,
            MessageType = messageType,
            UserName = _config.UserName,
        });
        requestContent.Add(jsonContent);

        using var fileStreamContent = new StreamContent(fileContent);
        requestContent.Add(fileStreamContent);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/upload");
        request.Content = requestContent;

        using var resp = await _client.SendAsync(request, ct).ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();

        var fileId = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        _logger.LogInformation(
            "Export written to connector with file id {FileId} ({EaiMessageType} {FileName})",
            fileId,
            messageType,
            fileName);
        return new UploadResponse(fileId);
    }
}
