// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Xml.Serialization;
using Ech0010_6_0;

namespace Voting.Lib.Ech.Ech0045_4_0.Models;

/// <summary>
/// Swiss abroad person extension voting place.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "votingPlace", IsNullable = false)]
public class SwissAbroadPersonExtensionVotingPlace
{
    /// <summary>
    /// Gets or sets organisation.
    /// </summary>
    [XmlElement("organisation")]
    public OrganisationMailAddressInfoType Organisation { get; set; } = new();

    /// <summary>
    /// Gets or sets address information.
    /// </summary>
    [XmlElement("addressInformation")]
    public AddressInformationType AddressInformation { get; set; } = new();
}
