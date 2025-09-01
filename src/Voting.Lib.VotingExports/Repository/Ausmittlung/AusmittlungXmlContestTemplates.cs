// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung XML templates for contest exports.
/// </summary>
public static class AusmittlungXmlContestTemplates
{
    /// <summary>
    /// Gets the eCH-0252 majority election result export template.
    /// </summary>
    public static readonly TemplateModel MajorityElectionResultsEch0252 = new()
    {
        Key = "majority_election_result_ech_0252",
        Filename = "eCH-0252_majority-election-result-delivery_{0}",
        Description = "Majorzwahlresultate (eCH-0252 Result Delivery)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0252 proportional election result export template.
    /// </summary>
    public static readonly TemplateModel ProportionalElectionResultsEch0252 = new()
    {
        Key = "proportional_election_result_ech_0252",
        Filename = "eCH-0252_proportional-election-result-delivery_{0}",
        Description = "Proporzwahlresultate (eCH-0252 Result Delivery)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0252 proportional election result export template with candidate list results info (Panaschierstatistik).
    /// </summary>
    public static readonly TemplateModel ProportionalElectionResultsWithCandidateListResultsInfoEch0252 = new()
    {
        Key = "proportional_election_result_with_candidate_list_results_info_ech_0252",
        Filename = "eCH-0252_proportional-election-result-delivery_with_info_{0}",
        Description = "Proporzwahlresultate inkl. Panaschierstatistik (eCH-0252 Result Delivery)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0252 vote result export template.
    /// </summary>
    public static readonly TemplateModel VoteResultsEch0252 = new()
    {
        Key = "vote_result_ech_0252",
        Filename = "eCH-0252_vote-result-delivery_{0}",
        Description = "Abstimmungsresultate (eCH-0252 Result Delivery)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0252 majority election information export template.
    /// </summary>
    public static readonly TemplateModel MajorityElectionInfosEch0252 = new()
    {
        Key = "majority_election_info_ech_0252",
        Filename = "eCH-0252_majority-election-info-delivery_{0}",
        Description = "Majorzwahlinformationen (eCH-0252 Information Delivery)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0252 proportional election information export template.
    /// </summary>
    public static readonly TemplateModel ProportionalElectionInfosEch0252 = new()
    {
        Key = "proportional_election_info_ech_0252",
        Filename = "eCH-0252_proportional-election-info-delivery_{0}",
        Description = "Proporzwahlinformationen (eCH-0252 Information Delivery)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        MajorityElectionInfosEch0252,
        ProportionalElectionInfosEch0252,
        MajorityElectionResultsEch0252,
        ProportionalElectionResultsEch0252,
        ProportionalElectionResultsWithCandidateListResultsInfoEch0252,
        VoteResultsEch0252,
    };
}
