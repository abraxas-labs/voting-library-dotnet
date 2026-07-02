// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Common;

/// <summary>
/// An actionId uniquely identifies an action in a system.
/// </summary>
public interface IActionId
{
    /// <summary>
    /// Computes the hash of the action id.
    /// </summary>
    /// <returns>The hash of the action id.</returns>
    string ComputeHash();
}
