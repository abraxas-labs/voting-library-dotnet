// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.ObjectStorage.Config;

/// <summary>
/// Configurations for the object storage (S3).
/// </summary>
public class ObjectStorageConfig
{
    /// <summary>
    /// Gets or sets the endpoint used for the S3 storage.
    /// Should include a hostname and a port (eg. localhost:9000).
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the access key.
    /// </summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the secret key.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to use ssl or not.
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// Gets or sets the replacement for the host and port of generated public download urls.
    /// If null, no replacement takes place.
    /// </summary>
    public string? PublicUrlHostReplacement { get; set; }

    /// <summary>
    /// Gets or sets the default ttl for public download links.
    /// </summary>
    public TimeSpan DefaultPublicDownloadLinkTtl { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets the default parameters which are set on public download links.
    /// </summary>
    public Dictionary<string, string> DefaultPublicDownloadLinkParameters { get; set; } = new()
    {
        ["Response-Content-Disposition"] = "attachment",
    };
}
