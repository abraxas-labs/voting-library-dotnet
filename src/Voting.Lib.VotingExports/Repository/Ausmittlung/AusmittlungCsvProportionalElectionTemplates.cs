// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung CSV templates for proportional election exports.
/// </summary>
public static class AusmittlungCsvProportionalElectionTemplates
{
    /// <summary>
    /// Gets the candidate counting circle results with vote sources export template.
    /// </summary>
    public static readonly TemplateModel CandidateCountingCircleResultsWithVoteSources = new TemplateModel
    {
        Key = "proportional_election_candidate_results_with_vote_sources",
        Filename = "Kandidatenstimmen Gesamttotal je Liste - unveränderte und veränderte Wahlzettel {0}",
        Description = "Kandidatenstimmen, Gesamttotal je Liste - unveränderte / veränderte Wahlzettel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the alphabetical candidates export template.
    /// </summary>
    public static readonly TemplateModel CandidatesAlphabetical = new TemplateModel
    {
        Key = "proportional_election_candidates_alphabetical",
        Filename = "Kandidatinnen und Kandidaten",
        Description = "Kandidatinnen und Kandidaten",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the numerical candidates export template.
    /// </summary>
    public static readonly TemplateModel CandidatesNumerical = new TemplateModel
    {
        Key = "proportional_election_candidates_numerical",
        Filename = "Numerisches Kandidierendenverzeichnis",
        Description = "Numerisches Kandidierendenverzeichnis",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        CandidateCountingCircleResultsWithVoteSources,
        CandidatesAlphabetical,
        CandidatesNumerical,
    };
}
