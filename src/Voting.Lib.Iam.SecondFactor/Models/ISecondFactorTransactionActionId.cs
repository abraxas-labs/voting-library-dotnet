// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.SecondFactor.Models;

/// <summary>
/// The action id for second factor transactions.
/// Uniquely identifies the action the second factor is requested for.
/// </summary>
public interface ISecondFactorTransactionActionId
{
    /// <summary>
    /// The hash representing this action.
    /// </summary>
    /// <returns>The hash of the action.</returns>
    string ComputeHash();
}
