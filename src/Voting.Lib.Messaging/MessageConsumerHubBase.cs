// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Messaging;

/// <summary>
/// A singleton bridge broadcasting messages to listeners.
/// (required since we need a singleton to broadcast to the listeners, but mass transit consumers are scoped).
/// </summary>
/// <typeparam name="TFilterMessage">The type of the message by which the listener can filter whether he is interested in the message.</typeparam>
/// <typeparam name="TListenerMessage">The type of the message which is consumed by the listener.</typeparam>
public abstract class MessageConsumerHubBase<TFilterMessage, TListenerMessage>
    where TListenerMessage : class
{
    private readonly ConcurrentDictionary<Guid, MessageConsumerRegistration<TFilterMessage, TListenerMessage>> _listeners = new();

    private readonly ILogger<MessageConsumerHubBase<TFilterMessage, TListenerMessage>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageConsumerHubBase{TFilterMessage, TListenerMessage}"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    protected MessageConsumerHubBase(ILogger<MessageConsumerHubBase<TFilterMessage, TListenerMessage>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets all consumers which are interested in the message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>Returns all consumers interested in the message.</returns>
    protected IEnumerable<MessageConsumerRegistration<TFilterMessage, TListenerMessage>> GetConsumers(TFilterMessage message)
        => _listeners.Values.Where(x => x.CanConsume(message));

    /// <summary>
    /// Registers a message consumer and listens for messages.
    /// </summary>
    /// <param name="registration">The message consumer registration.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. Returns after the cancellation token is cancelled.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the message consumer cannot be registered.</exception>
    protected async Task Listen(
        MessageConsumerRegistration<TFilterMessage, TListenerMessage> registration,
        CancellationToken cancellationToken)
    {
        var registrationId = Guid.NewGuid();

        _logger.LogDebug("Try adding listener with id {Id}", registrationId);

        if (!_listeners.TryAdd(registrationId, registration))
        {
            throw new InvalidOperationException("Could not add listener with id " + registrationId);
        }

        _logger.LogInformation("Event listener with id {Id} added ({Count})", registrationId, _listeners.Count);

        try
        {
            await cancellationToken.WaitForCancellation().ConfigureAwait(false);
        }
        finally
        {
            _listeners.Remove(registrationId, out _);
            _logger.LogInformation("Event listener with id {Id} removed ({Count})", registrationId, _listeners.Count);
        }
    }
}
