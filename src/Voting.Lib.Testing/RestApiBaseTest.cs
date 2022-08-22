// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

namespace Voting.Lib.Testing;

/// <summary>
/// Base class for REST api tests.
/// </summary>
/// <typeparam name="TFactory">Factory to be used.</typeparam>
/// <typeparam name="TStartup">Startup class to be used.</typeparam>
public class RestApiBaseTest<TFactory, TStartup> : BaseTest<TFactory, TStartup>
    where TStartup : class
    where TFactory : BaseTestApplicationFactory<TStartup>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RestApiBaseTest{TFactory, TStartup}"/> class.
    /// </summary>
    /// <param name="factory">The test application factory.</param>
    public RestApiBaseTest(BaseTestApplicationFactory<TStartup> factory)
        : base(factory)
    {
    }

    /// <summary>
    /// Executes a given test code and asserts that the response matches the given status code.
    /// </summary>
    /// <param name="testCode">Function to test.</param>
    /// <param name="status">Expected status code.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async Task<HttpResponseMessage> AssertStatus(Func<Task<HttpResponseMessage>> testCode, HttpStatusCode status)
    {
        var response = await testCode().ConfigureAwait(false);
        response.StatusCode.Should().Be(status);
        return response;
    }
}
