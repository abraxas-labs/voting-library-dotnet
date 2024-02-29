// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;

namespace Voting.Lib.ApiClientBase.DotNet.Services;

/// <summary>
/// The abstraction for implementing an http client for a generated api service.
/// </summary>
public interface IServiceClientBase
{
    /// <summary>
    /// Gets or sets the handler for generating an http client.
    /// </summary>
    Func<HttpClient>? HttpClientGenerationHandler { get; set; }

    /// <summary>
    /// Gets or sets the base url for the generated http client.
    /// </summary>
    string? BaseUrl { get; set; }
}
