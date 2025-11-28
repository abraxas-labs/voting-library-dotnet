// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

#pragma warning disable SA1310
/// <summary>
/// Basis XML templates for vote exports.
/// </summary>
public static class BasisXmlVoteTemplates
{
    /// <summary>
    /// Gets the eCH-0159 v4.0 (semantic v4.0) vote export template.
    /// </summary>
    public static readonly TemplateModel Ech0159_4_0 = new TemplateModel
    {
        Key = "vote_ech_0159_4_0",
        Filename = "eCH-0159_v4_0_{0}",
        Description = "eCH-0159 v4.0",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Vote,
        GeneratedBy = VotingApp.VotingBasis,
    };

    /// <summary>
    /// Gets the eCH-0159 v4.2 (semantic v5.1) vote export template.
    /// </summary>
    public static readonly TemplateModel Ech0159_5_1 = new TemplateModel
    {
        Key = "vote_ech_0159_5_1",
        Filename = "eCH-0159_v4_2_{0}",
        Description = "eCH-0159 v4.2",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Vote,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        Ech0159_4_0,
        Ech0159_5_1,
    };
}
#pragma warning restore SA1310
