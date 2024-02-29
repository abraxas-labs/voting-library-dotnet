// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DokConnector.Models;

/// <summary>
/// The model for a DOK Connector upload response.
/// </summary>
public class UploadResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UploadResponse"/> class.
    /// </summary>
    /// <param name="fileId">The ID of the file that has been uploaded.</param>
    public UploadResponse(string fileId)
    {
        FileId = fileId;
    }

    /// <summary>
    /// Gets the ID of the file that has been uploaded.
    /// </summary>
    public string FileId { get; }
}
