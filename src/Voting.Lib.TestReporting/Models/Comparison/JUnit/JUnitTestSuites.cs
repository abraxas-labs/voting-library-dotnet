// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Voting.Lib.TestReporting.Models.Comparison.JUnit;

/// <summary>
/// JUnit test suites XML root element.
/// Schema see <a href="https://github.com/windyroad/JUnit-Schema/blob/master/JUnit.xsd"/>.
/// </summary>
[XmlRoot(ElementName = "testsuites")]
public class JUnitTestSuites
{
    /// <summary>
    /// Gets or sets test suites.
    /// </summary>
    [XmlElement(ElementName = "testsuite")]
    public List<JUnitTestSuite> TestSuites { get; set; } = new();
}
