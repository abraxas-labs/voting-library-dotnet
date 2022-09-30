// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;

internal class UpdateBrickContentRequest
{
    public bool Checkin { get; set; }

    public string Content { get; set; } = string.Empty;
}
