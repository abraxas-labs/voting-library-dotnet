// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Common.Files;

/// <summary>
/// Represents a file whose content is written asynchronously through a provided delegate
/// into a <see cref="PipeWriter"/>. This allows streaming scenarios where the file data
/// is generated or supplied on demand.
/// </summary>
public class PipedFile : IFile
{
    private readonly Func<PipeWriter, CancellationToken, Task> _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="PipedFile"/> class.
    /// </summary>
    /// <param name="writer">A delegate that performs the asynchronous write operation to a <see cref="PipeWriter"/>.</param>
    /// <param name="fileName">The logical file name, including extension.</param>
    /// <param name="mimeType">The MIME type of the content.</param>
    public PipedFile(Func<PipeWriter, CancellationToken, Task> writer, string fileName, string mimeType)
    {
        _writer = writer;
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
    /// Writes data to the provided <see cref="PipeWriter"/> using the underlying write delegate.
    /// </summary>
    /// <param name="writer">The <see cref="PipeWriter"/> to write data to.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the write operation.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    /// <remarks>
    /// This method simply forwards the call to the private write delegate <see cref="_writer"/>.
    /// </remarks>
    public Task Write(PipeWriter writer, CancellationToken ct = default) => _writer(writer, ct);
}
