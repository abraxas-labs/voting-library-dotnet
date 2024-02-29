// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Rest.Files;

/// <summary>
/// A file result that uses a callback to request the file contents.
/// </summary>
// https://blog.stephencleary.com/2016/11/streaming-zip-on-aspnet-core.html
// https://jira.eia.abraxas.ch/jira/browse/VOTING-468
public class FileCallbackResult : FileResult
{
    private readonly Func<PipeWriter, Task> _callback;

    internal FileCallbackResult(string contentType, string fileName, Func<PipeWriter, Task> callback)
        : base(contentType)
    {
        _callback = callback;
        FileDownloadName = fileName;
    }

    /// <inheritdoc />
    public override Task ExecuteResultAsync(ActionContext context)
    {
        var executor = new FileCallbackResultExecutor(context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>());
        return executor.ExecuteAsync(context, this);
    }

    private sealed class FileCallbackResultExecutor : FileResultExecutorBase
    {
        public FileCallbackResultExecutor(ILoggerFactory loggerFactory)
            : base(CreateLogger<FileCallbackResultExecutor>(loggerFactory))
        {
        }

        public Task ExecuteAsync(ActionContext context, FileCallbackResult result)
        {
            SetHeadersAndLog(context, result, null, false);
            return result._callback(context.HttpContext.Response.BodyWriter);
        }
    }
}
