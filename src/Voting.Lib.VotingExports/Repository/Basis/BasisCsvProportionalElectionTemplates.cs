// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

/// <summary>
/// Basis CSV templates for proportional election exports.
/// </summary>
public static class BasisCsvProportionalElectionTemplates
{
    /// <summary>
    /// Gets the candidate list proportional election export template.
    /// </summary>
    public static readonly TemplateModel CandidateList = new TemplateModel
    {
        Key = "proportional_election_candidate_list",
        Filename = "candidate_list_{0}",
        Description = "Kandidierendenverzeichnis",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        CandidateList,
    };
}
