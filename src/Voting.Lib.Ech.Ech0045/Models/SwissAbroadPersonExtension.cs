// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Xml.Serialization;

namespace Voting.Lib.Ech.Ech0045.Models;

/// <summary>
/// Swiss abroad person extension.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "extension", IsNullable = false, Namespace = "http://www.ech.ch/xmlns/eCH-0045/4")]
public class SwissAbroadPersonExtension
{
    /// <summary>
    /// Gets or sets postage code.
    /// </summary>
    [XmlElement(ElementName = "postageCode", Namespace = "")]
    public string PostageCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets voting place.
    /// </summary>
    [XmlElement(ElementName = "votingPlace", Namespace = "")]
    public SwissAbroadPersonExtensionVotingPlace VotingPlace { get; set; } = new();

    /// <summary>
    /// Gets or sets address.
    /// </summary>
    [XmlElement(ElementName = "address", Namespace = "")]
    public SwissAbroadPersonExtensionAddress Address { get; set; } = new();
}
