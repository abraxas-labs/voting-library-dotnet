// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using FluentValidation;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Voting.Lib.Grpc.Interceptors;

/// <summary>
/// Interceptor which validates all request objects if an <see cref="IValidator{T}"/> for the request type exists
/// and is registered in the DI container.
/// </summary>
public class RequestFluentValidatorInterceptor : RequestInterceptor
{
    private readonly IServiceProvider _sp;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestFluentValidatorInterceptor"/> class.
    /// </summary>
    /// <param name="sp">The service provider.</param>
    public RequestFluentValidatorInterceptor(IServiceProvider sp)
    {
        _sp = sp;
    }

    /// <summary>
    /// Intercepts a request. If a validator for the request exists, perform validation on the request.
    /// </summary>
    /// <param name="request">The request to intercept and validate.</param>
    /// <param name="context">The call context.</param>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task InterceptRequest<TRequest>(TRequest request, ServerCallContext context)
    {
        var validator = _sp.GetService<IValidator<TRequest>>();
        if (validator != null)
        {
            await validator.ValidateAndThrowAsync(request, context.CancellationToken).ConfigureAwait(false);
        }
    }
}
