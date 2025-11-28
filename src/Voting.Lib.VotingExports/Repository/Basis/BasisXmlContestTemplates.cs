// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Basis;

#pragma warning disable SA1310
/// <summary>
/// Basis XML templates for contest exports.
/// </summary>
public static class BasisXmlContestTemplates
{
    /// <summary>
    /// Gets the eCH-0157 and eCH-0159 v4.0 (semantic v4.0) contest export template.
    /// </summary>
    public static readonly TemplateModel Ech0157And0159_4_0 = new TemplateModel
    {
        Key = "contest_ech_0157_and_0159_4_0",
        Filename = "eCH_{0}",
        Description = "eCH-0157 / eCH-0159 v4.0",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        GeneratedBy = VotingApp.VotingBasis,
    };

    /// <summary>
    /// Gets the eCH-0157 and eCH-0159 v4.0 (semantic v4.0) contest export template (e-voting only).
    /// </summary>
    public static readonly TemplateModel Ech0157And0159_4_0_EVotingOnly = new TemplateModel
    {
        Key = "contest_ech_0157_and_0159_4_0_e_voting",
        Filename = "eCH_{0}_e_voting",
        Description = "eCH-0157 / eCH-0159 v4.0 (nur E-Voting)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        GeneratedBy = VotingApp.VotingBasis,
    };

    /// <summary>
    /// Gets the eCH-0157 and eCH-0159 v5.1 (semantic v4.2) contest export template.
    /// </summary>
    public static readonly TemplateModel Ech0157And0159_5_1 = new TemplateModel
    {
        Key = "contest_ech_0157_and_0159_5_1",
        Filename = "eCH_v4_0_{0}",
        Description = "eCH-0157 / eCH-0159 v4.2",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        GeneratedBy = VotingApp.VotingBasis,
    };

    /// <summary>
    /// Gets the eCH-0157 and eCH-0159 v5.1 (semantic v4.2) contest export template (e-voting only).
    /// </summary>
    public static readonly TemplateModel Ech0157And0159_5_1_EVotingOnly = new TemplateModel
    {
        Key = "contest_ech_0157_and_0159_5_1_e_voting",
        Filename = "eCH_v4_2_{0}_e_voting",
        Description = "eCH-0157 / eCH-0159 v4.2 (nur E-Voting)",
        Format = ExportFileFormat.Xml,
        EntityType = EntityType.Contest,
        GeneratedBy = VotingApp.VotingBasis,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        Ech0157And0159_4_0,
        Ech0157And0159_4_0_EVotingOnly,
        Ech0157And0159_5_1,
        Ech0157And0159_5_1_EVotingOnly,
    };
}
#pragma warning restore SA1310
