// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Rest.Files;

/// <summary>
/// Represents a file.
/// </summary>
public interface IFile
{
    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    string Filename { get; }

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
}
