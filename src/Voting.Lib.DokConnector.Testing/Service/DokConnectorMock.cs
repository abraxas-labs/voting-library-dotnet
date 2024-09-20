// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Voting.Lib.DokConnector.Models;
using Voting.Lib.DokConnector.Service;
using Voting.Lib.DokConnector.Testing.Models;

namespace Voting.Lib.DokConnector.Testing.Service;

/// <summary>
/// A mock for the DOK connector.
/// </summary>
public class DokConnectorMock : IDokConnector
{
    private readonly Channel<MockData> _channel = Channel.CreateUnbounded<MockData>();
    private int _counter;

    /// <summary>
    /// Mocks the upload of a file and stores it in memory.
    /// </summary>
    /// <param name="messageType">The message type.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="fileContent">The file stream.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a mocked upload response.</returns>
    public async Task<UploadResponse> Upload(string messageType, string fileName, Stream fileContent, CancellationToken ct)
    {
        await using var ms = new MemoryStream();
        await fileContent.CopyToAsync(ms, ct).ConfigureAwait(false);
        await _channel.Writer.WriteAsync(new MockData(fileName, messageType, ms.ToArray()), ct).ConfigureAwait(false);
        return new UploadResponse("mock-file-id-" + _counter++);
    }

    /// <summary>
    /// Mocks the upload of a file and stores it in memory.
    /// </summary>
    /// <param name="messageType">The message type.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="writer">The file writer.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a mocked upload response.</returns>
    public async Task<UploadResponse> Upload(
        string messageType,
        string fileName,
        Func<PipeWriter, CancellationToken, Task> writer,
        CancellationToken ct)
    {
        await using var ms = new MemoryStream();
        var pipeWriter = PipeWriter.Create(ms, new StreamPipeWriterOptions());
        await writer(pipeWriter, ct).ConfigureAwait(false);
        await pipeWriter.FlushAsync(ct).ConfigureAwait(false);
        await _channel.Writer.WriteAsync(new MockData(fileName, messageType, ms.ToArray()), ct).ConfigureAwait(false);
        return new UploadResponse("mock-file-id-" + _counter++);
    }

    /// <summary>
    /// Retrieve a file that was uploaded.
    /// </summary>
    /// <param name="timeOut">The time out after which the operation will be canceled.</param>
    /// <returns>Returns the uploaded mock data.</returns>
    public async Task<MockData> NextUpload(TimeSpan timeOut)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(timeOut);
        return await NextUpload(cts.Token).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve a file that was uploaded.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the uploaded mock data.</returns>
    public ValueTask<MockData> NextUpload(CancellationToken ct)
        => _channel.Reader.ReadAsync(ct);
}
