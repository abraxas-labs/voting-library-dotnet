// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung CSV templates for vote exports.
/// </summary>
public static class AusmittlungCsvVoteTemplates
{
    /// <summary>
    /// Gets the e-voting details CSV export template.
    /// </summary>
    public static readonly TemplateModel EVotingDetails = new()
    {
        Key = "vote_e_voting_details",
        Filename = "Abstimmungsergebnisse_E-Voting",
        Description = "Abstimmungsergebnis(se) inkl. Details E-Voting",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        EVotingDetails,
    };
}
