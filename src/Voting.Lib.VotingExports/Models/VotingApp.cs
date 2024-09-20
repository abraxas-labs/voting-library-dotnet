// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.VotingExports.Models;

/// <summary>
/// Voting Apps, which can generate exports.
/// </summary>
public enum VotingApp
{
    /// <summary>
    /// Unspecified.
    /// </summary>
    Unspecified,

    /// <summary>
    /// The voting basis application.
    /// </summary>
    VotingBasis,

    /// <summary>
    /// The voting ausmittlung application.
    /// </summary>
    VotingAusmittlung,
}
