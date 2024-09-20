// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Voting.Lib.Eventing.Diagnostics;
using Voting.Lib.Eventing.Read;
using EventTypeFilter = EventStore.Client.EventTypeFilter;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Service that is responsible for starting the event processing subscription.
/// </summary>
/// <typeparam name="TScope">The type of event processing scope.</typeparam>
public class EventProcessingSubscriber<TScope> : IHostedService
    where TScope : IEventProcessorScope
{
    private readonly EventStoreClient _client;
    private readonly EventProcessingHandler<TScope> _eventProcessingHandler;
    private readonly ILogger<EventProcessingSubscriber<TScope>> _logger;
    private readonly Subscription<TScope> _subscription;
    private readonly EventTypeResolver _eventTypeResolver;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEventProcessingRetryPolicy<TScope> _retryPolicy;
    private readonly IEventReader _reader;

    private StreamSubscription? _streamSubscriptionConnection;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessingSubscriber{TScope}"/> class.
    /// </summary>
    /// <param name="client">The EventStore client.</param>
    /// <param name="eventProcessingHandler">The event processing handler to use.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="subscription">The subscription.</param>
    /// <param name="eventTypeResolver">The event type resolver.</param>
    /// <param name="scopeFactory">The service scope factory.</param>
    /// <param name="retryPolicy">The event retry policy.</param>
    /// <param name="reader">The event reader.</param>
    public EventProcessingSubscriber(
        EventStoreClient client,
        EventProcessingHandler<TScope> eventProcessingHandler,
        ILogger<EventProcessingSubscriber<TScope>> logger,
        Subscription<TScope> subscription,
        EventTypeResolver eventTypeResolver,
        IServiceScopeFactory scopeFactory,
        IEventProcessingRetryPolicy<TScope> retryPolicy,
        IEventReader reader)
    {
        _client = client;
        _eventProcessingHandler = eventProcessingHandler;
        _logger = logger;
        _subscription = subscription;
        _eventTypeResolver = eventTypeResolver;
        _scopeFactory = scopeFactory;
        _retryPolicy = retryPolicy;
        _reader = reader;
    }

    /// <summary>
    /// Starts the event processing subscription.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var latestEvent = await _reader.GetLatestEvent(_subscription.StreamName, cancellationToken).ConfigureAwait(false);
        _subscription.CatchUpEndsPosition = latestEvent?.Position ?? Position.Start;
        _subscription.CatchUpEndsEventNumber = latestEvent?.EventNumber ?? StreamPosition.Start;
        EventingMeter.SubscriptionCatchUpEnds(_subscription.ScopeName, _subscription.CatchUpEndsPosition.CommitPosition, _subscription.CatchUpEndsEventNumber);

        _logger.LogInformation("Trying to connect the subscription…");
        await ConnectToSubscription().ConfigureAwait(false);
    }

    /// <summary>
    /// Stops the event processing subscriptions.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _streamSubscriptionConnection?.Dispose();
        _streamSubscriptionConnection = null;
        return Task.CompletedTask;
    }

    private async Task ConnectToSubscription()
    {
        // Set position to the latest successful processed event position of the snapshot.
        // This is done before a subscription connection is opened,
        // since when a subscription drops (eg. due to subscriber failure)
        // the in-memory value Subscription.CurrentPosition may already be higher, than the last successful processed event.
        // Eg. this can be the case if the subscription drops.
        // (CheckPointReached may still be called, and there is no easy option to detect the drop of the subscription there).
        // (CheckPointReached gets called in the MoveNext of the event enumerator).
        var snapshot = await GetSnapshotPosition().ConfigureAwait(false);
        _subscription.CurrentPosition = snapshot?.Position;
        _subscription.CurrentEventNumber = snapshot?.StreamPosition;
        EventingMeter.SubscriptionSnapshotUpdated(
            _subscription.ScopeName,
            _subscription.CurrentPosition?.CommitPosition ?? Position.Start.CommitPosition,
            _subscription.CurrentEventNumber ?? StreamPosition.Start);

        // currently only the all stream supports checkpoints and server side event filters :(
        if (_subscription.IsAllStream)
        {
            _streamSubscriptionConnection = await CreateAllSubscription().ConfigureAwait(false);
        }
        else
        {
            _streamSubscriptionConnection = await CreateStreamSubscription().ConfigureAwait(false);
        }

        _logger.LogInformation("Subscription {Scope} connected", _subscription.ScopeName);
    }

    private Task<StreamSubscription> CreateStreamSubscription()
    {
        return _subscription.CurrentEventNumber.HasValue
            ? _client.SubscribeToStreamAsync(
                _subscription.StreamName,
                _subscription.CurrentEventNumber.Value,
                (_, data, _) => HandleEventAndCheckpointByEventNumber(data),
                true,
                (_, reason, ex) => HandleSubscriptionDropped(reason, ex))
            : _client.SubscribeToStreamAsync(
                _subscription.StreamName,
                (_, data, _) => HandleEventAndCheckpointByEventNumber(data),
                true,
                (_, reason, ex) => HandleSubscriptionDropped(reason, ex));
    }

    private Task<StreamSubscription> CreateAllSubscription()
    {
        return _subscription.CurrentPosition.HasValue
            ? _client.SubscribeToAllAsync(
                _subscription.CurrentPosition.Value,
                (_, data, _) => HandleEvent(data),
                true,
                (_, reason, ex) => HandleSubscriptionDropped(reason, ex),
                BuildFilterOptions())
            : _client.SubscribeToAllAsync(
                (_, data, _) => HandleEvent(data),
                true,
                (_, reason, ex) => HandleSubscriptionDropped(reason, ex),
                BuildFilterOptions());
    }

    // since subscriptions to a stream which is not $all provide invalid positions (see https://discuss.eventstore.com/t/event-position-18446744073709551615/3172)
    // we need to use the event number for catch up checks
    private async Task HandleEventAndCheckpointByEventNumber(ResolvedEvent data)
    {
        await HandleEvent(data);

        if (!_subscription.IsCatchUp || _subscription.CatchUpEndsEventNumber > _subscription.CurrentEventNumber)
        {
            return;
        }

        await _eventProcessingHandler.HandleCatchUpCompleted(_subscription).ConfigureAwait(false);
        _subscription.IsCatchUp = false;
    }

    private async Task HandleEvent(ResolvedEvent data)
    {
        _subscription.CurrentPosition = data.OriginalEvent.Position;
        _subscription.CurrentEventNumber = data.OriginalEvent.EventNumber;

        var stopwatch = Stopwatch.StartNew();
        if (await _eventProcessingHandler.HandleEvent(_subscription, data).ConfigureAwait(false))
        {
            _retryPolicy.Succeeded();
        }

        EventingMeter.EventProcessed(_subscription.ScopeName, data.OriginalEvent, data.Event, stopwatch.Elapsed);
    }

    /// <summary>
    /// This always gets called after every read event, when no event name filter matches or after <see cref="EventProcessingHandler{TScope}.HandleEvent"/>.
    /// </summary>
    /// <param name="position">Current position which is read.</param>
    /// <returns>A Task.</returns>
    private async Task CheckpointReached(Position position)
    {
        EventingMeter.SubscriptionCheckpointReached(_subscription.ScopeName, position.CommitPosition);

        if (!_subscription.IsCatchUp || _subscription.CatchUpEndsPosition > position)
        {
            _subscription.CurrentPosition = position;
            return;
        }

        await _eventProcessingHandler.HandleCatchUpCompleted(_subscription).ConfigureAwait(false);
        _subscription.IsCatchUp = false;
        _subscription.CurrentPosition = position;
    }

    private async void HandleSubscriptionDropped(SubscriptionDroppedReason reason, Exception? ex)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object> { ["Scope"] = _subscription.ScopeName });

        EventingMeter.SubscriptionFailure(_subscription.ScopeName, reason);
        _logger.LogWarning(ex, "Subscription dropped: {Reason}", reason);

        while (await _retryPolicy.Failed(reason).ConfigureAwait(false))
        {
            _logger.LogInformation("Trying to reconnect the subscription…");
            try
            {
                var subscription = _streamSubscriptionConnection;
                _streamSubscriptionConnection = null;
                subscription?.Dispose();
                await ConnectToSubscription().ConfigureAwait(false);
                return;
            }
            catch (Exception reconnectEx)
            {
                _logger.LogError(reconnectEx, "Failed to reconnect the subscription");
            }
        }

        _logger.LogInformation("Not trying to reconnect the subscription");
    }

    private SubscriptionFilterOptions BuildFilterOptions()
    {
        var descriptorFullNames = _eventTypeResolver.GetDescriptorNames(_subscription.EventTypes).ToList();

        if (descriptorFullNames.Count == 0)
        {
            throw new ArgumentException($"Trying to add a {nameof(EventProcessingSubscriber<TScope>)} without any events to process.");
        }

        return new SubscriptionFilterOptions(
            EventTypeFilter.RegularExpression(string.Join("|", descriptorFullNames.Select(Regex.Escape))),
            1,
            (_, pos, _) => CheckpointReached(pos));
    }

    private async Task<(Position Position, StreamPosition StreamPosition)?> GetSnapshotPosition()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var eventProcessorScope = scope.ServiceProvider.GetRequiredService<TScope>();
        return await eventProcessorScope.GetSnapshotPosition().ConfigureAwait(false);
    }
}
