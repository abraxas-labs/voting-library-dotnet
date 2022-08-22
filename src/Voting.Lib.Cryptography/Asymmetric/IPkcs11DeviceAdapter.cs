// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// Interface for an adapter of a PKCS#11 cryptographic device.
/// </summary>
public interface IPkcs11DeviceAdapter
{
    /// <summary>
    /// Signs <paramref name="data"/> with the configured private key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <returns>A signature.</returns>
    byte[] CreateSignature(byte[] data);

    /// <summary>
    /// Verifies that <paramref name="signature"/> is created by the configured public key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="signature">The signature.</param>
    /// <returns>A result whether the signature is valid.</returns>
    bool VerifySignature(byte[] data, byte[] signature);

    /// <summary>
    /// Gets the health status of the PKCS#11 device connection.
    /// </summary>
    /// <returns>Returns whether a connection to the PKCS#11 device could be established and the login is successful.</returns>
    bool IsHealthy();
}
