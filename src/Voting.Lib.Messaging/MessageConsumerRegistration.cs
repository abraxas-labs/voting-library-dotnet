// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;

namespace Voting.Lib.Messaging;

/// <summary>
/// A registration for a message consumer.
/// </summary>
/// <typeparam name="TFilterMessage">The type of messages this registration is interested in.</typeparam>
/// <typeparam name="TListenerMessage">The type of listener messages.</typeparam>
public class MessageConsumerRegistration<TFilterMessage, TListenerMessage>
{
    private readonly Predicate<TFilterMessage> _filter;
    private readonly Func<TListenerMessage, Task> _consumer;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageConsumerRegistration{TFilterMessage, TListenerMessage}"/> class.
    /// </summary>
    /// <param name="filter">A predicate to filter messages that the consumer is interested in.</param>
    /// <param name="consumer">The message consumer function.</param>
    /// <param name="language">The language which should be used in the consumer function.</param>
    public MessageConsumerRegistration(Predicate<TFilterMessage> filter, Func<TListenerMessage, Task> consumer, string language = "")
    {
        _filter = filter;
        _consumer = consumer;
        Language = language;
    }

    /// <summary>
    /// Gets the language used in the consumer function.
    /// </summary>
    public string Language { get; }

    /// <summary>
    /// Checks if the consumer can consume this message.
    /// </summary>
    /// <param name="message">The message to check.</param>
    /// <returns>Returns whether the consumer can consume this message.</returns>
    public bool CanConsume(TFilterMessage message) => _filter(message);

    /// <summary>
    /// Passes the message to the message consumer.
    /// </summary>
    /// <param name="message">The message to consume.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Consume(TListenerMessage message) => _consumer(message);
}
