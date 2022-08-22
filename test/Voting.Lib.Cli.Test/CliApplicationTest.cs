// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Cli.Test;

public class CliApplicationTest
{
    [Fact]
    public Task Test() => Run(99, "hello", "-n", "Hans");

    [Fact]
    public Task TestLongNameArg() => Run(99, "hello", "--name", "Hans");

    [Fact]
    public Task TestVersion() => Run(ExitCodes.Ok, "version");

    [Fact]
    public Task TestMissingArg() => Run(ExitCodes.ParserError, "hello");

    [Fact]
    public Task TestCouldNotParse() => Run(ExitCodes.ParserError, "fooBar");

    private async Task Run(int expectedExitCode, params string[] args)
    {
        var result = await CliApplication.Run<CliTestApp>(args);
        result.Should().Be(expectedExitCode);
    }
}
