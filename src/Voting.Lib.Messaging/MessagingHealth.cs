// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using MassTransit;
using Voting.Lib.Common;

namespace Voting.Lib.Messaging;

/// <summary>
/// Checks the health of the messaging system.
/// </summary>
public class MessagingHealth : IMessagingHealth
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    private readonly IBusControl _busControl;
    private readonly IClock _clock;
    private readonly object _refreshLock = new();
    private bool _isHealthy;
    private DateTime _lastRefresh;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagingHealth"/> class.
    /// </summary>
    /// <param name="busControl">The bus control.</param>
    /// <param name="clock">The clock.</param>
    public MessagingHealth(IBusControl busControl, IClock clock)
    {
        _busControl = busControl;
        _clock = clock;
    }

    /// <inheritdoc />
    public bool IsHealthy()
    {
        RefreshHealthIfNeeded();
        return _isHealthy;
    }

    private void RefreshHealthIfNeeded()
    {
        if (_clock.UtcNow - _lastRefresh < RefreshInterval)
        {
            return;
        }

        lock (_refreshLock)
        {
            if (_clock.UtcNow - _lastRefresh < RefreshInterval)
            {
                return;
            }

            _isHealthy = _busControl.CheckHealth().Status == BusHealthStatus.Healthy;
            _lastRefresh = _clock.UtcNow;
        }
    }
}
