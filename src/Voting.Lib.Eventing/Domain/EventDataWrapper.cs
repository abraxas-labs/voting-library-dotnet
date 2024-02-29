// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using EventStore.Client;
using Google.Protobuf;
using HashCode = System.HashCode;

namespace Voting.Lib.Eventing.Domain;

internal class EventDataWrapper : IDomainEvent, IEquatable<EventDataWrapper>
{
    internal EventDataWrapper(
        Guid id,
        Guid aggregateId,
        IMessage data,
        StreamRevision aggregateVersion,
        IMessage? metadata = null,
        Position? position = null,
        DateTime? created = null)
    {
        Id = id;
        AggregateId = aggregateId;
        Data = data;
        AggregateVersion = aggregateVersion;
        Position = position;
        Metadata = metadata;
        Created = created;
    }

    public Guid Id { get; }

    public Guid AggregateId { get; }

    public StreamRevision AggregateVersion { get; }

    public Position? Position { get; }

    public IMessage Data { get; }

    public IMessage? Metadata { get; }

    public DateTime? Created { get; }

    public override bool Equals(object? obj) => Equals(obj as EventDataWrapper);

    public bool Equals(EventDataWrapper? other)
    {
        return other != null && AggregateId == other.AggregateId && AggregateVersion == other.AggregateVersion;
    }

    public override int GetHashCode()
        => HashCode.Combine(AggregateId, AggregateVersion);
}
