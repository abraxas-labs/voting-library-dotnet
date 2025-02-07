// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;
internal class TagBricksResponse
{
    public int[] AlreadeyTagged { get; set; } = System.Array.Empty<int>();

    public int[] NewlyTagged { get; set; } = System.Array.Empty<int>();
}
