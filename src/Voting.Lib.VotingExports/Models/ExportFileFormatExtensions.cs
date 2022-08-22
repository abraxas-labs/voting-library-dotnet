// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Mime;

namespace Voting.Lib.VotingExports.Models;

/// <summary>
/// Extensions for the <see cref="ExportFileFormat"/>.
/// </summary>
public static class ExportFileFormatExtensions
{
    private const string CsvMimeType = "text/csv";

    /// <summary>
    /// Resolves the mime type of a file format.
    /// </summary>
    /// <param name="fileType">The format of the file.</param>
    /// <returns>The mime type.</returns>
    /// <exception cref="ArgumentException">If the format is unknown.</exception>
    public static string GetMimeType(this ExportFileFormat fileType)
    {
        return fileType switch
        {
            ExportFileFormat.Csv => CsvMimeType,
            ExportFileFormat.Pdf => MediaTypeNames.Application.Pdf,
            ExportFileFormat.Xml => MediaTypeNames.Application.Xml,
            _ => throw new ArgumentException($"Invalid file format {fileType}"),
        };
    }
}
