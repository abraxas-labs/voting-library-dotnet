// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Xml.Serialization;

namespace Voting.Lib.Ech.Ech0045_6_0.Models;

/// <summary>
/// Swiss abroad person extension address.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "address", IsNullable = false)]
public class SwissAbroadPersonExtensionAddress
{
    /// <summary>
    /// Gets or sets line 1.
    /// </summary>
    [XmlElement("line1")]
    public string Line1 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets line 2.
    /// </summary>
    [XmlElement("line2")]
    public string Line2 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets line 3.
    /// </summary>
    [XmlElement("line3")]
    public string Line3 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets line 4.
    /// </summary>
    [XmlElement("line4")]
    public string Line4 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets line 5.
    /// </summary>
    [XmlElement("line5")]
    public string Line5 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets line 6.
    /// </summary>
    [XmlElement("line6")]
    public string Line6 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets line 7.
    /// </summary>
    [XmlElement("line7")]
    public string Line7 { get; set; } = string.Empty;
}
