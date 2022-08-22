// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Iam.Services;

/// <summary>
/// Configuration for the SecureConnect API.
/// </summary>
public class SecureConnectApiOptions
{
    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public Uri? BaseUrl { get; set; }

    internal Uri IdentityUrl => BuildUri("identity");

    internal Uri PermissionUrl => BuildUri("permission");

    private Uri BuildUri(string path)
    {
        if (BaseUrl == null)
        {
            throw new InvalidOperationException($"{nameof(BaseUrl)} is not set but required");
        }

        return new Uri($"{BaseUrl.ToString().TrimEnd('/')}/{path}/");
    }
}
