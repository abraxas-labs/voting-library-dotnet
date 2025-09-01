// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Voting.Lib.Cryptography.Exceptions;

namespace Voting.Lib.Cryptography.Kms.Exceptions;

/// <summary>
/// A KMS exception.
/// </summary>
[Serializable]
public class KmsException : CryptographyException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KmsException"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="codeDesc">The error code description.</param>
    /// <param name="message">The message.</param>
    public KmsException(int code, string codeDesc, string? message)
        : base($"{code}: {codeDesc} ({message ?? "Unknown KMS error"})")
    {
        Code = code;
        CodeDescription = codeDesc;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KmsException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public KmsException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KmsException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public KmsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the code of the error.
    /// </summary>
    public int? Code { get; }

    /// <summary>
    /// Gets the description of the <see cref="Code"/>.
    /// </summary>
    public string? CodeDescription { get; } = null;
}
