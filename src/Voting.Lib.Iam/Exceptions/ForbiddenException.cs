// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

namespace Voting.Lib.Iam.Exceptions;

/// <summary>
/// Exception for when access to a resource is forbidden.
/// </summary>
[Serializable]
public class ForbiddenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    public ForbiddenException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public ForbiddenException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected ForbiddenException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }
}
