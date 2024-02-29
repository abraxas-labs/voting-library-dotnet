// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

namespace Voting.Lib.DocPipe.Exceptions;

/// <summary>
/// An exception for when something related to DocPipe goes wrong.
/// </summary>
[Serializable]
public class DocPipeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocPipeException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public DocPipeException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocPipeException"/> class.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public DocPipeException(Exception? innerException)
        : base("DocPipe call failed", innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocPipeException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DocPipeException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocPipeException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected DocPipeException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }
}
