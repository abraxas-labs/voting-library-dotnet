// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Voting.Lib.Cryptography.Exceptions;

namespace Voting.Lib.Cryptography.Kms.Exceptions;

/// <summary>
/// The key to be created is already existing.
/// </summary>
/// <param name="name">The name of the key.</param>
/// <param name="message">The exception message.</param>
[Serializable]
public class KmsKeyAlreadyExistsException(string name, string? message)
    : KeyAlreadyExistsException($"A key with name {name} already exists:{message}")
{
    internal const int KmsErrorCode = 416;
}
