// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class EnvironmentVariablesFixerTest
{
    [Fact]
    public void ShouldAddEscapedVariables()
    {
        Environment.SetEnvironmentVariable("Foo", "Bar");
        Environment.SetEnvironmentVariable("Foo_DOT_Bar", "FooBar");
        Environment.SetEnvironmentVariable("Foo_DOT_Bar_DOT_Baz", "FooBarBaz");
        EnvironmentVariablesFixer.FixDotEnvironmentVariables();
        Environment.GetEnvironmentVariable("Foo").Should().Be("Bar");
        Environment.GetEnvironmentVariable("Foo_DOT_Bar").Should().Be("FooBar");
        Environment.GetEnvironmentVariable("Foo.Bar").Should().Be("FooBar");
        Environment.GetEnvironmentVariable("Foo_DOT_Bar_DOT_Baz").Should().Be("FooBarBaz");
        Environment.GetEnvironmentVariable("Foo.Bar.Baz").Should().Be("FooBarBaz");
    }
}
