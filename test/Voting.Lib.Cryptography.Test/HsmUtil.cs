// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using DotNet.Testcontainers.Containers;
using Voting.Lib.Cryptography.Configuration;

namespace Voting.Lib.Cryptography.Test;

public static class HsmUtil
{
    private const string Pkcs11ConfigFileName = "cs_pkcs11_R3.cfg";
    private const string Pkcs11CryptoServerDevice = "TCP:3001@127.0.0.1";
    private const string LocalIpAddress = "127.0.0.1";
    private const string UnixLibraryPath = "./libcs_pkcs11_R3.so";
    private const string WindowsLibraryPath = "./cs_pkcs11_R3.dll";

    /// <summary>
    /// The PKCS#11 driver (libcs_pkcs11_R3.so) is configured to connect to the HSM simulator using the endpoint specified in the cs_pkcs11_R3.cfg file.
    /// This configuration file, which is part of the test execution binaries, is dynamically updated during runtime to include the IP address and port
    /// assigned by the TestContainers tool. Since the PKCS#11 driver does not support hostname resolution, the dynamically assigned IP address must be
    /// used directly. The endpoint is updated to ensure the PKCS#11 driver connects to the correct HSM simulator instance.
    /// </summary>
    /// <param name="hsmSimulatorContainer">The HSM simulator test container.</param>
    public static void UpdateHsmSimulatorConfig(IContainer hsmSimulatorContainer)
    {
        var ipAddress = hsmSimulatorContainer.Hostname == LocalIpAddress ? LocalIpAddress : hsmSimulatorContainer.IpAddress;
        var endpoint = $"TCP:{hsmSimulatorContainer.GetMappedPublicPort(HsmSimulatorTestContainer.HttpPort)}@{ipAddress}";

        var configContent = File.ReadAllText(Pkcs11ConfigFileName)
            .Replace(Pkcs11CryptoServerDevice, endpoint);

        File.WriteAllText(Pkcs11ConfigFileName, configContent);
    }

    public static Pkcs11Config GetPkcs11Config() => new()
    {
        LibraryPath = Environment.OSVersion.Platform == PlatformID.Unix ? UnixLibraryPath : WindowsLibraryPath,
        SlotId = 0,
        LoginPin = "1234",
    };

    public static Pkcs11Config WithEcdsa(
        this Pkcs11Config config,
        string privateKeyCkaLabel = "VOSR_ECDSA_PRIVATE_KEY_PRE",
        string publicKeyCkaLabel = "VOSR_ECDSA_PUBLIC_KEY_PRE")
    {
        config.PrivateKeyCkaLabel = privateKeyCkaLabel;
        config.PublicKeyCkaLabel = publicKeyCkaLabel;
        return config;
    }

    public static Pkcs11Config WithRsa(
        this Pkcs11Config config,
        string privateKeyCkaLabel = "privvotingpre",
        string publicKeyCkaLabel = "pubvotingpre")
    {
        config.PrivateKeyCkaLabel = privateKeyCkaLabel;
        config.PublicKeyCkaLabel = publicKeyCkaLabel;
        return config;
    }

    public static Pkcs11Config WithAes(
        this Pkcs11Config config,
        string secretKeyCkaLabel = "VOSR_AES_KEY_PRE")
    {
        config.SecretKeyCkaLabel = secretKeyCkaLabel;
        return config;
    }
}
