﻿// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

namespace Voting.Lib.Cryptography.Exceptions;

/// <summary>
/// Exception thrown during cryptographic operations.
/// </summary>
[Serializable]
public class CryptographyException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CryptographyException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public CryptographyException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CryptographyException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public CryptographyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CryptographyException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected CryptographyException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }
}
