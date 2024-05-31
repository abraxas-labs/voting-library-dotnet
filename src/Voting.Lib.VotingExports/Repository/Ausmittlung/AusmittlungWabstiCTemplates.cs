// (c) Copyright 2024 by Abraxas Informatik AG
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

    public static readonly TemplateModel WMWahlergebnisse = new TemplateModel
    {
        Key = "WM_Wahlergebnisse",
        Filename = "csv_Export_Detailergebnisse_{0}_{1}",
        Description = "CSV_Detailergebnisse",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.MajorityElection,
        ResultType = ResultType.PoliticalBusinessResult,
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

    public static readonly TemplateModel WPGemeindenSkStat = new TemplateModel
    {
        Key = "WP_Gemeinden_SK_STAT",
        Filename = "WP_Gemeinden_SK_STAT",
        Description = "WP_Gemeinden_SK_STAT",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPGemeindenBfs = new TemplateModel
    {
        Key = "WP_Gemeinden_BFS",
        Filename = "WP_Gemeinden_BFS",
        Description = "WP_Gemeinden_BFS",
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

    public static readonly TemplateModel WPListenGdeSkStat = new TemplateModel
    {
        Key = "WP_ListenGde_SK_STAT",
        Filename = "WP_ListenGde_SK_STAT",
        Description = "WP_ListenGde_SK_STAT",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPWahlEinzel = new TemplateModel
    {
        Key = "WP_Wahl_Einzel",
        Filename = "WP_Wahl_{0}",
        Description = "WP_Wahl_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPGemeindenEinzel = new TemplateModel
    {
        Key = "WP_Gemeinden_Einzel",
        Filename = "WP_Gemeinden_{0}",
        Description = "WP_Gemeinden_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPGemeindenSkStatEinzel = new TemplateModel
    {
        Key = "WP_Gemeinden_SK_STAT_Einzel",
        Filename = "WP_Gemeinden_SK_STAT_{0}",
        Description = "WP_Gemeinden_SK_STAT_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPGemeindenBfsEinzel = new TemplateModel
    {
        Key = "WP_Gemeinden_BFS_Einzel",
        Filename = "WP_Gemeinden_BFS_{0}",
        Description = "WP_Gemeinden_BFS_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPStaticGemeindenEinzel = new TemplateModel
    {
        Key = "WPStatic_Gemeinden_Einzel",
        Filename = "WPStatic_Gemeinden_{0}",
        Description = "WPStatic_Gemeinden_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPKandidatenEinzel = new TemplateModel
    {
        Key = "WP_Kandidaten_Einzel",
        Filename = "WP_Kandidaten_{0}",
        Description = "WP_Kandidaten_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPStaticKandidatenEinzel = new TemplateModel
    {
        Key = "WPStatic_Kandidaten_Einzel",
        Filename = "WPStatic_Kandidaten_{0}",
        Description = "WPStatic_Kandidaten_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPKandidatenGdeEinzel = new TemplateModel
    {
        Key = "WP_KandidatenGde_Einzel",
        Filename = "WP_KandidatenGde_{0}",
        Description = "WP_KandidatenGde_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPListenEinzel = new TemplateModel
    {
        Key = "WP_Listen_Einzel",
        Filename = "WP_Listen_{0}",
        Description = "WP_Listen_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPListenGdeEinzel = new TemplateModel
    {
        Key = "WP_ListenGde_Einzel",
        Filename = "WP_ListenGde_{0}",
        Description = "WP_ListenGde_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel WPListenGdeSkStatEinzel = new TemplateModel
    {
        Key = "WP_ListenGde_SK_STAT_Einzel",
        Filename = "WP_ListenGde_SK_STAT_{0}",
        Description = "WP_ListenGde_SK_STAT_Einzel",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.ProportionalElection,
        ResultType = ResultType.PoliticalBusinessResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel SGStaticGeschaefte = new TemplateModel
    {
        Key = "SGStatic_Geschäfte",
        Filename = "SGStatic_Geschaefte",
        Description = "SGStatic_Geschäfte",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };

    public static readonly TemplateModel SGGeschaefte = new TemplateModel
    {
        Key = "SG_Geschäfte",
        Filename = "SG_Geschaefte",
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

    public static readonly TemplateModel SGAbstimmungsergebnisse = new TemplateModel
    {
        Key = "SG_Abstimmungsergebnisse",
        Filename = "csv_Export_Detailergebnisse_{0}",
        Description = "CSV_Detailergebnisse",
        Format = ExportFileFormat.Csv,
        EntityType = EntityType.Vote,
        ResultType = ResultType.MultiplePoliticalBusinessesResult,
        GeneratedBy = VotingApp.VotingAusmittlung,
    };
#pragma warning restore CS1591

    internal static readonly IReadOnlyCollection<TemplateModel> All = new[]
    {
        WMWahl,
        WMWahlergebnisse,
        WMGemeinden,
        WMStaticGemeinden,
        WMKandidaten,
        WMKandidatenGde,
        WPWahl,
        WPGemeinden,
        WPGemeindenSkStat,
        WPGemeindenBfs,
        WPStaticGemeinden,
        WPKandidaten,
        WPStaticKandidaten,
        WPKandidatenGde,
        WPListen,
        WPListenGde,
        WPListenGdeSkStat,
        WPWahlEinzel,
        WPGemeindenEinzel,
        WPGemeindenSkStatEinzel,
        WPGemeindenBfsEinzel,
        WPStaticGemeindenEinzel,
        WPKandidatenEinzel,
        WPStaticKandidatenEinzel,
        WPKandidatenGdeEinzel,
        WPListenEinzel,
        WPListenGdeEinzel,
        WPListenGdeSkStatEinzel,
        SGStaticGeschaefte,
        SGGeschaefte,
        SGGemeinden,
        SGStaticGemeinden,
        SGAbstimmungsergebnisse,
    };
}
