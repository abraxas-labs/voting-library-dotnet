// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Snapper;
using Xunit;

namespace Voting.Lib.Common.Test;

public class LanguagesTest
{
    [Fact]
    public void AllShouldReturnSorted()
    {
        Languages.All.ShouldMatchSnapshot();
    }
}
