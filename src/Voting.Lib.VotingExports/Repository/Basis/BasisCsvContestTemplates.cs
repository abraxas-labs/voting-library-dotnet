// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

/// <summary>
/// Basis CSV templates for contest exports.
/// </summary>
public static class BasisCsvContestTemplates
{
    /// <summary>
    /// Gets the candidate list contest export template.
    /// </summary>
    public static readonly TemplateModel CandidateList = new TemplateModel
    {
        Key = "contest_candidate_list",
        Filename = "candidate_list_{0}",
        Description = "Kandidierendenverzeichnis",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Contest,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        CandidateList,
    };
}
