// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;
using EventStore.Client;

namespace Voting.Lib.Eventing.Exceptions;

/// <summary>
/// Thrown when the expected stream version does not match the actual stream version.
/// </summary>
[Serializable]
public class VersionMismatchException : Exception
{
    internal VersionMismatchException(Exception innerEx, string streamName, StreamRevision? expectedStreamRevision, StreamRevision actualStreamRevision)
        : base(GenerateMessage(streamName, expectedStreamRevision, actualStreamRevision), innerEx)
    {
        ExpectedStreamRevision = expectedStreamRevision;
        ActualStreamRevision = actualStreamRevision;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionMismatchException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected VersionMismatchException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }

    /// <summary>
    /// Gets the expected stream version.
    /// </summary>
    public StreamRevision? ExpectedStreamRevision { get; }

    /// <summary>
    /// Gets the actual stream version.
    /// </summary>
    public StreamRevision ActualStreamRevision { get; }

    private static string GenerateMessage(string streamName, StreamRevision? expectedRevision, StreamRevision? actualRevision)
        => $"Expected revision {expectedRevision} of stream {streamName}, but the actual revision is {actualRevision}";
}
