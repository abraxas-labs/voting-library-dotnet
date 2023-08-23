// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Testing.Mocks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The service collection extensions for the cryptography mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds mock services for cryptographic operations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddVotingLibCryptographyMocks(this IServiceCollection services)
        => services.AddVotingLibCryptographyMocks(HashAlgorithmName.SHA512);

    /// <summary>
    /// Adds mock services for cryptographic operations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="hashAlgorithm">The hash algorithm to use in the mocks.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddVotingLibCryptographyMocks(this IServiceCollection services, HashAlgorithmName hashAlgorithm)
        => services
            .RemoveAll<IAsymmetricAlgorithmAdapter<EcdsaPublicKey, EcdsaPrivateKey>>()
            .AddSingleton(new AsymmetricAlgorithmAdapterMock(hashAlgorithm))
            .AddSingleton<IAsymmetricAlgorithmAdapter<EcdsaPublicKey, EcdsaPrivateKey>>(sp => sp.GetRequiredService<AsymmetricAlgorithmAdapterMock>());
}
