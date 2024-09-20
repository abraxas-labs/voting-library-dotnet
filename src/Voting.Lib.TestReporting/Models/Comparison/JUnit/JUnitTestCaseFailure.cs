// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Xml.Serialization;

namespace Voting.Lib.TestReporting.Models.Comparison.JUnit;

/// <summary>
/// JUnit test case failure XML root element.
/// Schema see <a href="https://github.com/windyroad/JUnit-Schema/blob/master/JUnit.xsd"/>.
/// </summary>
[XmlRoot(ElementName = "failure")]
public class JUnitTestCaseFailure
{
    /// <summary>
    /// Gets or sets the test case failure type.
    /// </summary>
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the test case failure message.
    /// </summary>
    [XmlAttribute("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the test case failure content.
    /// </summary>
    [XmlText]
    public string Content { get; set; } = string.Empty;
}
