// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung PDF templates for proportional election exports.
/// </summary>
public static class AusmittlungPdfProportionalElectionTemplates
{
    /// <summary>
    /// Gets the voter turnout protocol export template.
    /// </summary>
    public static readonly TemplateModel VoterTurnoutProtocol = new TemplateModel
    {
        Key = "proportional_election_voter_turnout_protocol",
        Filename = "Wahlbeteiligung",
        Description = "Wahlbeteiligung über die gesamte Geschäftsebene",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list votes counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel ListVotesCountingCircleProtocol = new TemplateModel
    {
        Key = "proportional_election_lists_counting_circle_protocol",
        Filename = "Wahlzettelreport - {0}",
        Description = "Wahlzettelreport",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the lists counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel ListsCountingCircleProtocol = new TemplateModel
    {
        Key = "proportional_election_list_votes_counting_circle_protocol",
        Filename = "Gemeindeprotokoll - {0}",
        Description = "Gemeindeprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list candidate votes counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateVotesCountingCircleProtocol = new TemplateModel
    {
        Key = "proportional_election_list_candidate_votes_counting_circle_protocol",
        Filename = "Kandidatenstimmen - {0}",
        Description = "Kandidatenstimmen (Aushang)",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list candidate empty votes counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateEmptyVotesCountingCircleProtocol = new TemplateModel
    {
        Key = "proportional_election_list_candidate_empty_votes_counting_circle_protocol",
        Filename = "Kandidaten- Zusatz- und Parteistimmen - {0}",
        Description = "Kandidaten-, Zusatz- und Parteistimmen",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list candidate vote sources counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateVoteSourcesCountingCircleProtocol = new TemplateModel
    {
        Key = "proportional_election_list_candidate_vote_sources_counting_circle_protocol",
        Filename = "Stimmenherkunft inkl. Kumulierungen und Panaschierungen - {0}",
        Description = "Stimmenherkunft inkl. Kumulierungen und Panaschierungen",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list votes end result export template.
    /// </summary>
    public static readonly TemplateModel ListVotesEndResults = new TemplateModel
    {
        Key = "proportional_election_list_votes_end_results",
        Filename = "Listenergbenisse - {0}",
        Description = "Listenergbenisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list candidate votes end results export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateVotesEndResults = new TemplateModel
    {
        Key = "proportional_election_list_candidate_votes_end_results",
        Filename = "Kandidatenstimmen - {0}",
        Description = "Kandidatenstimmen",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list candidate end results export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateEndResults = new TemplateModel
    {
        Key = "proportional_election_list_candidate_end_results",
        Filename = "Kandidatenergebnisse - {0}",
        Description = "Kandidatenergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the list candidate vote sources end results export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateVoteSourcesEndResults = new TemplateModel
    {
        Key = "proportional_election_list_candidate_vote_sources_end_results",
        Filename = "Stimmenherkunft inkl. Kumulierungen und Panaschierungen - {0}",
        Description = "Stimmenherkunft inkl. Kumulierungen und Panaschierungen",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result calculation export template.
    /// </summary>
    public static readonly TemplateModel EndResultCalculation = new TemplateModel
    {
        Key = "proportional_election_end_result_calculation",
        Filename = "Verteilung der Sitze - {0}",
        Description = "Verteilung der Sitze",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result list unions export template.
    /// </summary>
    public static readonly TemplateModel EndResultListUnions = new TemplateModel
    {
        Key = "proportional_election_end_result_list_unions",
        Filename = "Wahlprotokoll - {0}",
        Description = "Wahlprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        VoterTurnoutProtocol,
        ListsCountingCircleProtocol,
        ListVotesCountingCircleProtocol,
        ListCandidateVotesCountingCircleProtocol,
        ListCandidateEmptyVotesCountingCircleProtocol,
        ListCandidateVoteSourcesCountingCircleProtocol,
        ListVotesEndResults,
        ListCandidateVotesEndResults,
        ListCandidateEndResults,
        ListCandidateVoteSourcesEndResults,
        EndResultCalculation,
        EndResultListUnions,
    };
}
