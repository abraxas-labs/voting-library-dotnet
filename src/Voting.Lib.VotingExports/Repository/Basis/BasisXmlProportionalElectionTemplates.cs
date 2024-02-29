// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

/// <summary>
/// Basis XML templates for proportional election exports.
/// </summary>
public static class BasisXmlProportionalElectionTemplates
{
    /// <summary>
    /// Gets the eCH-0157 proportional election export template.
    /// </summary>
    public static readonly TemplateModel Ech0157 = new TemplateModel
    {
        Key = "proportional_election_ech_0157",
        Filename = "{0}",
        Description = "eCH-0157",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.ProportionalElection,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        Ech0157,
    };
}
