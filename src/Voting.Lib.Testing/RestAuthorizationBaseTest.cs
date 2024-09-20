// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Voting.Lib.Testing;

/// <summary>
/// Base class for REST api tests which adds authorization base tests.
/// </summary>
/// <inheritdoc />
public abstract class RestAuthorizationBaseTest<TFactory, TStartup> : RestApiBaseTest<TFactory, TStartup>
    where TFactory : BaseTestApplicationFactory<TStartup>
    where TStartup : class
{
    /// <summary>
    /// The identifier to mark a user which is authenticated but doesn't have any roles assigned.
    /// </summary>
    protected const string NoRole = "no-role";

    /// <summary>
    /// Initializes a new instance of the <see cref="RestAuthorizationBaseTest{TFactory, TStartup}"/> class.
    /// </summary>
    /// <param name="factory">The test application factory.</param>
    protected RestAuthorizationBaseTest(BaseTestApplicationFactory<TStartup> factory)
        : base(factory)
    {
    }

    /// <summary>
    /// For an unauthorized user the status code unauthorized should be returned.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public Task Unauthorized()
        => AssertStatus(
            async () => await AuthorizationTestCall(CreateHttpClient(false)),
            HttpStatusCode.Unauthorized);

    /// <summary>
    /// For unauthorized users the status code forbidden should be returned.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task Forbidden()
    {
        foreach (var role in UnauthorizedRoles())
        {
            await AssertStatus(
                async () => await AuthorizationTestCall(
                    CreateHttpClient(role == NoRole ? Array.Empty<string>() : new[] { role })),
                HttpStatusCode.Forbidden);
        }
    }

    /// <summary>
    /// The testcall to test all authorization tests against.
    /// </summary>
    /// <param name="httpClient">The http client on which the test should be executed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient);

    /// <summary>
    /// An enumerable of roles which should lead to unauthorized access.
    /// </summary>
    /// <returns>An enumerable of roles.</returns>
    protected virtual IEnumerable<string> UnauthorizedRoles() => Array.Empty<string>();
}
