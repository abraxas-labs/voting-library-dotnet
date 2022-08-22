// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Cryptography.Asymmetric;
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
    public static IServiceCollection AddVotingLibPkcs11Mock(this IServiceCollection services)
    {
        return services
            .RemoveAll<IPkcs11DeviceAdapter>()
            .AddSingleton<Pkcs11DeviceAdapterMock>()
            .AddSingleton<IPkcs11DeviceAdapter>(sp => sp.GetRequiredService<Pkcs11DeviceAdapterMock>());
    }
}
