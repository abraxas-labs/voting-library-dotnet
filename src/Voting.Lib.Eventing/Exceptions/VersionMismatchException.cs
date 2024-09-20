// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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
