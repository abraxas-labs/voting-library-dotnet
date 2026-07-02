// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.Services.ApiClient.Identity;

namespace Voting.Lib.Iam.Models;

/// <summary>
/// A second factor.
/// </summary>
public class SecondFactor
{
    internal SecondFactor(V1SecondFactorProvider provider, SecondFactorNevisInfo? nevis = null)
    {
        Provider = provider;
        Nevis = nevis;
    }

    /// <summary>
    /// Gets the second factor provider used.
    /// </summary>
    public V1SecondFactorProvider Provider { get; }

    /// <summary>
    /// Gets the info about the nevis second factor.
    /// </summary>
    public SecondFactorNevisInfo? Nevis { get; init; }
}
