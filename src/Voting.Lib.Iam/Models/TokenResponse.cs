// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.Models;

internal class TokenResponse
{
    public string? AccessToken { get; set; }

    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the number of seconds, how long the <see cref="AccessToken"/> is valid.
    /// </summary>
    public long ExpiresIn { get; set; }
}
