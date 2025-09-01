// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Pkcs11.Configuration;

/// <summary>
/// Configuration for PKCS11.
/// </summary>
public record Pkcs11Config
{
    /// <summary>
    /// Gets or sets the path of the unmanaged PKCS#11 library.
    /// </summary>
    public string LibraryPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Login user type so that a User can login into the device.
    /// </summary>
    public HsmUserType LoginUserType { get; set; } = HsmUserType.User;

    /// <summary>
    /// Gets or sets the Login Pin so that a User can login into the device.
    /// </summary>
    public string LoginPin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the slot id of the device.
    /// </summary>
    public ulong SlotId { get; set; }
}
