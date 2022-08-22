// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.VotingExports.Models;

/// <summary>
/// The file format of an export.
/// </summary>
public enum ExportFileFormat
{
    /// <summary>
    /// Unspecified.
    /// </summary>
    Unspecified,

    /// <summary>
    /// CSV.
    /// </summary>
    Csv,

    /// <summary>
    /// PDF.
    /// </summary>
    Pdf,

    /// <summary>
    /// XML.
    /// </summary>
    Xml,
}
