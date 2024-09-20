// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

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
}
