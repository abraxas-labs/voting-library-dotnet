// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.Grpc.Interceptors;

namespace Voting.Lib.Grpc.Configuration;

/// <summary>
/// Configuration options for <see cref="RequestLoggerInterceptor"/>.
/// </summary>
public class RequestLoggerInterceptorConfig
{
    /// <summary>
    /// Gets or sets a list of request headers to add to the log output.
    /// </summary>
    public HashSet<string> RequestHeadersToLog { get; set; } = new()
    {
        "x-tenant",
        "x-app",
    };
}
