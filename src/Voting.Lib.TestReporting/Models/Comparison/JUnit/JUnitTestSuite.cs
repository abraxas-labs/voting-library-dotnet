// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Voting.Lib.TestReporting.Models.Comparison.JUnit;

/// <summary>
/// JUnit test suite XML root element.
/// Schema see <a href="https://github.com/windyroad/JUnit-Schema/blob/master/JUnit.xsd"/>.
/// </summary>
[XmlRoot(ElementName = "testsuite")]
public class JUnitTestSuite
{
    /// <summary>
    /// Gets or sets the testsuite id.
    /// </summary>
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the testsuite package.
    /// </summary>
    [XmlAttribute(AttributeName = "package")]
    public string Package { get; set; } = "Voting.Simulation";

    /// <summary>
    /// Gets or sets the test suite's test cases.
    /// </summary>
    [XmlElement(ElementName = "testcase")]
    public List<JUnitTestCase> TestCases { get; set; } = new();

    /// <summary>
    /// Gets or sets the testsuite system out.
    /// </summary>
    [XmlElement(ElementName = "system-out")]
    public string Systemout { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the testsuite system error.
    /// </summary>
    [XmlElement(ElementName = "system-err")]
    public string Systemerr { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the testsuite name.
    /// </summary>
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the testsuite total tests count.
    /// </summary>
    [XmlAttribute(AttributeName = "tests")]
    public int Tests { get; set; }

    /// <summary>
    /// Gets or sets the testsuite skipped tests count.
    /// </summary>
    [XmlAttribute(AttributeName = "skipped")]
    public int Skipped { get; set; }

    /// <summary>
    /// Gets or sets the testsuite failed tests count.
    /// </summary>
    [XmlAttribute(AttributeName = "failures")]
    public int Failures { get; set; }

    /// <summary>
    /// Gets or sets the testsuite error tests count.
    /// </summary>
    [XmlAttribute(AttributeName = "errors")]
    public int Errors { get; set; }

    /// <summary>
    /// Gets or sets the time in seconds.
    /// </summary>
    [XmlAttribute(AttributeName = "time")]
    public decimal Time { get; set; }

    /// <summary>
    /// Gets or sets the testsuite created timestamp.
    /// Default is <see cref="DateTime.UtcNow"/>.
    /// </summary>
    [XmlAttribute(AttributeName = "timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the testsuite hostname.
    /// Default is <see cref="Environment.MachineName"/>.
    /// </summary>
    [XmlAttribute(AttributeName = "hostname")]
    public string Hostname { get; set; } = Environment.MachineName;
}
