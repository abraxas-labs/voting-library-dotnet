// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Rest.Middleware;

/// <summary>
/// An exception handler middleware.
/// </summary>
public abstract class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;
    private readonly bool _enableDetailedErrors;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
    /// </summary>
    /// <param name="next">The next request delegate.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="enableDetailedErrors">Whether to enable detailed errors. Should not be enabled in production environments.</param>
    protected ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger, bool enableDetailedErrors = false)
    {
        _next = next;
        _logger = logger;
        _enableDetailedErrors = enableDetailedErrors;
    }

    /// <summary>
    /// Invocation of this middleware. Catches all unhandled exception that happen during request processing.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "Global exception handler")]
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Handle an exception and map it to an error response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            return HandleExceptionWithStartedResponseAsync(context, exception);
        }

        var problem = GetProblemDetails(exception);
        if (problem.Status == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception caused internal server error");
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Unhandled exception mapped to http status code {StatusCode}",
                problem.Status);
        }

        var result = JsonSerializer.Serialize(problem);
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsync(result);
    }

    /// <summary>
    /// Handles an exception if the response has already started.
    /// Since the status code has already sent over the wire and cannot be changed anymore
    /// we do our best and just abort the request which should be recognized by the client or result in a generic error.
    /// </summary>
    /// <param name="context">The http context.</param>
    /// <param name="exception">The exception.</param>
    /// <returns>A task representing the async operation.</returns>
    protected virtual Task HandleExceptionWithStartedResponseAsync(HttpContext context, Exception exception)
    {
        var status = MapExceptionToStatus(exception);
        if (status == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception caused internal server error, aborted the response since it already started");
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Unhandled exception mapped to http status code {StatusCode}, aborted the response since it has already started, cannot set the exception status code",
                status);
        }

        context.Abort();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Map the exception to <see cref="ProblemDetails"/>.
    /// </summary>
    /// <param name="exception">The exception to map.</param>
    /// <returns>Returns the mapped problem details.</returns>
    protected virtual ProblemDetails GetProblemDetails(Exception exception)
        => new()
        {
            Title = _enableDetailedErrors || ExposeExceptionType(exception) ? exception.GetType().Name : nameof(Exception),
            Detail = _enableDetailedErrors || ExposeExceptionMessage(exception) ? exception.Message : null,
            Status = MapExceptionToStatus(exception),
        };

    /// <summary>
    /// Maps an exception to a corresponding HTTP status code.
    /// </summary>
    /// <param name="ex">The exception to map.</param>
    /// <returns>Returns the corresponding HTTP status code.</returns>
    protected virtual int MapExceptionToStatus(Exception ex) => StatusCodes.Status500InternalServerError;

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
}
