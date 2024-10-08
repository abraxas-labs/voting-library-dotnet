// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.DmDoc.Exceptions;

/// <summary>
/// An exception for when something related to DmDoc goes wrong.
/// </summary>
[Serializable]
public class DmDocException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public DmDocException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocException"/> class.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public DmDocException(Exception? innerException)
        : base("DmDoc call failed", innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DmDocException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
