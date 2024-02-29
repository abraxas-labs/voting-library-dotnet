// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Grpc.Interceptors;

/// <summary>
/// gRPC Interceptor which maps exceptions to gRPC status codes.
/// </summary>
public abstract class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;
    private readonly bool _enableDetailedErrors;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionInterceptor"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="enableDetailedErrors">Whether to enable detailed error information. Should not be enabled in production environments.</param>
    protected ExceptionInterceptor(ILogger<ExceptionInterceptor> logger, bool enableDetailedErrors = false)
    {
        _logger = logger;
        _enableDetailedErrors = enableDetailedErrors;
    }

    /// <summary>
    /// Intercepts a request and handles all unhandled exceptions.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="context">The call context.</param>
    /// <param name="continuation">The continuation.</param>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>Returns the response.</returns>
    /// <exception cref="RpcException">Thrown when an exception happens.</exception>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.UnaryServerHandler(request, context, continuation).ConfigureAwait(false);
        }
        catch (Exception e) when (e is not RpcException)
        {
            var status = SetContextStatus(context, e);
            throw new RpcException(status);
        }
    }

    /// <summary>
    /// Intercepts a request and handles all unhandled exceptions.
    /// </summary>
    /// <param name="requestStream">The request stream.</param>
    /// <param name="context">The call context.</param>
    /// <param name="continuation">The continuation.</param>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>Returns the response.</returns>
    /// <exception cref="RpcException">Thrown when an exception happens.</exception>
    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.ClientStreamingServerHandler(requestStream, context, continuation).ConfigureAwait(false);
        }
        catch (Exception e) when (e is not RpcException)
        {
            var status = SetContextStatus(context, e);
            throw new RpcException(status);
        }
    }

    /// <summary>
    /// Intercepts a request and handles all unhandled exceptions.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="responseStream">The response stream.</param>
    /// <param name="context">The call context.</param>
    /// <param name="continuation">The continuation.</param>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="RpcException">Thrown when an exception happens.</exception>
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await base.ServerStreamingServerHandler(request, responseStream, context, continuation).ConfigureAwait(false);
        }
        catch (Exception e) when (e is not RpcException)
        {
            var status = SetContextStatus(context, e);
            throw new RpcException(status);
        }
    }

    /// <summary>
    /// Intercepts a request and handles all unhandled exceptions.
    /// </summary>
    /// <param name="requestStream">The request stream.</param>
    /// <param name="responseStream">The response stream.</param>
    /// <param name="context">The call context.</param>
    /// <param name="continuation">The continuation.</param>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="RpcException">Thrown when an exception happens.</exception>
    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation).ConfigureAwait(false);
        }
        catch (Exception e) when (e is not RpcException)
        {
            var status = SetContextStatus(context, e);
            throw new RpcException(status);
        }
    }

    /// <summary>
    /// Maps an exception to the corresponding status code.
    /// </summary>
    /// <param name="ex">The exception to map.</param>
    /// <returns>Returns the corresponding status code.</returns>
    protected abstract StatusCode MapExceptionToStatusCode(Exception ex);

    /// <summary>
    /// Decides whether the exception should be exposed or not.
    /// Note that in the http response an exception is always exposed when detailed errors are enabled, no matter what value is returned here.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <returns>Whether the exception type should be exposed.</returns>
    protected virtual bool ExposeExceptionType(Exception ex) => false;

    /// <summary>
    /// Decides whether the exception message should be exposed or not.
    /// Note that in the http response an exception message is always exposed when detailed errors are enabled, no matter what value is returned here.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <returns>Whether the exception message should be exposed.</returns>
    protected virtual bool ExposeExceptionMessage(Exception ex) => false;

    private Status SetContextStatus(ServerCallContext context, Exception ex)
    {
        var statusCode = MapExceptionToStatusCode(ex);

        if (statusCode is StatusCode.Unknown or StatusCode.Internal)
        {
            _logger.LogError(ex, "Uncaught exception happened");
        }
        else
        {
            _logger.LogError(ex, "Uncaught exception mapped to status code: {StatusCode}", statusCode);
        }

        // the frontend extracts the part before the : to display a custom error message.
        var msg = _enableDetailedErrors || ExposeExceptionType(ex)
            ? ex.GetType().Name
            : nameof(Exception);

        if (_enableDetailedErrors || ExposeExceptionMessage(ex))
        {
            msg += ": " + ex.Message;
        }

        return context.Status = new Status(statusCode, msg);
    }
}
