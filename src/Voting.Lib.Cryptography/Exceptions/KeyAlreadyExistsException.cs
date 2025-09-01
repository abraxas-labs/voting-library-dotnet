// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Cryptography.Exceptions;

/// <summary>
/// Exception that indicates that a key to be created already exists.
/// </summary>
[Serializable]
public class KeyAlreadyExistsException(string message) : CryptographyException(message);
