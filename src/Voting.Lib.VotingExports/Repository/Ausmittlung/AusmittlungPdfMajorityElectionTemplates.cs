// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung PDF templates for majority election exports.
/// </summary>
public static class AusmittlungPdfMajorityElectionTemplates
{
    /// <summary>
    /// Gets the counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel CountingCircleProtocol = new TemplateModel
    {
        Key = "majority_election_counting_circle_protocol",
        Filename = "Majorz_Gemeindeprotokoll_{0}_{1}",
        Description = "Gemeindeprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the counting circle e-voting protocol export template.
    /// </summary>
    public static readonly TemplateModel CountingCircleEVotingProtocol = new TemplateModel
    {
        Key = "majority_election_counting_circle_e_voting_protocol",
        Filename = "Majorz_Gemeindeprotokoll_EVoting_{0}_{1}",
        Description = "Gemeindeprotokoll E-Voting",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        DomainOfInfluenceTypes = new HashSet<DomainOfInfluenceType>
        {
            DomainOfInfluenceType.Ch,
            DomainOfInfluenceType.Ct,
            DomainOfInfluenceType.Bz,
        },
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultProtocol = new TemplateModel
    {
        Key = "majority_election_end_result_protocol",
        Filename = "Majorz_Wahlprotokoll_{0}_{1}",
        Description = "Wahlprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result e-voting protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultEVotingProtocol = new TemplateModel
    {
        Key = "majority_election_end_result_e_voting_protocol",
        Filename = "Majorz_Wahlprotokoll_EVoting_{0}_{1}",
        Description = "Wahlprotokoll E-Voting",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result detail protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultDetailProtocol = new TemplateModel
    {
        Key = "majority_election_end_result_detail_protocol",
        Filename = "Majorz_Detailergebnisse_{0}_{1}",
        Description = "Detailergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result detail protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultDetailWithoutEmptyAndInvalidVotesProtocol = new TemplateModel
    {
        Key = "majority_election_end_result_detail_without_empty_and_invalid_votes_protocol",
        Filename = "Majorz_Detailergebnisse_exkl_{0}_{1}",
        Description = "Detailergebnisse_exkl",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the result bundle review export template.
    /// </summary>
    public static readonly TemplateModel ResultBundleReview = new TemplateModel
    {
        Key = "majority_election_result_bundle_review",
        Filename = "Bundkontrolle {0}",
        Description = "Bundkontrolle",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResultBundleReview,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        CountingCircleProtocol,
        CountingCircleEVotingProtocol,
        EndResultProtocol,
        EndResultEVotingProtocol,
        EndResultDetailProtocol,
        EndResultDetailWithoutEmptyAndInvalidVotesProtocol,
        ResultBundleReview,
    };
}
