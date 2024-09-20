// (c) Copyright by Abraxas Informatik AG
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
    /// Gets the list votes counting circle e-voting protocol export template.
    /// </summary>
    public static readonly TemplateModel ListVotesCountingCircleEVotingProtocol = new TemplateModel
    {
        Key = "proportional_election_lists_counting_circle_e_voting_protocol",
        Filename = "Proporz_Formular1_Wahlzettelrapport_EVoting_{0}_{1}",
        Description = "1 - Wahlzettelrapport E-Voting",
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
    /// Gets the list candidate empty votes counting circle e-voting protocol export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateEmptyVotesCountingCircleEVotingProtocol = new TemplateModel
    {
        Key = "proportional_election_list_candidate_empty_votes_counting_circle_e_voting_protocol",
        Filename = "Proporz_Formular2_KandParteiStimmen_EVoting_{0}_{1}",
        Description = "2 - Kandidierenden- und Parteistimmen E-Voting",
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
    /// Gets the list votes end result inside of a proportional election union export template.
    /// </summary>
    public static readonly TemplateModel ListVotesPoliticalBusinessUnionEndResults = new TemplateModel
    {
        Key = "proportional_election_list_votes_political_business_union_end_results",
        Filename = "Proporz_FormularC_Listenergebnisse_{0}_{1}",
        Description = "C - Listenergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
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
    /// Gets the list candidate end results e-voting export template.
    /// </summary>
    public static readonly TemplateModel ListCandidateEndResultsEVoting = new TemplateModel
    {
        Key = "proportional_election_list_candidate_end_results_e_voting",
        Filename = "Proporz_Formular5b_KandParteiErgebnisse_EVoting_{0}_{1}",
        Description = "5b - Kandidierenden- und Parteiergebnisse E-Voting",
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
    /// Gets the end result list unions e-voting export template.
    /// </summary>
    public static readonly TemplateModel EndResultListUnionsEVoting = new TemplateModel
    {
        Key = "proportional_election_end_result_list_unions_e_voting",
        Filename = "Proporz_Formular5_Wahlprotokoll_EVoting_{0}_{1}",
        Description = "5 - Wahlprotokoll E-Voting",
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

    /// <summary>
    /// Gets the end result double proportional export template.
    /// </summary>
    public static readonly TemplateModel EndResultDoubleProportional = new TemplateModel
    {
        Key = "proportional_election_end_result_double_proportional",
        Filename = "DPT3_DopP_Sitzverteilung",
        Description = "DPT3 - Ergebnis der DopP-Sitzverteilung, 1-Wahlkreis",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the union end result quorum union list double proportional pukelsheim export template.
    /// </summary>
    public static readonly TemplateModel UnionEndResultQuorumUnionListDoubleProportional = new TemplateModel
    {
        Key = "proportional_election_union_end_result_quorum_union_list_double_proportional",
        Filename = "Stimmenquorum der Listengruppen",
        Description = "Stimmenquorum der Listengruppen",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the union end result super apportionment double proportional export template.
    /// </summary>
    public static readonly TemplateModel UnionEndResultSuperApportionmentDoubleProportional = new TemplateModel
    {
        Key = "proportional_election_union_end_result_super_apportionment_double_proportional",
        Filename = "Oberzuteilung der Sitze an die Listengruppen",
        Description = "Oberzuteilung der Sitze an die Listengruppen",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the union end result sub apportionment double proportional export template.
    /// </summary>
    public static readonly TemplateModel UnionEndResultSubApportionmentDoubleProportional = new TemplateModel
    {
        Key = "proportional_election_union_end_result_sub_apportionment_double_proportional",
        Filename = "Unterzuteilung der Sitze an die Listen",
        Description = "Unterzuteilung der Sitze an die Listen",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the union end result number of mandates double proportional export template.
    /// </summary>
    public static readonly TemplateModel UnionEndResultNumberOfMandatesDoubleProportional = new TemplateModel
    {
        Key = "proportional_election_union_end_result_number_of_mandates_double_proportional",
        Filename = "Anzahl Sitze pro Liste und Wahlkreis",
        Description = "Anzahl Sitze pro Liste und Wahlkreis",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the union end result calculation double proportional export template.
    /// </summary>
    public static readonly TemplateModel UnionEndResultCalculationDoubleProportional = new TemplateModel
    {
        Key = "proportional_election_union_end_result_calculation_double_proportional",
        Filename = "Gesamtergebnis der Sitzeverteilung",
        Description = "Gesamtergebnis der Sitzeverteilung",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessUnionResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        ListsCountingCircleProtocol,
        ListVotesCountingCircleProtocol,
        ListVotesCountingCircleEVotingProtocol,
        ListCandidateVotesCountingCircleProtocol,
        ListCandidateEmptyVotesCountingCircleProtocol,
        ListCandidateEmptyVotesCountingCircleEVotingProtocol,
        ListCandidateVoteSourcesCountingCircleProtocol,
        ListVotesEndResults,
        ListVotesPoliticalBusinessUnionEndResults,
        ListCandidateVotesEndResults,
        ListCandidateEndResults,
        ListCandidateEndResultsEVoting,
        ListCandidateVoteSourcesEndResults,
        EndResultCalculation,
        EndResultListUnions,
        EndResultListUnionsEVoting,
        ResultBundleReview,
        EndResultDoubleProportional,
        UnionEndResultQuorumUnionListDoubleProportional,
        UnionEndResultSuperApportionmentDoubleProportional,
        UnionEndResultSubApportionmentDoubleProportional,
        UnionEndResultNumberOfMandatesDoubleProportional,
        UnionEndResultCalculationDoubleProportional,
    };
}
