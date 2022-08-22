// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.VotingExports.Models;

namespace Voting.Lib.VotingExports.Repository.Ausmittlung;

/// <summary>
/// WabstiC Templates.
/// We use german names here since the entire wabstiC domain is in german and there are no eCH definitions.
/// See https://jira.eia.abraxas.ch/jira/browse/VOTING-647.
/// WM => all majority elections of a contest
/// WP => all proportional elections of a contest
/// SG => all votes of a contest.
/// </summary>
public static class AusmittlungWabstiCTemplates
{
#pragma warning disable CS1591
    public static readonly TemplateModel WMWahl = new TemplateModel
    {
        Key = "WM_Wahl",
        Filename = "WM_Wahl",
        Description = "WM_Wahl",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WMGemeinden = new TemplateModel
    {
        Key = "WM_Gemeinden",
        Filename = "WM_Gemeinden",
        Description = "WM_Gemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WMStaticGemeinden = new TemplateModel
    {
        Key = "WMStatic_Gemeinden",
        Filename = "WMStatic_Gemeinden",
        Description = "WMStatic_Gemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WMPlausiGemeinden = new TemplateModel
    {
        Key = "WM_PlausiGemeinden",
        Filename = "WM_PlausiGemeinden",
        Description = "WM_PlausiGemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WMKandidaten = new TemplateModel
    {
        Key = "WM_Kandidaten",
        Filename = "WM_Kandidaten",
        Description = "WM_Kandidaten",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WMKandidatenGde = new TemplateModel
    {
        Key = "WM_KandidatenGde",
        Filename = "WM_KandidatenGde",
        Description = "WM_KandidatenGde",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPWahl = new TemplateModel
    {
        Key = "WP_Wahl",
        Filename = "WP_Wahl",
        Description = "WP_Wahl",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPGemeinden = new TemplateModel
    {
        Key = "WP_Gemeinden",
        Filename = "WP_Gemeinden",
        Description = "WP_Gemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPStaticGemeinden = new TemplateModel
    {
        Key = "WPStatic_Gemeinden",
        Filename = "WPStatic_Gemeinden",
        Description = "WPStatic_Gemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPPlausiGemeinden = new TemplateModel
    {
        Key = "WP_PlausiGemeinden",
        Filename = "WP_PlausiGemeinden",
        Description = "WP_PlausiGemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPKandidaten = new TemplateModel
    {
        Key = "WP_Kandidaten",
        Filename = "WP_Kandidaten",
        Description = "WP_Kandidaten",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPStaticKandidaten = new TemplateModel
    {
        Key = "WPStatic_Kandidaten",
        Filename = "WPStatic_Kandidaten",
        Description = "WPStatic_Kandidaten",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPKandidatenGde = new TemplateModel
    {
        Key = "WP_KandidatenGde",
        Filename = "WP_KandidatenGde",
        Description = "WP_KandidatenGde",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPListen = new TemplateModel
    {
        Key = "WP_Listen",
        Filename = "WP_Listen",
        Description = "WP_Listen",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPListenGde = new TemplateModel
    {
        Key = "WP_ListenGde",
        Filename = "WP_ListenGde",
        Description = "WP_ListenGde",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel SGStaticGeschaefte = new TemplateModel
    {
        Key = "SGStatic_Geschäfte",
        Filename = "SGStatic_Geschäfte",
        Description = "SGStatic_Geschäfte",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel SGGeschaefte = new TemplateModel
    {
        Key = "SG_Geschäfte",
        Filename = "SG_Geschäfte",
        Description = "SG_Geschäfte",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel SGGemeinden = new TemplateModel
    {
        Key = "SG_Gemeinden",
        Filename = "SG_Gemeinden",
        Description = "SG_Gemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel SGStaticGemeinden = new TemplateModel
    {
        Key = "SGStatic_Gemeinden",
        Filename = "SGStatic_Gemeinden",
        Description = "SGStatic_Gemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel SGPlausiGemeinden = new TemplateModel
    {
        Key = "SG_PlausiGemeinden",
        Filename = "SG_PlausiGemeinden",
        Description = "SG_PlausiGemeinden",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };
#pragma warning restore CS1591

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        WMWahl,
        WMGemeinden,
        WMStaticGemeinden,
        WMPlausiGemeinden,
        WMKandidaten,
        WMKandidatenGde,
        WPWahl,
        WPGemeinden,
        WPStaticGemeinden,
        WPPlausiGemeinden,
        WPKandidaten,
        WPStaticKandidaten,
        WPKandidatenGde,
        WPListen,
        WPListenGde,
        SGStaticGeschaefte,
        SGGeschaefte,
        SGGemeinden,
        SGStaticGemeinden,
        SGPlausiGemeinden,
    };
}
