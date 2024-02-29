// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.DocPipe.Configuration;
using Voting.Lib.DocPipe.Extensions;
using Voting.Lib.DocPipe.Models;

namespace Voting.Lib.DocPipe;

/// <summary>
/// Implementation of the DocPipe service client.
/// </summary>
public class DocPipeService : IDocPipeService
{
    private readonly DocPipeConfig _config;
    private readonly IDocPipeUrlBuilder _urlBuilder;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocPipeService"/> class.
    /// </summary>
    /// <param name="config">The DocPipe configuration.</param>
    /// <param name="urlBuilder">The URL builder.</param>
    /// <param name="httpClient">The HTTP client.</param>
    public DocPipeService(
        DocPipeConfig config,
        IDocPipeUrlBuilder urlBuilder,
        HttpClient httpClient)
    {
        _config = config;
        _urlBuilder = urlBuilder;
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<T?> ExecuteJob<T>(string application, Dictionary<string, string> jobVariables, CancellationToken ct = default)
    {
        var url = _urlBuilder.Jobs();
        var request = new JobRequest(application, _config.Client, _config.Instance, jobVariables, _config.Timeout);
        var response = await _httpClient.PostDocPipe<JobRequest, JobResponse<T>>(url, request, ct).ConfigureAwait(false);
        return response == null
            ? default
            : response.CustomResultData;
    }
}
