// (c) Copyright 2024 by Abraxas Informatik AG
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
    public static readonly TemplateModel MajorityElectionsEch0252 = new()
    {
        Key = "majority_election_ech_0252",
        Filename = "eCH-0252_majority-elections_{0}",
        Description = "Majorzwahlresultate (eCH-0252)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0252 proportional election result export template.
    /// </summary>
    public static readonly TemplateModel ProportionalElectionsEch0252 = new()
    {
        Key = "proportional_election_ech_0252",
        Filename = "eCH-0252_proportional-elections_{0}",
        Description = "Proporzwahlresultate (eCH-0252)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0252 vote result export template.
    /// </summary>
    public static readonly TemplateModel VoteEch0252 = new()
    {
        Key = "vote_ech_0252",
        Filename = "eCH-0252_votes_{0}",
        Description = "Abstimmungsresultate (eCH-0252)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        MajorityElectionsEch0252,
        ProportionalElectionsEch0252,
        VoteEch0252,
    };
}
