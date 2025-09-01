// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Cryptography.Asymmetric;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for cryptography.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services for cryptographic operations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddVotingLibCryptography(this IServiceCollection services)
    {
        services.TryAddSingleton<IAsymmetricAlgorithmAdapter<EcdsaPublicKey, EcdsaPrivateKey>, EcdsaAdapter>();
        return services;
    }
}
