// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

/// <summary>
/// Basis XML templates for contest exports.
/// </summary>
public static class BasisXmlContestTemplates
{
    /// <summary>
    /// Gets the eCH-0157 and eCH-0159 contest export template.
    /// </summary>
    public static readonly TemplateModel Ech0157And0159 = new TemplateModel
    {
        Key = "contest_ech_0157_and_0159",
        Filename = "eCH_{0}",
        Description = "eCH-0157 / eCH-0159",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        GeneratedBy = VotingApp.VotingBasis,
    };

    /// <summary>
    /// Gets the eCH-0157 and eCH-0159 contest export template (e-voting only).
    /// </summary>
    public static readonly TemplateModel Ech0157And0159EVotingOnly = new TemplateModel
    {
        Key = "contest_ech_0157_and_0159_e_voting",
        Filename = "eCH_{0}_e_voting",
        Description = "eCH-0157 / eCH-0159 (nur E-Voting)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        Ech0157And0159,
        Ech0157And0159EVotingOnly,
    };
}
