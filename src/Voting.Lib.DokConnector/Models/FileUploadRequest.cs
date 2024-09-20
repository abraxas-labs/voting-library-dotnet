// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DokConnector.Models;

internal class FileUploadRequest
{
    public string MessageType { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;
}
