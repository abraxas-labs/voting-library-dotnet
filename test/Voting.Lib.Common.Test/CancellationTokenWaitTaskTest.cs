// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class CancellationTokenWaitTaskTest
{
    [Fact]
    public async Task Test()
    {
        using var cts = new CancellationTokenSource();
        var cancellationWaiter = cts.Token.WaitForCancellation();
        cancellationWaiter.IsCompleted.Should().BeFalse();
        cts.Cancel();

        await cancellationWaiter;
        cancellationWaiter.IsCompleted.Should().BeTrue();
    }
}
