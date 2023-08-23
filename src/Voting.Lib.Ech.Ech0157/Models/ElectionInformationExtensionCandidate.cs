// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Xml.Serialization;
using Eai.Common.eCH.AttributeChecker;

namespace Voting.Lib.Ech.Ech0157.Models;

/// <summary>
/// Election information extension for canidate used as a look-up table referencing one
/// of the candidates under <see cref="eCH_0157_4_0.ElectionInformationType.Candidate"/>.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "candidate", IsNullable = false)]
public class ElectionInformationExtensionCandidate : FieldValueChecker<ElectionInformationExtensionCandidate>
{
    private string _candidateIdentification = string.Empty;
    private string? _titleAndOccupation;

    /// <summary>
    /// Gets or sets candidate identification.
    /// </summary>
    [FieldRequired]
    [FieldMinMaxLength(1, 50)]
    [XmlElement("candidateIdentification")]
    public string CandidateIdentification
    {
        get => _candidateIdentification;
        set => CheckAndSetValue(ref _candidateIdentification, value);
    }

    /// <summary>
    /// Gets or sets the candidate title and occupation.
    /// </summary>
    [FieldMinMaxLength(1, 250)]
    [XmlElement("titleAndOccupation")]
    public string? TitleAndOccupation
    {
        get => _titleAndOccupation;
        set => CheckAndSetValue(ref _titleAndOccupation, value);
    }
}
