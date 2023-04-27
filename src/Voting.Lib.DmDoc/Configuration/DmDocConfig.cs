// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.DmDoc.Configuration;

/// <summary>
/// DmDoc configuration.
/// </summary>
public class DmDocConfig
{
    /// <summary>
    /// Gets or sets the token used to access DmDoc.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the DmDoc username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base address of the dm doc api (incl. version and trailing slash).
    /// </summary>
    public Uri? BaseAddress { get; set; }

    /// <summary>
    /// Gets or sets the timeout used for dm doc calls.
    /// Use <c>null</c> for no timeout.
    /// </summary>
    public TimeSpan? Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the serialization format for the dm doc data containers.
    /// </summary>
    public DmDocDataSerializationFormat DataSerializationFormat { get; set; } = DmDocDataSerializationFormat.Json;

    /// <summary>
    /// Gets or sets the maximum attempts when polling the draft state.
    /// </summary>
    public int MaxDraftStatePollingAttempts { get; set; } = 10;

    /// <summary>
    /// Gets or sets the delay between draft state polling attempts.
    /// </summary>
    public TimeSpan DraftStatePollingDelay { get; set; } = TimeSpan.FromSeconds(1);
}
