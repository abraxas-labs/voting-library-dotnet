// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Voting.Lib.Ech.Utils;
using Xunit;

namespace Voting.Lib.Ech.Test.Utils;

public class BallotQuestionIdConverterTest
{
    [Theory]
    [InlineData("47466b60-b2b4-4f6f-a9f8-489f751556a0", false, 1, "47466b60-b2b4-4f6f-a9f8-489f751556a0_n_1")]
    [InlineData("08bafdd3-54f2-47b6-8d76-0a54f11b6fc6", true, 2, "08bafdd3-54f2-47b6-8d76-0a54f11b6fc6_t_2")]
    public void ShouldConvertToEchIdAndBack(string ballotId, bool isTieBreakQuestion, int number, string echQuestionId)
    {
        var ballotGuid = Guid.Parse(ballotId);

        var converted = BallotQuestionIdConverter.ToEchBallotQuestionId(ballotGuid, isTieBreakQuestion, number);
        converted.Should().Be(echQuestionId);

        var (convertedBallotId, convertedIsTieBreak, convertedNumber) = BallotQuestionIdConverter.FromEchBallotQuestionId(converted);
        convertedBallotId.Should().Be(ballotGuid);
        isTieBreakQuestion.Should().Be(convertedIsTieBreak);
        number.Should().Be(convertedNumber);
    }
}
