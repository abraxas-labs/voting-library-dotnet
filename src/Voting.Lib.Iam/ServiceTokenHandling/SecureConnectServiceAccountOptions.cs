// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Voting.Lib.Iam.ServiceTokenHandling;

/// <summary>
/// Configuration for SecureConnect service accounts.
/// </summary>
public class SecureConnectServiceAccountOptions
{
    /// <summary>
    /// Gets or sets the authority (eg. https://accounts.abraxas.ch).
    /// Only required if <see cref="MetadataAddress"/> is <c>null</c>.
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the metadata address needs to start with https://.
    /// </summary>
    public bool RequireHttpsMetadata { get; set; } = true;

    /// <summary>
    /// Gets or sets the metadata address.
    /// If this is <c>null</c>, <see cref="Authority"/> is used with a '.well-known/openid-configuration' suffix.
    /// If this is not <c>null</c>, authority is not required.
    /// </summary>
    public string? MetadataAddress { get; set; }

    /// <summary>
    /// Gets or sets the user name of the service account.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the service account.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a collection of service account client id scopes.
    /// Expecting scopes to be passed without the prefix 'urn:abraxas:iam:audience_client_id:'.
    /// i.e. [ "EAWV", "NOTIFY-V2", "ADMIN" ].
    /// </summary>
    public ICollection<string>? ClientIdScopes { get; set; }

    /// <summary>
    /// Gets or sets additional scopes to <see cref="ClientIdScopes"/>.
    /// </summary>
    public ICollection<string>? Scopes { get; set; }

    /// <summary>
    /// Gets or sets the name of the http client which is used to fetch the service tokens.
    /// Can't be equal to the options name / http client name to which the token should be attached.
    /// </summary>
    public string? ServiceTokenClientName { get; set; } = "ServiceTokenHandler";

    /// <summary>
    /// Gets or sets the timeframe when a token is considered invalid before the real expiry for the token.
    /// This needs to be smaller than the actual lifetime of the service tokens issued by SecureConnect.
    /// </summary>
    public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromMinutes(2);

    internal ConfigurationManager<OpenIdConnectConfiguration> ConfigurationManager { get; set; } = default!; // initialized during post configuration

    internal void ApplyFrom(SecureConnectServiceAccountOptions options)
    {
        Authority = options.Authority;
        RequireHttpsMetadata = options.RequireHttpsMetadata;
        MetadataAddress = options.MetadataAddress;
        UserName = options.UserName;
        Password = options.Password;
        ClientIdScopes = options.ClientIdScopes;
        Scopes = options.Scopes;
        ServiceTokenClientName = options.ServiceTokenClientName;
        RefreshBeforeExpiration = options.RefreshBeforeExpiration;
    }
}
