// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Configuration;

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
        return services
            .AddSingleton<IAsymmetricAlgorithmAdapter<EcdsaPublicKey, EcdsaPrivateKey>, EcdsaAdapter>();
    }

    /// <summary>
    /// Adds a PKCS#11 interface perform cryptographic functions.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The configuration of the pkcs11 interface.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddVotingLibPkcs11(this IServiceCollection services, Pkcs11Config config)
    {
        return services.AddSingleton(config)
            .AddSingleton<IPkcs11DeviceAdapter, HsmAdapter>();
    }
}
