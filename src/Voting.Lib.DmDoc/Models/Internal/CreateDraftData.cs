// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;

internal class CreateDraftData
{
    internal CreateDraftData(string mime, string data)
    {
        Mime = mime;
        Data = data;
    }

    public string Mime { get; }

    public string Data { get; }
}
