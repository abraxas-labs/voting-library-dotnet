// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using EventStore.Client;
using Voting.Lib.Common;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.Diagnostics;

/// <summary>
/// Diagnostics for eventing components.
/// </summary>
public static class EventingMeter
{
    // documentation warning
#pragma warning disable CS1591
    public const string Name = VotingMeters.NamePrefix + "Eventing";

    public const string PublishedEventsCounterName = VotingMeters.InstrumentNamePrefix + "events_published";
    public const string SubscriptionProcessedEventsCounterName = VotingMeters.InstrumentNamePrefix + "subscription_events_processed";
    public const string SubscriptionEventTypeProcessedDurationSecondsName = VotingMeters.InstrumentNamePrefix + "subscription_events_processed_by_type_duration_seconds";
    public const string SubscriptionSnapshotPositionsGaugeName = VotingMeters.InstrumentNamePrefix + "subscription_snapshot_position";
    public const string SubscriptionSnapshotNumbersGaugeName = VotingMeters.InstrumentNamePrefix + "subscription_snapshot_number";
    public const string SubscriptionCheckpointPositionsGaugeName = VotingMeters.InstrumentNamePrefix + "subscription_checkpoint_position";
    public const string SubscriptionCatchUpEndPositionsGaugeName = VotingMeters.InstrumentNamePrefix + "subscription_catch_up_ends_position";
    public const string SubscriptionCatchUpEndNumbersGaugeName = VotingMeters.InstrumentNamePrefix + "subscription_catch_up_ends_number";
    public const string SubscriptionFailuresCounterName = VotingMeters.InstrumentNamePrefix + "subscription_failures";
    public const string SubscriptionLatestEventCommitPositionGaugeName = VotingMeters.InstrumentNamePrefix + "latest_event_commit_position";
    public const string SubscriptionLatestEventPreparePositionGaugeName = VotingMeters.InstrumentNamePrefix + "latest_event_prepare_position";
    public const string SubscriptionLatestEventNumberGaugeName = VotingMeters.InstrumentNamePrefix + "latest_event_number";

    public const string EventProcessorScopeLabelName = "eventProcessorScope";
    public const string EventTypeLabelName = "eventType";
    public const string SubscriptionFailureReasonLabelName = "failure";
#pragma warning restore CS1591

    private static readonly Meter Meter = new(Name, VotingMeters.Version);

    private static readonly ConcurrentDictionary<string, long> SubscriptionSnapshotPositionsByProcessorScope = new();
    private static readonly ConcurrentDictionary<string, long> SubscriptionSnapshotNumbersByProcessorScope = new();
    private static readonly ConcurrentDictionary<string, long> SubscriptionCheckpointPositionsByProcessorScope = new();
    private static readonly ConcurrentDictionary<string, long> SubscriptionCatchUpEndPositionsByProcessorScope = new();
    private static readonly ConcurrentDictionary<string, long> SubscriptionCatchUpEndNumbersByProcessorScope = new();
    private static readonly ConcurrentDictionary<string, long> LatestEventNumbersByProcessorScope = new();
    private static readonly ConcurrentDictionary<string, long> LatestEventCommitPositionsByProcessorScope = new();
    private static readonly ConcurrentDictionary<string, long> LatestEventPreparePositionsByProcessorScope = new();

    private static readonly Counter<long> PublishedEventsCounter = Meter.CreateCounter<long>(
        PublishedEventsCounterName,
        "Event",
        "Number of published events");

    private static readonly Counter<long> SubscriptionProcessedEventsCounter = Meter.CreateCounter<long>(
        SubscriptionProcessedEventsCounterName,
        "Event",
        "Number of processed events by subscription");

    private static readonly Histogram<decimal> SubscriptionEventTypeProcessedDurationSeconds = Meter.CreateHistogram<decimal>(
        SubscriptionEventTypeProcessedDurationSecondsName,
        "s",
        "Seconds spent processing a specific event type");

    private static readonly Counter<long> SubscriptionFailuresCounter = Meter.CreateCounter<long>(
        SubscriptionFailuresCounterName,
        description: "Failures of a subscription (dropped, server error or event processor error)");

