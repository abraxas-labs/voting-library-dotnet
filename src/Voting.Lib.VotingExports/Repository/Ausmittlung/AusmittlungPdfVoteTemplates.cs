// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung PDF templates for vote exports.
/// </summary>
public static class AusmittlungPdfVoteTemplates
{
    /// <summary>
    /// Gets the end result protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultProtocol = new TemplateModel
    {
        Key = "vote_overall_result",
        Filename = "Gesamtergebnis aller Sachgeschäfte {0}",
        Description = "Gesamtergebnis aller Sachgeschäfte",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
        PerDomainOfInfluenceType = true,
    };

    /// <summary>
    /// Gets the temporary end result protocol domain of influences export template.
    /// </summary>
    public static readonly TemplateModel TemporaryEndResultDomainOfInfluencesProtocol = new TemplateModel
    {
        Key = "vote_domain_of_influence_overall_temporary_end_result",
        Filename = "Prov. Ergebnisse aller Auszählungskreise - {0}",
        Description = "Prov. Ergebnisse aller Auszählungskreise",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result domain of influences protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultDomainOfInfluencesProtocol = new TemplateModel
    {
        Key = "vote_domain_of_influence_overall_end_result",
        Filename = "Definitive Ergebnisse aller Auszählungskreise - {0}",
        Description = "Definitive Ergebnisse aller Auszählungskreise",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the result protocol export template.
    /// </summary>
    public static readonly TemplateModel ResultProtocol = new TemplateModel
    {
        Key = "vote_result",
        Filename = "Abstimmungsprotokoll {0}",
        Description = "Protokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesCountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
        PerDomainOfInfluenceType = true,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        EndResultProtocol,
        TemporaryEndResultDomainOfInfluencesProtocol,
        EndResultDomainOfInfluencesProtocol,
        ResultProtocol,
    };
}
