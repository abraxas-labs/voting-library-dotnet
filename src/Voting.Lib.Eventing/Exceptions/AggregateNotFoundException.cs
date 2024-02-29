// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

namespace Voting.Lib.Eventing.Exceptions;

/// <summary>
/// Thrown when an aggregate is not found.
/// </summary>
[Serializable]
public class AggregateNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateNotFoundException"/> class.
    /// </summary>
    /// <param name="id">The aggregate ID.</param>
    public AggregateNotFoundException(Guid id)
        : base($"Aggregate {id} not found")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateNotFoundException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected AggregateNotFoundException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }
}
