// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.SecondFactor.Exceptions;

/// <summary>
/// The 2fa transaction is not verified.
/// </summary>
[Serializable]
public class SecondFactorTransactionNotVerifiedException : Exception;