    static EventingMeter()
    {
        Meter.CreateObservableGauge(
            SubscriptionSnapshotPositionsGaugeName,
            () => BuildProcessorScopeMeasurement(SubscriptionSnapshotPositionsByProcessorScope),
            description: "The latest successfully processed event position of this subscription, excludes server side filtered messages without a processor");

        Meter.CreateObservableGauge(
            SubscriptionSnapshotNumbersGaugeName,
            () => BuildProcessorScopeMeasurement(SubscriptionSnapshotNumbersByProcessorScope),
            description: "The latest successfully processed event number of this subscription, excludes server side filtered messages without a processor");

        Meter.CreateObservableGauge(
            SubscriptionCheckpointPositionsGaugeName,
            () => BuildProcessorScopeMeasurement(SubscriptionCheckpointPositionsByProcessorScope),
            description: "The latest checkpoint position processed by the subscription, includes filtered messages without a processor");

        Meter.CreateObservableGauge(
            SubscriptionCatchUpEndPositionsGaugeName,
            () => BuildProcessorScopeMeasurement(SubscriptionCatchUpEndPositionsByProcessorScope),
            description: "The position the event stream had, when the subscription connected");

        Meter.CreateObservableGauge(
            SubscriptionCatchUpEndNumbersGaugeName,
            () => BuildProcessorScopeMeasurement(SubscriptionCatchUpEndNumbersByProcessorScope),
            description: "The event number the event stream had, when the subscription connected");

        Meter.CreateObservableGauge(
            SubscriptionLatestEventNumberGaugeName,
            () => BuildProcessorScopeMeasurement(LatestEventNumbersByProcessorScope),
            description: "The event number of the latest event in a stream");

        Meter.CreateObservableGauge(
            SubscriptionLatestEventCommitPositionGaugeName,
            () => BuildProcessorScopeMeasurement(LatestEventCommitPositionsByProcessorScope),
            description: "The event commit position of the latest event in a stream");

        Meter.CreateObservableGauge(
            SubscriptionLatestEventPreparePositionGaugeName,
            () => BuildProcessorScopeMeasurement(LatestEventPreparePositionsByProcessorScope),
            description: "The event prepare position of the latest event in a stream");

        InitHistograms();
    }

    internal static void EventProcessed(string processorScopeName, EventRecord originalEventRecord, EventRecord eventRecord, TimeSpan duration)
    {
        SubscriptionProcessedEventsCounter.Add(1, CreateProcessorScopeTag(processorScopeName));
        SubscriptionEventTypeProcessedDurationSeconds.Record((decimal)duration.TotalSeconds, CreateProcessorScopeTag(processorScopeName), new(EventTypeLabelName, eventRecord.EventType));
        SubscriptionSnapshotUpdated(processorScopeName, originalEventRecord.Position.CommitPosition, originalEventRecord.EventNumber);
    }

    internal static void SubscriptionSnapshotUpdated(string processorScopeName, ulong position, ulong eventNumber)
    {
        SubscriptionSnapshotPositionsByProcessorScope[processorScopeName] = EventStoreSaveToLong(position);
        SubscriptionSnapshotNumbersByProcessorScope[processorScopeName] = EventStoreSaveToLong(eventNumber);
    }

    internal static void SubscriptionCatchUpEnds(string processorScopeName, ulong position, ulong eventNumber)
    {
        SubscriptionCatchUpEndPositionsByProcessorScope[processorScopeName] = EventStoreSaveToLong(position);
        SubscriptionCatchUpEndNumbersByProcessorScope[processorScopeName] = EventStoreSaveToLong(eventNumber);
    }

    internal static void SubscriptionCheckpointReached(string processorScopeName, ulong position)
        => SubscriptionCheckpointPositionsByProcessorScope[processorScopeName] = EventStoreSaveToLong(position);

    internal static void SubscriptionFailure(string processorScopeName, SubscriptionDroppedReason reason)
        => SubscriptionFailuresCounter.Add(
            1,
            CreateProcessorScopeTag(processorScopeName),
            new(SubscriptionFailureReasonLabelName, reason.ToString()));

    internal static void EventsPublished(int count)
        => PublishedEventsCounter.Add(count);

    internal static void LatestEvent(IReadOnlyCollection<string> scopeNames, EventRecord? eventRecord)
    {
        var position = eventRecord?.Position ?? Position.Start;
        foreach (var scopeName in scopeNames)
        {
            LatestEventCommitPositionsByProcessorScope[scopeName] = EventStoreSaveToLong(position.CommitPosition);
            LatestEventPreparePositionsByProcessorScope[scopeName] = EventStoreSaveToLong(position.PreparePosition);
            LatestEventNumbersByProcessorScope[scopeName] = (eventRecord?.EventNumber ?? StreamPosition.Start).ToInt64();
        }
    }

    private static IEnumerable<Measurement<T>> BuildProcessorScopeMeasurement<T>(IEnumerable<KeyValuePair<string, T>> items)
        where T : struct
    {
        return items.Select(x => new Measurement<T>(x.Value, CreateProcessorScopeTag(x.Key)));
    }

    private static KeyValuePair<string, object?> CreateProcessorScopeTag(string processorScopeName)
        => KeyValuePair.Create<string, object?>(EventProcessorScopeLabelName, processorScopeName);

    private static long EventStoreSaveToLong(ulong value)
    {
        // dotnet diagnostics apis cannot handle ulong
        // however there is a bug in eventstore which results in delivering ulong.MaxValue instead of an actual position
        // (see https://discuss.eventstore.com/t/event-position-18446744073709551615/3172/3)
        // since this case is not relevant for our monitoring, we just use long.MaxValue
        return value == ulong.MaxValue
            ? long.MaxValue
            : (long)value;
    }

    private static void InitHistograms()
    {
        SubscriptionEventTypeProcessedDurationSeconds.Record(
            0,
            CreateProcessorScopeTag(typeof(TransientEventProcessorScope).FullName ?? nameof(TransientEventProcessorScope)),
            new(EventTypeLabelName, "Initialization"));
    }
}
