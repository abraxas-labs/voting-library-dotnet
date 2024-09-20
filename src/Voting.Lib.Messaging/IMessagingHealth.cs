// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Messaging;

/// <summary>
/// Interface for messaging health.
/// </summary>
public interface IMessagingHealth
{
    /// <summary>
    /// Checks whether the messaging system is healthy.
    /// </summary>
    /// <returns>Returns true if the messaging system is healthy.</returns>
    bool IsHealthy();
}
