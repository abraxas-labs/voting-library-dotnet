// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using MassTransit;

namespace Voting.Lib.Messaging;

/// <summary>
/// A scoped proxy to forward messages to the message consumer hub (singleton).
/// (required since we need a singleton to broadcast to the listeners, but mass transit consumers are scoped).
/// </summary>
/// <typeparam name="TMessage">The type of the message and by which the listener can filter whether he is interested in the message.</typeparam>
/// <typeparam name="TListenerMessage">The type of the listener data message.</typeparam>
public abstract class MessageConsumer<TMessage, TListenerMessage> : IConsumer<TMessage>
    where TMessage : class
    where TListenerMessage : class
{
    private readonly MessageConsumerHub<TMessage, TListenerMessage> _hub;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageConsumer{TMessage, TListenerMessage}"/> class.
    /// </summary>
    /// <param name="hub">The message consumer hub.</param>
    protected MessageConsumer(MessageConsumerHub<TMessage, TListenerMessage> hub)
    {
        _hub = hub;
    }

    /// <inheritdoc />
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        var message = await Transform(context.Message).ConfigureAwait(false);
        if (message == null)
        {
            return;
        }

        await _hub.Consume(context.Message, message).ConfigureAwait(false);
    }

    /// <summary>
    /// Transform the received message into the listener message type.
    /// </summary>
    /// <param name="message">The received message.</param>
    /// <returns>Returns the transformed message.</returns>
    protected abstract Task<TListenerMessage?> Transform(TMessage message);
}

/// <summary>
/// A scoped proxy to forward messages to the message consumer hub (singleton).
/// (required since we need a singleton to broadcast to the listeners, but mass transit consumers are scoped).
/// </summary>
/// <typeparam name="TMessage">The type of the message and by which the listener can filter whether he is interested in the message.</typeparam>
public class MessageConsumer<TMessage> : MessageConsumer<TMessage, TMessage>
    where TMessage : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageConsumer{TMessage}"/> class.
    /// </summary>
    /// <param name="hub">The message consumer hub.</param>
    public MessageConsumer(MessageConsumerHub<TMessage> hub)
        : base(hub)
    {
    }

    /// <summary>
    /// Transforms the message. No-op in this class, since it is the same message type.
    /// </summary>
    /// <param name="message">The message to transform.</param>
    /// <returns>Returns the same message it received.</returns>
    protected override Task<TMessage?> Transform(TMessage message)
        => Task.FromResult<TMessage?>(message);
}
