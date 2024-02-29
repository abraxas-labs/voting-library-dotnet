// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Messaging.Testing.Mocks;

/// <summary>
/// Mock for messaging health.
/// </summary>
public class MessagingHealthMock : IMessagingHealth
{
    private readonly bool _isHealthy;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagingHealthMock"/> class.
    /// </summary>
    /// <param name="isHealthy">The static value indicating whether the messaging broker is healthy.</param>
    public MessagingHealthMock(bool isHealthy)
    {
        _isHealthy = isHealthy;
    }

    /// <inheritdoc />
    // In the test we always use healthy since the in memory harness of MassTransit doesnt publish health states.
    public bool IsHealthy() => _isHealthy;
}
