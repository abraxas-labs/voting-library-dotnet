// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Net.Pkcs11Interop.Common;
using Voting.Lib.Cryptography.Exceptions;

namespace Voting.Lib.Cryptography.Pkcs11.Exceptions;

/// <summary>
/// Exception thrown if a key already exists during PKCS11 key generation.
/// </summary>
[Serializable]
public class Pkcs11KeyAlreadyExistsException : KeyAlreadyExistsException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Pkcs11KeyAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="ckaKeyType">The CKA_CLASS of the key.</param>
    /// <param name="ckaLabel">The CKA_LABEL of the key.</param>
    public Pkcs11KeyAlreadyExistsException(CKO ckaKeyType, string ckaLabel)
        : base($"Key of type {ckaKeyType} with label {ckaLabel} already exists")
    {
    }
}
