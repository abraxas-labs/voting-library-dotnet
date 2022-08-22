// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Voting.Lib.Grpc.Interceptors;

/// <summary>
/// Base class to intercept requests.
/// Currently only unary and server streaming calls are supported.
/// </summary>
public abstract class RequestInterceptor : Interceptor
{
    /// <inheritdoc />
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        await InterceptRequest(request, context).ConfigureAwait(false);
        return await base.UnaryServerHandler(request, context, continuation).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        await InterceptRequest(request, context).ConfigureAwait(false);
        await base.ServerStreamingServerHandler(request, responseStream, context, continuation).ConfigureAwait(false);
    }

    /// <summary>
    /// Handler for intercepting a request.
    /// </summary>
    /// <param name="request">The request to intercept.</param>
    /// <param name="context">The call context.</param>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task InterceptRequest<TRequest>(TRequest request, ServerCallContext context);
}
