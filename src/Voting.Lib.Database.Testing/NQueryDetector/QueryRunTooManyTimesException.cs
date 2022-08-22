// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

namespace Voting.Lib.Database.Testing.NQueryDetector;

/// <summary>
/// Thrown if a query has run too many times.
/// </summary>
[Serializable]
public class QueryRunTooManyTimesException : Exception
{
    internal QueryRunTooManyTimesException(string query, int maxCount, int actualCount)
        : base($"The query\n\n'{query}'\n\nrun {actualCount} times while only {maxCount} runs are allowed")
    {
        Query = query;
        MaxCount = maxCount;
        ActualCount = actualCount;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryRunTooManyTimesException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected QueryRunTooManyTimesException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }

    /// <summary>
    /// Gets the maximum count that the query was allowed to run.
    /// </summary>
    public int MaxCount { get; }

    /// <summary>
    /// Gets the amount of times that the query has run.
    /// </summary>
    public int ActualCount { get; }

    /// <summary>
    /// Gets the query text.
    /// </summary>
    public string Query { get; } = string.Empty;
}
