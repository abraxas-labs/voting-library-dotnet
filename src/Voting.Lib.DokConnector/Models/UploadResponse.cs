// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DokConnector.Models;

/// <summary>
/// The model for a DOK connector upload response.
/// </summary>
public class UploadResponse
{
    internal UploadResponse(string fileId)
    {
        FileId = fileId;
    }

    /// <summary>
    /// Gets the ID of the file that has been uploaded.
    /// </summary>
    public string FileId { get; }
}
