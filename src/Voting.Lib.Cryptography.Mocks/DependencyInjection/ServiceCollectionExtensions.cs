// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Cryptography;
using Voting.Lib.Cryptography.Mocks;
using Voting.Lib.Cryptography.Mocks.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for cryptography mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a PKCS#11 mock interface to perform cryptographic functions.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The crypto mock config.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddVotingLibCryptoProviderMock(this IServiceCollection services, CryptoMockConfig? config = null)
    {
        config ??= new CryptoMockConfig();

        return services
            .RemoveAll<ICryptoProvider>()
            .AddSingleton(config)
            .AddSingleton<CryptoProviderMock>()
            .AddSingleton<ICryptoProvider>(sp => sp.GetRequiredService<CryptoProviderMock>());
    }
}
