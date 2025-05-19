// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung PDF templates for secondary majority election exports.
/// </summary>
public static class AusmittlungPdfSecondaryMajorityElectionTemplates
{
    /// <summary>
    /// Secondary majority election template key prefix.
    /// </summary>
    public const string SecondaryMajorityElectionTemplateKeyPrefix = "secondary_";

    /// <summary>
    /// Gets the counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel CountingCircleProtocol = new TemplateModel
    {
        Key = SecondaryMajorityElectionTemplateKeyPrefix + "majority_election_counting_circle_protocol",
        Filename = "Majorz_Gemeindeprotokoll_{0}_{1}",
        Description = "Gemeindeprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.SecondaryMajorityElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the counting circle e-voting protocol export template.
    /// </summary>
    public static readonly TemplateModel CountingCircleEVotingProtocol = new TemplateModel
    {
        Key = SecondaryMajorityElectionTemplateKeyPrefix + "majority_election_counting_circle_e_voting_protocol",
        Filename = "Majorz_Gemeindeprotokoll_EVoting_{0}_{1}",
        Description = "Gemeindeprotokoll E-Voting",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.SecondaryMajorityElection,
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
        Key = SecondaryMajorityElectionTemplateKeyPrefix + "majority_election_end_result_protocol",
        Filename = "Majorz_Wahlprotokoll_{0}_{1}",
        Description = "Wahlprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.SecondaryMajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result e-voting protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultEVotingProtocol = new TemplateModel
    {
        Key = SecondaryMajorityElectionTemplateKeyPrefix + "majority_election_end_result_e_voting_protocol",
        Filename = "Majorz_Wahlprotokoll_EVoting_{0}_{1}",
        Description = "Wahlprotokoll E-Voting",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.SecondaryMajorityElection,
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
    /// Gets the end result detail protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultDetailProtocol = new TemplateModel
    {
        Key = SecondaryMajorityElectionTemplateKeyPrefix + "majority_election_end_result_detail_protocol",
        Filename = "Majorz_Detailergebnisse_{0}_{1}",
        Description = "Detailergebnisse",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.SecondaryMajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result detail protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultDetailWithoutEmptyAndInvalidVotesProtocol = new TemplateModel
    {
        Key = SecondaryMajorityElectionTemplateKeyPrefix + "majority_election_end_result_detail_without_empty_and_invalid_votes_protocol",
        Filename = "Majorz_Detailergebnisse_exkl_{0}_{1}",
        Description = "Detailergebnisse_exkl",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.SecondaryMajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
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
    };
}
