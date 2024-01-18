// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Ech0155_4_0;

namespace Voting.Lib.Ech.Ech0157_4_0.Models;

/// <summary>
/// Election information extension.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "extension", IsNullable = false, Namespace = "http://www.ech.ch/xmlns/eCH-0155/4")]
public class ElectionInformationExtension
{
    /// <summary>
    /// Gets or sets candidate extension as a lookup for <see cref="CandidateType"/>.
    /// </summary>
    [XmlArray(ElementName = "candidates", Namespace = "")]
    [XmlArrayItem(ElementName = "candidate", Namespace = "")]
    public List<ElectionInformationExtensionCandidate>? Candidates { get; set; }
}
