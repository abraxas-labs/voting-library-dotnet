// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Voting.Lib.Cryptography.Pkcs11.Test;

internal static class HsmSimulatorTestContainer
{
    public const ushort HttpPort = 3001;
    public const string Hostname = "hsm-simulator";
    private const string Image = "harbor.abraxas-tools.ch/voting/hsm/cryptoserversimulator:4.51.0.1";

    public static async Task<IContainer> StartNew(ILogger logger)
    {
        var container = new ContainerBuilder()
            .WithImage(Image)
            .WithHostname(Hostname)
            .WithPortBinding(HttpPort, HttpPort)
            .WithLogger(logger)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(HttpPort)
                .AddCustomWaitStrategy(Pkcs11WaitStrategy.Instance))
            .Build();

        await container.StartAsync();
        return container;
    }
}

internal class Pkcs11WaitStrategy : IWaitUntil
{
    public static readonly IWaitUntil Instance = new Pkcs11WaitStrategy();

    private Pkcs11WaitStrategy()
    {
    }

    /// <summary>
    /// Waits until the HSM simulator is fully operational by verifying the successful execution
    /// of a cryptographic operation. Simply establishing a session with the HSM does not guarantee
    /// readiness for cryptographic tasks.
    /// </summary>
    /// <param name="container">The test container instance.</param>
    /// <returns>True if the HSM simulator is ready for cryptographic operations, otherwise false.</returns>
    public Task<bool> UntilAsync(IContainer container)
    {
        Pkcs11CryptoProvider? cryptoProvider = null;

        try
        {
            Pkcs11Config.UpdateHsmSimulatorConfig(container);

            cryptoProvider = new Pkcs11CryptoProvider(NullLogger<Pkcs11CryptoProvider>.Instance, Pkcs11Config.Instance);
            return Task.FromResult(
                cryptoProvider.CreateEcdsaSha384Signature([0x00], Pkcs11Config.CkaLabelEcdsaPrivate) != null);
        }
        catch
        {
            return Task.FromResult(false);
        }
        finally
        {
            cryptoProvider?.Dispose();
        }
    }
}
