// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Messaging;

/// <inheritdoc />
public class MessageConsumerHub<TFilterMessage, TListenerMessage>
    : MessageConsumerHubBase<TFilterMessage, TListenerMessage>
    where TListenerMessage : class
{
    private readonly ILogger<MessageConsumerHub<TFilterMessage, TListenerMessage>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageConsumerHub{TFilterMessage, TListenerMessage}"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public MessageConsumerHub(ILogger<MessageConsumerHub<TFilterMessage, TListenerMessage>> logger)
        : base(logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Adds a callback to the listeners.
    /// Method is blocking until the cancellation token is fired.
    /// </summary>
    /// <param name="filter">The filter, only messages where this callback returns true are passed to the listener.</param>
    /// <param name="listener">The delegate to callback on new events.</param>
    /// <param name="cancellationToken">The cancellation token which stops listening to events and returns the method.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. Blocks until the cancellation token fires.</returns>
    public Task Listen(
        Predicate<TFilterMessage> filter,
        Func<TListenerMessage, Task> listener,
        CancellationToken cancellationToken)
        => Listen(new MessageConsumerRegistration<TFilterMessage, TListenerMessage>(filter, listener), cancellationToken);

    internal Task Consume(TFilterMessage message, TListenerMessage listenerMessage)
    {
        var consumers = GetConsumers(message).ToList();

        _logger.LogDebug("Consuming message by {ConsumersCount} consumers", consumers.Count);

        var listenerTasks = consumers.Select(x => x.Consume(listenerMessage));

        return Task.WhenAll(listenerTasks);
    }
}

/// <summary>
/// A singleton bridge broadcasting messages to listeners.
/// (required since we need a singleton to broadcast to the listeners, but mass transit consumers are scoped).
/// </summary>
/// <typeparam name="TMessage">The type of the message by which the listener can filter whether he is interested in the message.</typeparam>
public class MessageConsumerHub<TMessage>
    : MessageConsumerHub<TMessage, TMessage>
    where TMessage : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageConsumerHub{TMessage}"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public MessageConsumerHub(ILogger<MessageConsumerHub<TMessage>> logger)
        : base(logger)
    {
    }
}
