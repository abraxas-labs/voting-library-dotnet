// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.DokConnector.Models;

internal class FileUploadRequest
{
    public string MessageType { get; set; } = string.Empty;

    [Obsolete("Removed once migration to DokConnectorApi provider is complete.")]
    public string UserName { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;
}
