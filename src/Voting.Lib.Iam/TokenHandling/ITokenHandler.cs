// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// Handles token related stuff.
/// </summary>
public interface ITokenHandler
{
    /// <summary>
    /// Returns a valid token in its string representation.
    /// Caches internally until a token expires.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The token.</returns>
    Task<string> GetToken(CancellationToken cancellationToken);
}
