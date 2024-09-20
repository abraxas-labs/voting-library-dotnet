// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

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
}
