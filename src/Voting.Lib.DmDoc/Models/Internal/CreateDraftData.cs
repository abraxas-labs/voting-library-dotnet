// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;

internal class CreateDraftData
{
    internal CreateDraftData(string mime, string data, string? bulkRoot = null)
    {
        Mime = mime;
        Data = data;
        BulkRoot = bulkRoot;
    }

    public string Mime { get; }

    public string Data { get; }

    public string? BulkRoot { get; }
}
