// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Xml.Serialization;

namespace Voting.Lib.TestReporting.Models.Comparison.JUnit;

/// <summary>
/// JUnit test case XML root element.
/// Schema see <a href="https://github.com/windyroad/JUnit-Schema/blob/master/JUnit.xsd"/>.
/// </summary>
[XmlRoot(ElementName = "testcase")]
public class JUnitTestCase
{
    /// <summary>
    /// Gets or sets the test case name.
    /// </summary>
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the test case classname.
    /// </summary>
    [XmlAttribute(AttributeName = "classname")]
    public string Classname { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time in seconds.
    /// </summary>
    [XmlAttribute(AttributeName = "time")]
    public decimal Time { get; set; }

    /// <summary>
    /// Gets or sets the test case failure.
    /// </summary>
    [XmlElement("failure")]
    public JUnitTestCaseFailure? Failure { get; set; }

    /// <summary>
    /// Gets or sets the test case system out.
    /// </summary>
    [XmlElement("system-out")]
    public string? SystemOut { get; set; }
}
