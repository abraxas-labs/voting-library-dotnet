// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Cryptography.Kms.ApiModels;

[Flags]
internal enum KeyUsageMask
{
    /// <summary>
    /// Sign data.
    /// </summary>
    Sign = 1 << 0,

    /// <summary>
    /// Verify signatures.
    /// </summary>
    Verify = 1 << 1,

    /// <summary>
    /// Encrypt plaintexts.
    /// </summary>
    Encrypt = 1 << 2,

    /// <summary>
    /// Decrypt ciphertexts.
    /// </summary>
    Decrypt = 1 << 3,

    /// <summary>
    /// Generate MACs.
    /// </summary>
    MacGenerate = 1 << 7,

    /// <summary>
    /// Verify MACs.
    /// </summary>
    MacVerify = 1 << 8,

    /// <summary>
    /// <see cref="Sign"/> and <see cref="Verify"/>.
    /// </summary>
    SignAndVerify = Sign | Verify,

    /// <summary>
    /// <see cref="Encrypt"/> and <see cref="Decrypt"/>.
    /// </summary>
    EncryptAndDecrypt = Encrypt | Decrypt,

    /// <summary>
    /// <see cref="MacGenerate"/> and <see cref="MacVerify"/>.
    /// </summary>
    Mac = MacGenerate | MacVerify,
}
