// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;

internal class TagBricksResponse
{
    public int[] AlreadeyTagged { get; set; } = [];

    public int[] NewlyTagged { get; set; } = [];
}
