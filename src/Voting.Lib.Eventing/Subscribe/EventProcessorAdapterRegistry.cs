// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using Voting.Lib.Eventing.Protobuf;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Type registry for <see cref="EventProcessorAdapter{TScope,TEvent}"/>.
/// </summary>
public abstract class EventProcessorAdapterRegistry
{
    private readonly ConcurrentDictionary<string, Type?> _processorTypes = new();
    private readonly IProtobufTypeRegistry _registry;
    private readonly Type _scopeType;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessorAdapterRegistry"/> class.
    /// </summary>
    /// <param name="registry">The protobuf type registry.</param>
    /// <param name="scopeType">The type of event processor scope.</param>
    protected EventProcessorAdapterRegistry(IProtobufTypeRegistry registry, Type scopeType)
    {
        _registry = registry;
        _scopeType = scopeType;
    }

    internal IEventProcessorAdapter? GetProcessorAdapter(
        IServiceProvider sp,
        string eventTypeName)
    {
        var processorType = _processorTypes.GetOrAdd(eventTypeName, CreateProcessorType);
        return processorType == null
            ? null
            : (IEventProcessorAdapter?)sp.GetService(processorType);
    }

    private Type? CreateProcessorType(string eventTypeName)
    {
        var descriptor = _registry.Find(eventTypeName);

        return descriptor == null
            ? null
            : typeof(EventProcessorAdapter<,>).MakeGenericType(_scopeType, descriptor.ClrType);
    }
}

/// <summary>
/// Type registry for <see cref="EventProcessorAdapter{TScope,TEvent}"/>.
/// </summary>
/// <typeparam name="TScope">The type of event processor scope.</typeparam>
public class EventProcessorAdapterRegistry<TScope> : EventProcessorAdapterRegistry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessorAdapterRegistry{TScope}"/> class.
    /// </summary>
    /// <param name="registry">The protobuf type registry.</param>
    public EventProcessorAdapterRegistry(IProtobufTypeRegistry registry)
        : base(registry, typeof(TScope))
    {
    }
}
