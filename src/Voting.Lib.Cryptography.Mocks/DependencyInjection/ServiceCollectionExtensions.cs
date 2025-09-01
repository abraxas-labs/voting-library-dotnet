// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Cryptography;
using Voting.Lib.Cryptography.Mocks;

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
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddVotingLibCryptoProviderMock(this IServiceCollection services)
    {
        return services
            .RemoveAll<ICryptoProvider>()
            .AddSingleton<CryptoProviderMock>()
            .AddSingleton<ICryptoProvider>(sp => sp.GetRequiredService<CryptoProviderMock>());
    }
}
