// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;
using Moq;
using Voting.Lib.Cryptography.Asymmetric;

namespace Voting.Lib.Cryptography.Test;

public static class HsmSimulatorTestContainer
{
    public const ushort HttpPort = 3001;
    public const string Hostname = "hsm-simulator";
    private const string Image = "harbor.abraxas-tools.ch/voting/hsm/cryptoserversimulator:4.45";

    public static async Task<IContainer> StartNew()
    {
        var container = new ContainerBuilder()
            .WithImage(Image)
            .WithHostname(Hostname)
            .WithPortBinding(HttpPort, HttpPort)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(HttpPort)
                .AddCustomWaitStrategy(new Pkcs11WaitStrategy()))
            .Build();

        await container.StartAsync();
        return container;
    }
}

internal class Pkcs11WaitStrategy : IWaitUntil
{
    /// <summary>
    /// Waits until the HSM simulator is fully operational by verifying the successful execution
    /// of a cryptographic operation. Simply establishing a session with the HSM does not guarantee
    /// readiness for cryptographic tasks.
    /// </summary>
    /// <param name="container">The test container instance.</param>
    /// <returns>True if the HSM simulator is ready for cryptographic operations, otherwise false.</returns>
    public Task<bool> UntilAsync(IContainer container)
    {
        try
        {
            HsmUtil.UpdateHsmSimulatorConfig(container);
            return Task.FromResult(new HsmAdapter(new Mock<ILogger<HsmAdapter>>().Object, null!)
                .CreateEcdsaSha384Signature([0x00], HsmUtil.GetPkcs11Config().WithEcdsa()) != null);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
