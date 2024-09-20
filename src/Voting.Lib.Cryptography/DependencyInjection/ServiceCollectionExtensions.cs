// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Net.Pkcs11Interop.HighLevelAPI;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for cryptography.
/// </summary>
public static class ServiceCollectionExtensions
{
    private static bool _dllImportResolverRegistered = false;

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
        if (!_dllImportResolverRegistered)
        {
            NativeLibrary.SetDllImportResolver(typeof(Pkcs11InteropFactories).Assembly, CustomDllImportResolver);
            _dllImportResolverRegistered = true;
        }

        return services.AddSingleton(config)
            .AddSingleton<IPkcs11DeviceAdapter, HsmAdapter>();
    }

    /// <summary>
    /// Custom DLL import resolver for resolving native library imports for the Pkcs11Interop assembly.
    /// This resolver creates a mapping from "libdl" to "libdl.so.2" to resolve the native library import
    /// used by the Pkcs11Interop assembly.
    /// Reference: https://github.com/Pkcs11Interop/pkcs11-logger/blob/master/test/Pkcs11LoggerTests/Settings.cs#L101-L113.
    /// </summary>
    /// <param name="libraryName">The name of the library to be resolved.</param>
    /// <param name="assembly">The assembly that requires the library.</param>
    /// <param name="dllImportSearchPath">The search path for the library.</param>
    /// <returns>The resolved library handle.</returns>
    private static IntPtr CustomDllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? dllImportSearchPath)
    {
        string mappedLibraryName = (libraryName == "libdl") ? "libdl.so.2" : libraryName;
        return NativeLibrary.Load(mappedLibraryName, assembly, dllImportSearchPath);
    }
}
