// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

#pragma warning disable SA1310
/// <summary>
/// Basis XML templates for proportional election exports.
/// </summary>
public static class BasisXmlProportionalElectionTemplates
{
    /// <summary>
    /// Gets the eCH-0157 v4.0 (semantic v4.0) proportional election export template.
    /// </summary>
    public static readonly TemplateModel Ech0157_4_0 = new TemplateModel
    {
        Key = "proportional_election_ech_0157_4_0",
        Filename = "eCH-0157_v4_0_{0}",
        Description = "eCH-0157 v4.0",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.ProportionalElection,
        GeneratedBy = VotingApp.VotingBasis,
    };

    /// <summary>
    /// Gets the eCH-0157 v4.2 (semantic v5.1) proportional election export template.
    /// </summary>
    public static readonly TemplateModel Ech0157_5_1 = new TemplateModel
    {
        Key = "proportional_election_ech_0157_5_1",
        Filename = "eCH-0157_v4_2_{0}",
        Description = "eCH-0157 v4.2",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.ProportionalElection,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        Ech0157_4_0,
        Ech0157_5_1,
    };
}
#pragma warning restore SA1310
