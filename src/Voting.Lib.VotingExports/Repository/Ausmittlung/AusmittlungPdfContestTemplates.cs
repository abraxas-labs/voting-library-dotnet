// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung PDF templates for contest exports.
/// </summary>
public static class AusmittlungPdfContestTemplates
{
    /// <summary>
    /// Gets the activity protocol export template.
    /// </summary>
    public static readonly TemplateModel ActivityProtocol = new TemplateModel
    {
        Key = "contest_activity_protocol",
        Filename = "Aktivitätenprotokoll",
        Description = "Aktivitätenprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Contest,
        ResultType = ResultType.Contest,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        ActivityProtocol,
    };
}
