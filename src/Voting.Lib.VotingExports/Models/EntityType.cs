﻿// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.VotingExports.Models;

/// <summary>
/// The entity type of an export.
/// </summary>
public enum EntityType
{
    /// <summary>
    /// Unspecified.
    /// </summary>
    Unspecified,

    /// <summary>
    /// Contest.
    /// </summary>
    Contest,

    /// <summary>
    /// Vote.
    /// </summary>
    Vote,

    /// <summary>
    /// Majority Election.
    /// </summary>
    MajorityElection,

    /// <summary>
    /// Secondary Majority Election.
    /// </summary>
    SecondaryMajorityElection,

    /// <summary>
    /// Proportional Election.
    /// </summary>
    ProportionalElection,
}
