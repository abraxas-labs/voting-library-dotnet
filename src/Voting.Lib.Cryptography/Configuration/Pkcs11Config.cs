// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Configuration;

/// <summary>
/// Configuration for PKCS11.
/// </summary>
public class Pkcs11Config
{
    /// <summary>
    /// Gets or sets the path of the unmanaged PKCS#11 library.
    /// </summary>
    public string LibraryPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CKA_LABEL which is required to get the stored Public Key of the device
    /// used for asymmetric algorithms.
    /// </summary>
    public string PublicKeyCkaLabel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CKA_LABEL which is required to get the stored Private Key of the device
    /// used for asymmetric algorithms.
    /// </summary>
    public string PrivateKeyCkaLabel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CKA_LABEL which is required to get the stored Secret Key of the device
    /// used for symmetric algorithms.
    /// </summary>
    public string SecretKeyCkaLabel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Login Pin so that a User can login into the device.
    /// </summary>
    public string LoginPin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the slot id of the device.
    /// </summary>
    public ulong SlotId { get; set; }
}
