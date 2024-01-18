// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.DocPipe;

/// <summary>
/// Interface for the DocPipe service.
/// </summary>
public interface IDocPipeService
{
    /// <summary>
    /// Execute a DocPipe job.
    /// </summary>
    /// <param name="application">The application name.</param>
    /// <param name="jobVariables">The variables for this job.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the job result data.</typeparam>
    /// <returns>Returns the result of the job.</returns>
    Task<T?> ExecuteJob<T>(string application, Dictionary<string, string> jobVariables, CancellationToken ct = default);
}
