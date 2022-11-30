// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Voting.Lib.Grpc.Interceptors;

/// <summary>
/// Grpc request intercepter to log and write all grpc requests including payload.
/// </summary>
public class RequestLoggerInterceptor : RequestInterceptor
{
    private const string LogTypeProperty = "GrpcRequestLogType";
    private readonly ILogger<RequestLoggerInterceptor> _logger;
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestLoggerInterceptor"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="environment">The web host environment.</param>
    public RequestLoggerInterceptor(
        ILogger<RequestLoggerInterceptor> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <inheritdoc/>
    protected override Task InterceptRequest<TRequest>(TRequest request, ServerCallContext context)
    {
        if (!_environment.IsDevelopment())
        {
            return Task.CompletedTask;
        }

        if (request is not IMessage protoRequest)
        {
            return Task.CompletedTask;
        }

        using (LogContext.PushProperty(LogTypeProperty, true))
        {
            _logger.LogDebug(
                "{Uri}{NewLine}{RequestData}{NewLine}",
                context.Method,
                Environment.NewLine,
                protoRequest.ToString(),
                Environment.NewLine);
        }

        return Task.CompletedTask;
    }
}
