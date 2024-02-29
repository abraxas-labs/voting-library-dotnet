// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Messaging;

/// <summary>
/// The message producer buffer buffers messages until TryComplete is called.
/// This ensures messages are only broadcasted after the transaction has completed and the read model is up to date.
/// (called by the EventProcessingScope).
/// </summary>
public sealed class MessageProducerBuffer
{
    private readonly List<object> _buffer = new();
    private readonly ILogger<MessageProducerBuffer> _logger;
    private readonly IPublishEndpoint _publisher;
    private readonly IMessagingHealth _health;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageProducerBuffer"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="publisher">The publisher.</param>
    /// <param name="health">The messaging health.</param>
    public MessageProducerBuffer(ILogger<MessageProducerBuffer> logger, IPublishEndpoint publisher, IMessagingHealth health)
    {
        _logger = logger;
        _publisher = publisher;
        _health = health;
    }

    /// <summary>
    /// Try to publish all buffered messages. In case an exception occurs or the messaging system isn't healthy,
    /// this method will log the error and return without throwing an exception.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task TryComplete()
    {
        if (_buffer.Count == 0)
        {
            return;
        }

        // since messaging is not used for mission critical data in voting
        // we simply skip publishing these messages if the service is not available.
        if (!_health.IsHealthy())
        {
            _logger.LogError("Message bus is not healthy, skipping message publishing");
            return;
        }

        try
        {
            await _publisher.PublishBatch(_buffer).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Event publishing failed");
        }
        finally
        {
            // The buffer is cleared in both, the successful and the failure case.
            // This ensures that if a single message has a problem to be published,
            // further message batches will be tried to publish without the problematic messages.
            _buffer.Clear();
        }
    }

    /// <summary>
    /// Add a message to this buffer.
    /// </summary>
    /// <param name="eventData">The message to add.</param>
    public void Add(object eventData)
        => _buffer.Add(eventData);
}
