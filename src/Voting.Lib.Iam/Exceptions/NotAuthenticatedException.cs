// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

namespace Voting.Lib.Iam.Exceptions;

/// <summary>
/// Exception for when a user is not authentication, but should be.
/// </summary>
[Serializable]
public class NotAuthenticatedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotAuthenticatedException"/> class.
    /// </summary>
    public NotAuthenticatedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotAuthenticatedException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected NotAuthenticatedException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }
}
