// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class SystemClockTest
{
    [Fact]
    public void UtcShouldReturnDateTime()
    {
        var timestamp = DateTime.UtcNow;
        new SystemClock()
            .UtcNow
            .Should()
            .BeWithin(TimeSpan.FromSeconds(1))
            .After(timestamp);
    }
}
