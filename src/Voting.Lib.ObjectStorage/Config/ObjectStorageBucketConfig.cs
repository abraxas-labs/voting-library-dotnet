// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.ObjectStorage.Config;

/// <summary>
/// Bucket specific configuration for the object storage client.
/// </summary>
public class ObjectStorageBucketConfig
{
    /// <summary>
    /// Gets or sets the name of the bucket.
    /// </summary>
    public string BucketName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a prefix which is added to each object name.
    /// </summary>
    public string ObjectPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a default ttl for public download links.
    /// </summary>
    public TimeSpan? DefaultPublicDownloadLinkTtl { get; set; }

    /// <summary>
    /// Gets or sets the default parameters which are set on public download links.
    /// </summary>
    public Dictionary<string, string>? DefaultPublicDownloadLinkParameters { get; set; }
}
