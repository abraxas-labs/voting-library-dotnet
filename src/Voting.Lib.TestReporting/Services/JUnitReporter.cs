// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Voting.Lib.TestReporting.Models.Comparison;
using Voting.Lib.TestReporting.Models.Comparison.JUnit;

namespace Voting.Lib.TestReporting.Services;

/// <summary>
/// A reporter generating a JUnit conform xml report according to schema <a href="https://github.com/windyroad/JUnit-Schema/blob/master/JUnit.xsd"/>.
/// </summary>
public class JUnitReporter
{
    private readonly ILogger<JUnitReporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JUnitReporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public JUnitReporter(ILogger<JUnitReporter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Writes a Junit XML report of the passed comparisons to the file system path.
    /// </summary>
    /// <param name="container">The comparison container.</param>
    /// <param name="path">The file path to write the xml to.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task WriteJUnitReport(ComparisonContainer container, string path)
    {
        var junitContainer = new JUnitTestSuites
        {
            TestSuites =
                {
                    new JUnitTestSuite
                    {
                        Failures = container.NotEqualEntries.Count(),
                        Name = container.Description,
                        Tests = container.AllEntries.Count(),
                        TestCases = container.AllEntries.Select(e => new JUnitTestCase
                        {
                            Classname = e.ParentDescription,
                            Name = e.Name,
                            SystemOut = e.DetailedDescription,
                            Failure = e.Equal ? null : new JUnitTestCaseFailure
                            {
                                Message = e.Description,
                                Type = "value comparison mismatch",
                                Content = e.DetailedDescription,
                            },
                        }).ToList(),
                    },
                },
        };

        _logger.LogInformation("writing junit report to {File}", path);

        await using var file = File.Create(path);
        var xmlSerializer = new XmlSerializer(typeof(JUnitTestSuites));
        xmlSerializer.Serialize(file, junitContainer);
    }
}
