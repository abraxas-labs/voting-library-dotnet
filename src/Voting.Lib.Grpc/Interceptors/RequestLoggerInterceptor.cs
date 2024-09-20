// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Voting.Lib.Grpc.Configuration;

namespace Voting.Lib.Grpc.Interceptors;

/// <summary>
/// Grpc request intercepter to log and write all grpc requests including payload.
/// </summary>
public class RequestLoggerInterceptor : RequestInterceptor
{
    private const string LogTypeProperty = "GrpcRequestLogType";
    private readonly ILogger<RequestLoggerInterceptor> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly RequestLoggerInterceptorConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestLoggerInterceptor"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="environment">The web host environment.</param>
    /// <param name="config">The interceptor configuration.</param>
    public RequestLoggerInterceptor(
        ILogger<RequestLoggerInterceptor> logger,
        IWebHostEnvironment environment,
        RequestLoggerInterceptorConfig config)
    {
        _logger = logger;
        _environment = environment;
        _config = config;
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
                "{RequestPath}{NewLine}{RequestData}{NewLine}{RequestHeaders}{NewLine}",
                context.Method,
                Environment.NewLine,
                protoRequest.ToString(),
                Environment.NewLine,
                GetRequestHeaders(context),
                Environment.NewLine);
        }

        return Task.CompletedTask;
    }

    private string GetRequestHeaders(ServerCallContext context)
    {
        var sb = new StringBuilder();

        foreach (var header in _config.RequestHeadersToLog)
        {
            var headerValue = context.RequestHeaders.GetValue(header);
            if (string.IsNullOrEmpty(headerValue))
            {
                continue;
            }

            sb.Append(header).Append(": ").AppendLine(headerValue);
        }

        return sb.ToString();
    }
}
