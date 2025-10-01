// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.SecondFactor.Exceptions;

/// <summary>
/// The data of the 2fa transaction changed since it was created.
/// </summary>
[Serializable]
public class SecondFactorTransactionDataChangedException : Exception;
