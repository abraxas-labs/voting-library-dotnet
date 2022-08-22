// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Iam.Store;

namespace Voting.Lib.Rest.Middleware;

/// <summary>
/// Adds IAM log scopes to REST calls.
/// </summary>
public class IamLoggingHandler
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="IamLoggingHandler"/> class.
    /// </summary>
    /// <param name="next">The next request delegate.</param>
    public IamLoggingHandler(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invocation of this middleware. Starts a log scope with added authentication information.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        var auth = serviceProvider.GetRequiredService<AuthStore>();
        using var logScope = auth.StartLogScope();
        await _next(context).ConfigureAwait(false);
    }
}
