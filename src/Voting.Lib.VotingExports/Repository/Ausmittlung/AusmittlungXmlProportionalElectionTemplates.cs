// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung XML templates for proportional election exports.
/// </summary>
public static class AusmittlungXmlProportionalElectionTemplates
{
    /// <summary>
    /// Gets the eCH-0110 proportional election result export template.
    /// </summary>
    public static readonly TemplateModel Ech0110 = new TemplateModel
    {
        Key = "proportional_election_ech_0110",
        Filename = "eCH-0110_{0}",
        Description = "Proporzwahlresultat (eCH-0110)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the eCH-0222 proportional election result export template.
    /// </summary>
    public static readonly TemplateModel Ech0222 = new TemplateModel
    {
        Key = "proportional_election_ech_0222",
        Filename = "eCH-0222_{0}",
        Description = "Rohdaten Proporzwahl (eCH-0222)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        Ech0110,
        Ech0222,
    };
}
