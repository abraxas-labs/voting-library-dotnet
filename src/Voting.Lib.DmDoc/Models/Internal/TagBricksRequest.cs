// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;

internal class TagBricksRequest
{
    public string TagInternName { get; set; } = string.Empty;

    public int[]? BrickIds { get; set; }
}
