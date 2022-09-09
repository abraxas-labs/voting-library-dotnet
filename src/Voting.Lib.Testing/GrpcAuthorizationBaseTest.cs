// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Xunit;

namespace Voting.Lib.Testing;

/// <summary>
/// Base class for grpc api tests which adds authorization base tests.
/// </summary>
/// <inheritdoc />
public abstract class GrpcAuthorizationBaseTest<TFactory, TStartup> : GrpcApiBaseTest<TFactory, TStartup>
    where TFactory : BaseTestApplicationFactory<TStartup>
    where TStartup : class
{
    /// <summary>
    /// The identifier to mark a user which is authenticated but doesn't have any roles assigned.
    /// </summary>
    protected const string NoRole = "no-role";

    /// <summary>
    /// Initializes a new instance of the <see cref="GrpcAuthorizationBaseTest{TFactory, TStartup}"/> class.
    /// </summary>
    /// <param name="factory">The test application factory.</param>
    protected GrpcAuthorizationBaseTest(BaseTestApplicationFactory<TStartup> factory)
        : base(factory)
    {
    }

    /// <summary>
    /// For an unauthenticated user the status code unauthenticated should be thrown.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public Task Unauthenticated()
        => AssertStatus(
            async () => await AuthorizationTestCall(CreateGrpcChannel(false)).ConfigureAwait(false),
            StatusCode.Unauthenticated);

    /// <summary>
    /// For unauthorized users the status code permission denied should be thrown.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task PermissionDenied()
    {
        foreach (var role in UnauthorizedRoles())
        {
            await AssertStatus(
                async () => await AuthorizationTestCall(
                    CreateGrpcChannel(role == NoRole ? Array.Empty<string>() : new[] { role })).ConfigureAwait(false),
                StatusCode.PermissionDenied,
                null,
                $"Permission denied failed for role {role}").ConfigureAwait(false);
        }
    }

    /// <summary>
    /// The testcall to test all authorization tests against.
    /// </summary>
    /// <param name="channel">The grpc channel on which the test should be executed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task AuthorizationTestCall(GrpcChannel channel);

    /// <summary>
    /// An enumerable of roles which should lead to unauthorized access.
    /// </summary>
    /// <returns>An enumerable of roles.</returns>
    protected virtual IEnumerable<string> UnauthorizedRoles() => Array.Empty<string>();
}
