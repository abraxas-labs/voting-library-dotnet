// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Class responsible for handling the event processing.
/// </summary>
/// <typeparam name="TScope">The type of event processing scope.</typeparam>
public class EventProcessingHandler<TScope>
    where TScope : IEventProcessorScope
{
    private readonly ILogger<EventProcessingHandler<TScope>> _logger;
    private readonly EventProcessorAdapterRegistry<TScope> _processorRegistry;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessingHandler{TScope}"/> class.
    /// </summary>
    /// <param name="processorRegistry">The event processor registry.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="scopeFactory">The service scope factory.</param>
    public EventProcessingHandler(
        EventProcessorAdapterRegistry<TScope> processorRegistry,
        ILogger<EventProcessingHandler<TScope>> logger,
        IServiceScopeFactory scopeFactory)
    {
        _processorRegistry = processorRegistry;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Handles an event which appeared on a subscription.
    /// </summary>
    /// <param name="subscription">The subscription on which the event appeared.</param>
    /// <param name="eventData">The event which appeared.</param>
    /// <returns>
    /// A boolean value, which indicates whether there was a processor which handled the event (true)
    /// or no processor for this event type was registered (false).
    /// </returns>
    internal async Task<bool> HandleEvent(Subscription<TScope> subscription, ResolvedEvent eventData)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["Scope"] = subscription.ScopeName,
            ["EventType"] = eventData.Event.EventType,
            ["EventId"] = eventData.OriginalEvent.EventId,
            ["Position"] = eventData.OriginalEvent.Position,
            ["OriginalEventNumber"] = eventData.OriginalEventNumber,
        });
        _logger.LogDebug("Received event");

        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var processor = _processorRegistry.GetProcessorAdapter(scope.ServiceProvider, eventData.Event.EventType);
            if (processor == null)
            {
                _logger.LogDebug("No processor for {EventType}", eventData.Event.EventType);
                return false;
            }

            var eventProcessingScope = scope.ServiceProvider.GetRequiredService<TScope>();
            await eventProcessingScope.Begin(eventData.OriginalEvent.Position, eventData.OriginalEventNumber).ConfigureAwait(false);
            await processor.Process(eventData.Event.Data, subscription.IsCatchUp).ConfigureAwait(false);
            await eventProcessingScope.Complete(eventData.OriginalEvent.Position, eventData.OriginalEventNumber).ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process event");
            throw;
        }
    }

    internal async Task HandleCatchUpCompleted(Subscription<TScope> subscription)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object> { ["Scope"] = subscription.ScopeName });
        await using var scope = _scopeFactory.CreateAsyncScope();
        _logger.LogInformation("Catch up of scope completed");

        var eventCatchUpCompleters = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventProcessorCatchUpCompleter<TScope>>>();
        foreach (var completer in eventCatchUpCompleters)
        {
            await completer.CatchUpCompleted().ConfigureAwait(false);
        }
    }
}
