// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Cryptography.Exceptions;

/// <summary>
/// Exception thrown during PKCS11 operations.
/// </summary>
[Serializable]
public class Pkcs11Exception : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Pkcs11Exception"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public Pkcs11Exception(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pkcs11Exception"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public Pkcs11Exception(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
