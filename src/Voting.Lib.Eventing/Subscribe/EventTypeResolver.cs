// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Voting.Lib.Eventing.Protobuf;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Resolves event types to their Protobuf definition.
/// </summary>
public class EventTypeResolver
{
    private readonly IProtobufTypeRegistry _typeRegistry;
    private readonly ILogger<EventTypeResolver> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventTypeResolver"/> class.
    /// </summary>
    /// <param name="typeRegistry">The protobuf registry.</param>
    /// <param name="logger">The logger.</param>
    public EventTypeResolver(IProtobufTypeRegistry typeRegistry, ILogger<EventTypeResolver> logger)
    {
        _typeRegistry = typeRegistry;
        _logger = logger;
    }

    /// <summary>
    /// Find the Protobuf descriptor names for the specified types.
    /// </summary>
    /// <param name="eventTypes">The event type to find the descriptor names for.</param>
    /// <returns>Returns the found descriptor names. If a descriptor name is not found, it won't be present in the result.</returns>
    public IReadOnlyCollection<string> GetDescriptorNames(IEnumerable<Type> eventTypes)
    {
        return eventTypes
            .Select(eventType =>
            {
                if (_typeRegistry.Find(eventType)?.FullName is { } fullName)
                {
                    return fullName;
                }

                _logger.LogWarning("Could not find registered proto descriptor for event of type {EventType}", eventType);
                return null;
            })
            .WhereNotNull()
            .ToList();
    }
}
