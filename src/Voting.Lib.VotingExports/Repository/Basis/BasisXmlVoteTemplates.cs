// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

/// <summary>
/// Basis XML templates for vote exports.
/// </summary>
public static class BasisXmlVoteTemplates
{
    /// <summary>
    /// Gets the eCH-0159 vote export template.
    /// </summary>
    public static readonly TemplateModel Ech0159 = new TemplateModel
    {
        Key = "vote_ech_0159",
        Filename = "{0}",
        Description = "eCH-0159",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Vote,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        Ech0159,
    };
}
