// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung PDF templates for vote exports.
/// </summary>
public static class AusmittlungPdfVoteTemplates
{
    /// <summary>
    /// Gets the end result protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultProtocol = new TemplateModel
    {
        Key = "vote_overall_result",
        Filename = "Abst_{0}_Gesamtergebnisse_{1}",
        Description = "Gesamtergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
        PerDomainOfInfluenceType = true,
    };

    /// <summary>
    /// Gets the temporary end result protocol domain of influences export template.
    /// </summary>
    public static readonly TemplateModel TemporaryEndResultDomainOfInfluencesProtocol = new TemplateModel
    {
        Key = "vote_domain_of_influence_overall_temporary_end_result",
        Filename = "Abst_{0}_provisorischeErgebnisse_{1}_{2}",
        Description = "Provisorische Ergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        DomainOfInfluenceTypes = new HashSet<DomainOfInfluenceType>
        {
            DomainOfInfluenceType.Ch,
            DomainOfInfluenceType.Ct,
            DomainOfInfluenceType.Bz,
        },
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result domain of influences protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultDomainOfInfluencesProtocol = new TemplateModel
    {
        Key = "vote_domain_of_influence_overall_end_result",
        Filename = "Abst_{0}_Detailergebnisse_{1}_{2}",
        Description = "Detailergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        DomainOfInfluenceTypes = new HashSet<DomainOfInfluenceType>
        {
            DomainOfInfluenceType.Ch,
            DomainOfInfluenceType.Ct,
            DomainOfInfluenceType.Bz,
        },
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the result protocol export template.
    /// </summary>
    public static readonly TemplateModel ResultProtocol = new TemplateModel
    {
        Key = "vote_result",
        Filename = "Abstimmungsprotokoll_{0}_{1}",
        Description = "Protokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesCountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
        DomainOfInfluenceTypes = new HashSet<DomainOfInfluenceType>
        {
            DomainOfInfluenceType.Ch,
            DomainOfInfluenceType.Ct,
            DomainOfInfluenceType.Bz,
        },
        PerDomainOfInfluenceType = true,
    };

    /// <summary>
    /// Gets the e-voting result protocol export template.
    /// </summary>
    public static readonly TemplateModel EVotingDetailsResultProtocol = new TemplateModel
    {
        Key = "vote_result_details_e_voting",
        Filename = "Abstimmungsprotokoll_E-Voting_{0}",
        Description = "Protokoll inkl. Details E-Voting",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
        DomainOfInfluenceTypes = new HashSet<DomainOfInfluenceType>
        {
            DomainOfInfluenceType.Ch,
            DomainOfInfluenceType.Ct,
        },
    };

    /// <summary>
    /// Gets the counting circle e-voting result protocol export template.
    /// </summary>
    public static readonly TemplateModel EVotingCountingCircleResultProtocol = new TemplateModel
    {
        Key = "vote_couting_circle_result_details_e_voting",
        Filename = "Abstimmungsprotokoll_E-Voting_{0}",
        Description = "Protokoll inkl. Details E-Voting",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesCountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the e-voting result protocol export template.
    /// </summary>
    public static readonly TemplateModel EVotingResultProtocol = new TemplateModel
    {
        Key = "vote_result_e_voting_only",
        Filename = "Abstimmungsprotokoll_E-Voting_Ergebnisse_{0}",
        Description = "Protokoll E-Voting Ergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
        DomainOfInfluenceTypes = new HashSet<DomainOfInfluenceType>
        {
            DomainOfInfluenceType.Ch,
            DomainOfInfluenceType.Ct,
        },
    };

    /// <summary>
    /// Gets the result bundle review export template.
    /// </summary>
    public static readonly TemplateModel ResultBundleReview = new TemplateModel
    {
        Key = "vote_result_bundle_review",
        Filename = "Bundkontrolle {0}",
        Description = "Bundkontrolle",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.PoliticalBusinessResultBundleReview,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        EndResultProtocol,
        TemporaryEndResultDomainOfInfluencesProtocol,
        EndResultDomainOfInfluencesProtocol,
        ResultProtocol,
        EVotingDetailsResultProtocol,
        EVotingCountingCircleResultProtocol,
        EVotingResultProtocol,
        ResultBundleReview,
    };
}
