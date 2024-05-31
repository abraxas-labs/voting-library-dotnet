// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Iam.Exceptions;

/// <summary>
/// Exception for when a user is already authenticated.
/// </summary>
[Serializable]
public class AlreadyAuthenticatedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyAuthenticatedException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public AlreadyAuthenticatedException(string message)
        : base(message)
    {
    }
}
