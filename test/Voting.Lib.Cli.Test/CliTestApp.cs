// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using FluentAssertions;

namespace Voting.Lib.Cli.Test;

public class CliTestApp : CliStartup
{
    [Verb("hello")]
    public class MyCommandOptions
    {
        [Option('n', "name", Required = true)]
        public string Name { get; set; } = string.Empty;
    }

    public class MyCommand : ICommand<MyCommandOptions>
    {
        public Task<int> Run(MyCommandOptions options, CancellationToken ct)
        {
            options.Name.Should().Be("Hans");
            return Task.FromResult(99);
        }
    }
}
