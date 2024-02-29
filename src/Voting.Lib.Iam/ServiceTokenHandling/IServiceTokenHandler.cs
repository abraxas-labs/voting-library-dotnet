// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;

namespace Voting.Lib.Iam.ServiceTokenHandling;

/// <summary>
/// Handles service token related stuff.
/// </summary>
public interface IServiceTokenHandler
{
    /// <summary>
    /// Returns a valid service token in its string representation.
    /// Caches internally until a token expires.
    /// </summary>
    /// <returns>The token.</returns>
    Task<string> GetServiceToken();
}
