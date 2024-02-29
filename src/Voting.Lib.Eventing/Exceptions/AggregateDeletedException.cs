// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

namespace Voting.Lib.Eventing.Exceptions;

/// <summary>
/// Thrown when an operation was performed on a deleted aggregate.
/// </summary>
[Serializable]
public class AggregateDeletedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateDeletedException"/> class.
    /// </summary>
    /// <param name="id">The aggregate ID.</param>
    public AggregateDeletedException(Guid id)
        : base($"Aggregate {id} is already deleted.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateDeletedException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected AggregateDeletedException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }
}
