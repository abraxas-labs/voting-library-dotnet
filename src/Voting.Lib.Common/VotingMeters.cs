// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Reflection;

namespace Voting.Lib.Common;

/// <summary>
/// Common voting diagnostic meter names.
/// </summary>
public static class VotingMeters
{
    /// <summary>
    /// The prefix used by all meters provided by Voting.Lib.
    /// </summary>
    public const string NamePrefix = "Voting.Lib.";

    /// <summary>
    /// A wildcard name, which matches all voting meter names.
    /// </summary>
    /// <remarks>
    /// Can be used in opentelemetry AddMeter calls.
    /// </remarks>
    public const string All = NamePrefix + "*";

    /// <summary>
    /// The prefix of all instrument names provided by Voting.Lib.
    /// </summary>
    public const string InstrumentNamePrefix = "voting_";

    /// <summary>
    /// Gets the version of the meters.
    /// </summary>
    public static readonly string Version = GetVersion();

    private static string GetVersion()
    {
        return typeof(VotingMeters)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion
            ?? throw new InvalidOperationException("Could not find the assembly version.");
    }
}
