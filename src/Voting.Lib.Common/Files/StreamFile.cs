// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Common.Files;

/// <summary>
/// An <see cref="IFile"/> implementation backed by a readable <see cref="Stream"/>.
/// Writes the stream's content to a <see cref="PipeWriter"/> and, optionally, disposes the source stream afterwards.
/// </summary>
/// <remarks>
/// The provided stream is read starting from its current position. The caller is responsible for positioning the stream as needed.
/// </remarks>
public class StreamFile : IFile
{
    private readonly Stream _fileStream;
    private readonly bool _disposeAfterWrite;

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamFile"/> class.
    /// </summary>
    /// <param name="fileStream">The source <see cref="Stream"/> to read from.</param>
    /// <param name="fileName">The logical file name, including extension.</param>
    /// <param name="mimeType">The MIME type of the content.</param>
    /// <param name="disposeAfterWrite">If set to <c>true</c>, disposes <paramref name="fileStream"/> after <see cref="Write(PipeWriter, CancellationToken)"/> completes (or fails); otherwise the caller retains ownership.</param>
    public StreamFile(Stream fileStream, string fileName, string mimeType, bool disposeAfterWrite = true)
    {
        _fileStream = fileStream;
        _disposeAfterWrite = disposeAfterWrite;
        FileName = fileName;
        MimeType = mimeType;
    }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets the MIME type of the content.
    /// </summary>
    public string MimeType { get; }

    /// <summary>
    /// Writes the file content to the specified <see cref="PipeWriter"/>.
    /// </summary>
    /// <param name="writer">The target <see cref="PipeWriter"/> to receive the content.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to observe while the copy operation is in progress.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    /// <remarks>
    /// This method does not complete or flush the provided <paramref name="writer"/>; the caller is responsible for doing so.
    /// If <c>disposeAfterWrite</c> was set to <c>true</c> in the constructor, the underlying stream is disposed when the copy completes or if an error occurs.
    /// </remarks>
    public async Task Write(PipeWriter writer, CancellationToken ct = default)
    {
        try
        {
            await _fileStream.CopyToAsync(writer.AsStream(), ct);
        }
        finally
        {
            if (_disposeAfterWrite)
            {
                await _fileStream.DisposeAsync();
            }
        }
    }
}
