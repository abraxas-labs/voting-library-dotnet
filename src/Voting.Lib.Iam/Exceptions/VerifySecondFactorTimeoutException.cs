// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Iam.Exceptions;

/// <summary>
/// Exception for when the verification of a second factor has timed out.
/// </summary>
[Serializable]
public class VerifySecondFactorTimeoutException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifySecondFactorTimeoutException"/> class.
    /// </summary>
    public VerifySecondFactorTimeoutException()
    {
    }
}
