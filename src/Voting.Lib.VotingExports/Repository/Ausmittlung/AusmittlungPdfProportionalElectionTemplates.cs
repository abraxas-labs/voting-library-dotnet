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
        Filename = "Proporz_Formular1_Wahlzettelrapport_{0}_{1}",
        Description = "1 - Wahlzettelrapport",
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
        Filename = "Proporz_Formular4_Gemeindeprotokoll_{0}_{1}",
        Description = "4 - Gemeindeprotokoll",
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
        Filename = "Proporz_Formular2a_KandStimmen_{0}_{1}",
        Description = "2a - Kandidierendenstimmen",
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
        Filename = "Proporz_Formular2_KandParteiStimmen_{0}_{1}",
        Description = "2 - Kandidierenden- und Parteistimmen",
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
        Filename = "Proporz_Formular3b_Stimmenherkunft_KumPan_{0}_{1}",
        Description = "3b - Stimmenherkunft inkl. KumPan",
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
        Filename = "Proporz_FormularC_Listenergebnisse_{0}_{1}",
        Description = "C - Listenergebnisse",
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
        Filename = "Proporz_FormularB_KandStimmen_{0}_{1}",
        Description = "B - Kandidierendenstimmen",
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
        Filename = "Proporz_Formular5b_KandParteiErgebnisse_{0}_{1}",
        Description = "5b - Kandidierenden- und Parteiergebnisse",
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
        Filename = "Proporz_FormularD_Stimmenherkunft_KumPan_{0}_{1}",
        Description = "D - Stimmenherkunft inkl. KumPan",
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
        Filename = "Proporz_Formular5a_Sitzverteilung_{0}_{1}",
        Description = "5a - Sitzverteilung",
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
        Filename = "Proporz_Formular5_Wahlprotokoll_{0}_{1}",
        Description = "5 - Wahlprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the result bundle review export template.
    /// </summary>
    public static readonly TemplateModel ResultBundleReview = new TemplateModel
    {
        Key = "proportional_election_result_bundle_review",
        Filename = "Bundkontrolle {0}",
        Description = "Bundkontrolle",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResultBundleReview,
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
        ResultBundleReview,
    };
}
