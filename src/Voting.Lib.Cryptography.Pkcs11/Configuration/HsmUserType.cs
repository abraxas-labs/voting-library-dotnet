// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Pkcs11.Configuration;

/// <summary>
/// HSM user type which gets mapped to the PKCS11 CKU type.
/// </summary>
public enum HsmUserType
{
    /// <summary>
    /// Security Officer, corresponds to CKU_SO.
    /// </summary>
    SecurityOfficer,

    /// <summary>
    /// Normal user, corresponds to CKU_USER.
    /// </summary>
    User,

    /// <summary>
    /// Context specific, corresponds to CKU_CONTEXT_SPECIFIC.
    /// </summary>
    ContextSpecific,

    /// <summary>
    /// Proprietary user type for Utimaco HSM which is used for users with the Key Manager (KM) role.
    /// </summary>
    UtimacoGeneric,
}
