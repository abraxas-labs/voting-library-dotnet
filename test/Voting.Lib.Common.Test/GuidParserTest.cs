// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class GuidParserTest
{
    [Fact]
    public void ParseShouldWork()
    {
        GuidParser.Parse("c693f620-4101-475a-bf81-0ccb32369de6")
            .Should()
            .Be(Guid.Parse("c693f620-4101-475a-bf81-0ccb32369de6"));
    }

    [Fact]
    public void ParseShouldThrow()
    {
        Assert.Throws<ValidationException>(() => GuidParser.Parse("test"))
            .Message
            .Should()
            .Be("Could not parse GUID test");
    }

    [Fact]
    public void ParseNullableShouldWorkWithNull()
    {
        GuidParser.ParseNullable(null)
            .Should()
            .BeNull();
    }

    [Fact]
    public void ParseNullableShouldWorkWithEmptyString()
    {
        GuidParser.ParseNullable(string.Empty)
            .Should()
            .BeNull();
    }

    [Fact]
    public void ParseNullableShouldWork()
    {
        GuidParser.ParseNullable("c693f620-4101-475a-bf81-0ccb32369de6")
            .Should()
            .Be(Guid.Parse("c693f620-4101-475a-bf81-0ccb32369de6"));
    }

    [Fact]
    public void ParseNullableShouldThrow()
    {
        Assert.Throws<ValidationException>(() => GuidParser.ParseNullable("test"))
            .Message
            .Should()
            .Be("Could not parse GUID test");
    }
}
