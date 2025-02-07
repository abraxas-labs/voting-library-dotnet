// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Xml.Serialization;

namespace Voting.Lib.Ech.Ech0045_4_0.Models;

/// <summary>
/// Swiss person extension.
/// Due to the given format by the post it is not possible distinguish between swiss domestic and swiss abroad persons.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "extension", IsNullable = false, Namespace = "http://www.ech.ch/xmlns/eCH-0045/4")]
public class SwissPersonExtension
{
    /// <summary>
    /// Gets or sets postage code of swiss living abroad.
    /// </summary>
    [XmlElement(ElementName = "postageCode", Namespace = "")]
    public string? PostageCode { get; set; }

    /// <summary>
    /// Gets or sets voting place of swiss living abroad.
    /// </summary>
    [XmlElement(ElementName = "votingPlace", Namespace = "")]
    public SwissAbroadPersonExtensionVotingPlace? VotingPlace { get; set; }

    /// <summary>
    /// Gets or sets address of swiss living abroad.
    /// </summary>
    [XmlElement(ElementName = "address", Namespace = "")]
    public SwissAbroadPersonExtensionAddress? Address { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the voting documents are sent to the address of the voter (<c>false</c>)
    /// or to the domain of influence documents return address (eg. the municipality administration, <c>true</c>).
    /// This is usually true for people without a permanent domicile.
    /// In german usually called 'nicht Zustellen' (do not deliver to the citizen).
    /// </summary>
    [XmlElement(ElementName = "sendVotingCardsToDomainOfInfluenceReturnAddress", Namespace = "")]
    public bool? SendVotingCardsToDomainOfInfluenceReturnAddress { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the person is householder of the residence.
    /// </summary>
    [XmlElement(ElementName = "isHouseholder", Namespace = "")]
    public bool IsHouseholder { get; set; }

    /// <summary>
    /// Gets or sets the residence building id.
    /// </summary>
    [XmlElement(ElementName = "residenceBuildingId", Namespace = "")]
    public int? ResidenceBuildingId { get; set; }

    /// <summary>
    /// Gets or sets the residence apartment id.
    /// </summary>
    [XmlElement(ElementName = "residenceApartmentId", Namespace = "")]
    public int? ResidenceApartmentId { get; set; }

    /// <summary>
    /// Whether or not the property <see cref="SendVotingCardsToDomainOfInfluenceReturnAddress"/> should be serialized.
    /// </summary>
    /// <returns>True if the property <see cref="SendVotingCardsToDomainOfInfluenceReturnAddress"/> has a value.</returns>
    public bool ShouldSerializeSendVotingCardsToDomainOfInfluenceReturnAddress()
        => SendVotingCardsToDomainOfInfluenceReturnAddress.HasValue;
}
