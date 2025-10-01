// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Common.Files;

/// <summary>
/// Represents a file.
/// </summary>
public interface IFile
{
    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    string FileName { get; }

    /// <summary>
    /// Gets the MIME type of the file.
    /// </summary>
    string MimeType { get; }

    /// <summary>
    /// Write the file to the specified <see cref="PipeWriter"/>.
    /// </summary>
    /// <param name="writer">The writer to write the file to.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Write(PipeWriter writer, CancellationToken ct = default);

    /// <summary>
    /// Get the file content as a readable <see cref="Stream"/> without buffering.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A readable stream that streams the file content.</returns>
    Stream AsStream(CancellationToken ct = default)
    {
        var pipe = new Pipe();

        _ = Task.Run(
            async () =>
            {
                try
                {
                    await Write(pipe.Writer, ct).ConfigureAwait(false);
                }
                finally
                {
                    await pipe.Writer.CompleteAsync().ConfigureAwait(false);
                }
            },
            cancellationToken: ct);

        return pipe.Reader.AsStream(leaveOpen: false);
    }
}
