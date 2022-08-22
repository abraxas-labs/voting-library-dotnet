// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Voting.Lib.Iam.AuthenticationScheme;

internal class SecureConnectPostConfigureOptions : JwtBearerPostConfigureOptions,
    IPostConfigureOptions<SecureConnectOptions>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SecureConnectPostConfigureOptions(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public void PostConfigure(string name, SecureConnectOptions options)
    {
        if (options.Backchannel != null)
        {
            throw new InvalidOperationException(
                "SecureConnect does not support using custom http clients," +
                "instead you can configure the http client via the http client factory and its backchannel http client name." +
                "This is required to ensure dotnet doesn't create an http client without using the factory and therefore not using" +
                "additional voting library mechanisms such as certificate pinning.");
        }

        options.Backchannel = _httpClientFactory.CreateClient(SecureConnectDefaults.BackchannelHttpClientName);

        base.PostConfigure(name, options);

        if (options.FetchRoleToken &&
            (string.IsNullOrWhiteSpace(options.ServiceAccount) ||
             string.IsNullOrWhiteSpace(options.ServiceAccountPassword)))
        {
            throw new InvalidOperationException(
                "When FetchRoleToken is set, the ServiceAccount and the ServiceAccountPassword must be set.");
        }
    }
}
