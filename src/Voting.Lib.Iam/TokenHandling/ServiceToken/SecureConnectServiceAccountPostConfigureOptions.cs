// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Voting.Lib.Iam.AuthenticationScheme;

namespace Voting.Lib.Iam.TokenHandling.ServiceToken;

internal class SecureConnectServiceAccountPostConfigureOptions : IPostConfigureOptions<SecureConnectServiceAccountOptions>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SecureConnectServiceAccountPostConfigureOptions(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void PostConfigure(string? name, SecureConnectServiceAccountOptions options)
    {
        EnsureFieldNotWhiteSpace(options.UserName);
        EnsureFieldNotWhiteSpace(options.Password);
        BuildConfigurationManager(options);
    }

    private void BuildConfigurationManager(SecureConnectServiceAccountOptions options)
    {
        // build oidc configuration manager, see Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerPostConfigureOptions
        options.MetadataAddress ??= options.Authority;
        EnsureFieldNotWhiteSpace(options.MetadataAddress);

        if (!options.MetadataAddress.EndsWith("/", StringComparison.Ordinal))
        {
            options.MetadataAddress += "/";
        }

        options.MetadataAddress += ".well-known/openid-configuration";

        if (options.RequireHttpsMetadata && !options.MetadataAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"The MetadataAddress or Authority must use HTTPS unless disabled for development by setting {nameof(options.RequireHttpsMetadata)}=false.");
        }

        var backchannel = _httpClientFactory.CreateClient(SecureConnectDefaults.BackchannelHttpClientName);

        options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            options.MetadataAddress,
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetriever(backchannel) { RequireHttps = options.RequireHttpsMetadata });
    }

    private void EnsureFieldNotWhiteSpace([NotNull] string? field, [CallerArgumentExpression("field")] string? fieldName = null)
    {
        if (!string.IsNullOrWhiteSpace(field))
        {
            return;
        }

        throw new InvalidOperationException($"{fieldName} must be set to a non white space value");
    }
}
