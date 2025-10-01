// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Common.Files;

/// <summary>
/// Represents a ZIP archive composed from multiple <see cref="IFile"/> entries.
/// </summary>
public class ZipFile : IFile
{
    private readonly IAsyncEnumerable<IFile> _files;
    private readonly DateTimeOffset? _zipEntryLastWriteTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZipFile"/> class.
    /// </summary>
    /// <param name="files">The sequence of files to include as entries in the ZIP archive.</param>
    /// <param name="fileName">The name of the resulting ZIP file (e.g., "files.zip").</param>
    /// <param name="zipEntryLastWriteTime">Optional last write time to set on each ZIP entry; if <c>null</c>, the current time is used.</param>
    private ZipFile(
        IAsyncEnumerable<IFile> files,
        string fileName,
        DateTimeOffset? zipEntryLastWriteTime)
    {
        FileName = fileName;
        _files = files;
        _zipEntryLastWriteTime = zipEntryLastWriteTime;
    }

    /// <inheritdoc />
    public string FileName { get; }

    /// <inheritdoc />
    public string MimeType => MediaTypeNames.Application.Zip;

    /// <summary>
    /// Creates a new <see cref="ZipFile"/> representing the provided files.
    /// </summary>
    /// <param name="files">The sequence of files to include as entries in the ZIP archive.</param>
    /// <param name="fileName">The name of the resulting ZIP file (e.g., "files.zip").</param>
    /// <param name="zipEntryLastWriteTime">Optional last write time to set on each ZIP entry; if <c>null</c>, the current time is used.</param>
    /// <returns>A <see cref="ZipFile"/> instance.</returns>
    public static ZipFile Create(
        IAsyncEnumerable<IFile> files,
        string fileName,
        DateTimeOffset? zipEntryLastWriteTime = null)
        => new ZipFile(files, fileName, zipEntryLastWriteTime);

    /// <inheritdoc />
    public async Task Write(PipeWriter writer, CancellationToken ct = default)
    {
        using var archive = new ZipArchive(writer.AsStream(), ZipArchiveMode.Create, true);
        await foreach (var fileModel in _files.WithCancellation(ct).ConfigureAwait(false))
        {
            var zipEntry = archive.CreateEntry(fileModel.FileName);
            if (_zipEntryLastWriteTime.HasValue)
            {
                zipEntry.LastWriteTime = _zipEntryLastWriteTime.Value;
            }

            await using var zipEntryStream = zipEntry.Open();

            var entryWriter = PipeWriter.Create(zipEntryStream);
            await fileModel.Write(entryWriter, ct).ConfigureAwait(false);
        }
    }
}
