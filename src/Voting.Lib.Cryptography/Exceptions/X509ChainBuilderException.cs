// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Cryptography.Exceptions;

/// <summary>
/// Exception thrown during building X.509 chains.
/// </summary>
[Serializable]
public class X509ChainBuilderException(string message) : CryptographyException(message);
