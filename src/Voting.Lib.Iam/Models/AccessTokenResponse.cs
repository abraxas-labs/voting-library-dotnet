// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.Models;

internal class AccessTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of seconds, how long the <see cref="AccessToken"/> is valid.
    /// </summary>
    public long ExpiresIn { get; set; }
}
