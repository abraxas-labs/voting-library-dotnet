// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Voting.Lib.Iam.ServiceTokenHandling;

namespace Voting.Lib.Iam.AuthenticationScheme;

/// <inheritdoc />
/// <summary>
/// Options for secure connect authentication scheme.
/// </summary>
public class SecureConnectOptions : JwtBearerOptions
{
    /// <summary>
    /// Gets or sets account user for calls to the authorize endpoint to get the
    /// role token for the given subject.
    /// </summary>
    public string ServiceAccount { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets password for the provided service user account.
    /// </summary>
    public string ServiceAccountPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether when true, after the authentication the role token is fetched
    /// and evaluated to provide Authorization information.
    /// </summary>
    public bool FetchRoleToken { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether any role is required.
    /// Only taken into account if <see cref="FetchRoleToken"/> is <c>true</c>.
    /// If this is <c>true</c> the authentication is only successful, if the fetched role token
    /// has any role present on the tenant provided in the header named <see cref="TenantHeaderName"/>.
    /// </summary>
    public bool AnyRoleRequired { get; set; } = true;

    /// <summary>
    /// Gets or sets name of the used http header to select the actual used tenant.
    /// </summary>
    public string TenantHeaderName { get; set; } = "x-tenant";

    /// <summary>
    /// Gets or sets a default tenant id, if no value for <see cref="TenantHeaderName"/> is provided.
    /// </summary>
    public string? DefaultTenantId { get; set; }

    /// <summary>
    /// Gets or sets name of the used http header to select the actual used app.
    /// </summary>
    public string AppHeaderName { get; set; } = "x-app";

    /// <summary>
    /// Gets or sets accessor name that defines the claim that contains the permissions (roles)
    /// of the user in the role token.
    /// Only taken into account if <see cref="FetchRoleToken"/> is <c>true</c>.
    /// </summary>
    public string RoleClaimName { get; set; } = "permissions";

    /// <summary>
    /// Gets or sets the app shortcuts to request the role token for.
    /// Only used if <see cref="FetchRoleToken"/> is true.
    /// If this value is null, the <see cref="JwtBearerOptions.Audience"/> is used.
    /// If this field is set, all roles are prefixed with the shortcut of the app
    /// and separated by <see cref="RoleAppsSeparator"/>.
    /// </summary>
    public ICollection<string>? RoleTokenApps { get; set; }

    /// <summary>
    /// Gets or sets he separator used to separate the app shortcut from the role if
    /// the roles are requested via <see cref="RoleTokenApps"/>.
    /// </summary>
    public string RoleAppsSeparator { get; set; } = "::";

    /// <summary>
    /// Gets or sets a value indicating whether only roles for apps
    /// provided via the <see cref="AppHeaderName"/> are loaded.
    /// Values provided by <see cref="AppHeaderName"/> have to be a subset of <see cref="RoleTokenApps"/>
    /// or <see cref="JwtBearerOptions.Audience"/>.
    /// </summary>
    public bool LimitRolesToAppHeaderApps { get; set; } = true;

    /// <summary>
    /// Gets or sets a collection of service account scopes which the application is allowed to use.
    /// Expecting scopes to be passed without the prefix 'urn:abraxas:iam:audience_client_id:'.
    /// i.e. [ "EAWV", "NOTIFY-V2", "ADMIN" ].
    /// </summary>
    public ICollection<string>? ServiceAccountScopes { get; set; }

    /// <summary>
    /// Gets or sets the timeframe when a token is considered invalid before the real expiry for the token.
    /// This needs to be smaller than the actual lifetime of the service tokens issued by SecureConnect.
    /// <see cref="SecureConnectServiceAccountOptions.RefreshBeforeExpiration"/>.
    /// </summary>
    public TimeSpan ServiceTokenRefreshBeforeExpiration { get; set; } = TimeSpan.FromMinutes(2);
}
