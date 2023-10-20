// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung CSV templates for contest exports.
/// </summary>
public static class AusmittlungCsvContestTemplates
{
    /// <summary>
    /// Gets the activity protocol export template.
    /// </summary>
    public static readonly TemplateModel ActivityProtocol = new TemplateModel
    {
        Key = "contest_detailed_activity_protocol",
        Filename = "Aktivitätenprotokoll",
        Description = "Aktivitätenprotokoll",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        ActivityProtocol,
    };
}
