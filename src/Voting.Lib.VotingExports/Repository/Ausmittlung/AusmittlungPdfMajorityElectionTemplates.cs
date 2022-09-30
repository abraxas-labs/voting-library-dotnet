// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// Ausmittlung PDF templates for majority election exports.
/// </summary>
public static class AusmittlungPdfMajorityElectionTemplates
{
    /// <summary>
    /// Gets the counting circle protocol export template.
    /// </summary>
    public static readonly TemplateModel CountingCircleProtocol = new TemplateModel
    {
        Key = "majority_election_counting_circle_protocol",
        Filename = "Gemeindeprotokoll - {0}",
        Description = "Gemeindeprotokoll",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.CountingCircleResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultProtocol = new TemplateModel
    {
        Key = "majority_election_end_result_protocol",
        Filename = "Wahlprotokoll Gesamtergebnis aller Einheiten - {0}",
        Description = "Gesamtergebnis aller Einheiten",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the end result detail protocol export template.
    /// </summary>
    public static readonly TemplateModel EndResultDetailProtocol = new TemplateModel
    {
        Key = "majority_election_end_result_detail_protocol",
        Filename = "Detailergebniss aller einseh. Einheiten - {0}",
        Description = "Detailergebnisse aller einsehbaren Einheiten",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    /// <summary>
    /// Gets the result bundle review export template.
    /// </summary>
    public static readonly TemplateModel ResultBundleReview = new TemplateModel
    {
        Key = "majority_election_result_bundle_review",
        Filename = "Bundkontrolle {0}",
        Description = "Bundkontrolle",
        Format = ExportFileFormat.Pdf,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResultBundleReview,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        CountingCircleProtocol,
        EndResultProtocol,
        EndResultDetailProtocol,
        ResultBundleReview,
    };
}
