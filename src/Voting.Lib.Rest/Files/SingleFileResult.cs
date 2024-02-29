// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Voting.Lib.Rest.Files;

/// <summary>
/// Helper class to create a single file result.
/// </summary>
public static class SingleFileResult
{
    /// <summary>
    /// Creates a ZIP file result.
    /// </summary>
    /// <param name="files">The list of files.</param>
    /// <param name="zipName">The name of the resulting ZIP file.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a file result for a single file.</returns>
    public static FileResult CreateZipFile(
        IAsyncEnumerable<IFile> files,
        string zipName,
        CancellationToken ct = default)
    {
        return new FileCallbackResult(MediaTypeNames.Application.Zip, zipName, async bodyWriter =>
        {
            using var archive = new ZipArchive(bodyWriter.AsStream(), ZipArchiveMode.Create, true);
            await foreach (var fileModel in files.WithCancellation(ct).ConfigureAwait(false))
            {
                var zipEntry = archive.CreateEntry(fileModel.Filename);

                await using var zipEntryStream = zipEntry.Open();

                var writer = PipeWriter.Create(zipEntryStream);
                await fileModel.Write(writer, ct).ConfigureAwait(false);
            }
        });
    }

    /// <summary>
    /// Creates a file result.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a file result for a single file.</returns>
    public static FileResult Create(IFile file, CancellationToken ct = default)
    {
        return new FileCallbackResult(file.MimeType, file.Filename, writer => file.Write(writer, ct));
    }

    /// <summary>
    /// Creates a file result.
    /// </summary>
    /// <param name="mimeType">The content type.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="writerFunction">The writer function.</param>
    /// <returns>Returns a file result for a single file.</returns>
    public static FileResult Create(string mimeType, string fileName, Func<PipeWriter, Task> writerFunction)
    {
        return new FileCallbackResult(mimeType, fileName, writerFunction);
    }
}
