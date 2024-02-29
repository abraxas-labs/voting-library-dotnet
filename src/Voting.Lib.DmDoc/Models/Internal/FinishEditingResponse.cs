// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;

internal class FinishEditingResponse : DmDocApiResponse
{
    public byte[]? ResultPdf { get; set; }
}
