// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Xml.Serialization;
using Ech0157_4_0;

namespace Voting.Lib.Ech.Ech0157_4_0.Models;

/// <summary>
/// Election information extension for canidate used as a look-up table referencing one
/// of the candidates under <see cref="EventInitialDeliveryElectionGroupBallotElectionInformation.Candidate"/>.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "candidate", IsNullable = false)]
public class ElectionInformationExtensionCandidate
{
    /// <summary>
    /// Gets or sets candidate identification.
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1)]
    [System.ComponentModel.DataAnnotations.MinLengthAttribute(50)]
    [System.ComponentModel.DataAnnotations.RequiredAttribute]
    [XmlElement("candidateIdentification")]
    public string CandidateIdentification { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the candidate title and occupation.
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLengthAttribute(1)]
    [System.ComponentModel.DataAnnotations.MinLengthAttribute(250)]
    [XmlElement("titleAndOccupation")]
    public string? TitleAndOccupation { get; set; }
}
