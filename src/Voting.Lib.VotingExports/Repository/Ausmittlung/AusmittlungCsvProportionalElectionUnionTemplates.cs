// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung CSV templates for proportional election union exports.
/// </summary>
public static class AusmittlungCsvProportionalElectionUnionTemplates
{
    /// <summary>
    /// A list of mandates won by each party for an entire political business union.
    /// </summary>
    public static readonly TemplateModel PartyMandates = new TemplateModel
    {
        Key = "proportional_election_union_party_mandates",
        Filename = "Sitzverteilung",
        Description = "Sitzverteilung",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Voter participation inside of a proportional election union.
    /// </summary>
    public static readonly TemplateModel VoterParticipation = new TemplateModel
    {
        Key = "proportional_election_union_voter_participation",
        Filename = "Wahlbeteiligung",
        Description = "Wahlbeteiligung",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Strength (votes) of each parties in a proportional election union.
    /// </summary>
    public static readonly TemplateModel PartyVotes = new TemplateModel
    {
        Key = "proportional_election_union_party_votes",
        Filename = "Parteistimmen",
        Description = "Parteistimmen",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        PartyMandates,
        VoterParticipation,
        PartyVotes,
    };
}
