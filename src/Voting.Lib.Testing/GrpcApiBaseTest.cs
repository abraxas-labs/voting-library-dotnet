// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using Xunit;

namespace Voting.Lib.Testing;

/// <summary>
/// Base class for grpc api tests.
/// </summary>
/// <typeparam name="TFactory">Factory to be used.</typeparam>
/// <typeparam name="TStartup">Startup class to be used.</typeparam>
public class GrpcApiBaseTest<TFactory, TStartup> : BaseTest<TFactory, TStartup>
    where TFactory : BaseTestApplicationFactory<TStartup>
    where TStartup : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GrpcApiBaseTest{TFactory, TStartup}"/> class.
    /// </summary>
    /// <param name="factory">The test application factory.</param>
    public GrpcApiBaseTest(BaseTestApplicationFactory<TStartup> factory)
        : base(factory)
    {
    }

    /// <summary>
    /// Executes a given test code and asserts it throws an <see cref="RpcException"/> with a given status.
    /// </summary>
    /// <param name="testCode">Function to test.</param>
    /// <param name="status">Expected status code.</param>
    /// <param name="statusContent">Expected status content, compared with contains.</param>
    /// <param name="reason">Reason which is provided to the assertion comparison.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async Task<RpcException> AssertStatus(
        Func<Task> testCode,
        StatusCode status,
        string? statusContent = null,
        string reason = "")
    {
        var ex = await Assert.ThrowsAsync<RpcException>(testCode).ConfigureAwait(false);
        ex.StatusCode.Should().Be(status, reason);
        if (statusContent != null)
        {
            ex.Status.Detail.Should().Contain(statusContent);
        }

        return ex;
    }
}
