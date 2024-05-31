// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Voting.Lib.TestReporting.Models.Comparison;
using Voting.Lib.TestReporting.Services;
using Xunit;

namespace Voting.Lib.TestReporting.Test;

public class JUnitReporterTest
{
    private const string ReportFilePath = "test-report.xml";

    [Fact]
    public async Task WriteJUnitReport_ShouldWriteReportToFileWithoutTestFailures_WhenComparisonsOk()
    {
        // Arrange
        var comparisonContainer = GetArrangedComparisonContainer(true);

        var loggerMock = new Mock<ILogger<JUnitReporter>>();
        var jUnitReporter = new JUnitReporter(loggerMock.Object);

        // Act
        await jUnitReporter.WriteJUnitReport(comparisonContainer, ReportFilePath);

        // Assert
        File.Exists(ReportFilePath).Should().BeTrue();

        // Clean up
        File.Delete(ReportFilePath);
    }

    [Fact]
    public async Task WriteJUnitReport_ShouldWriteReportToFileWithTestFailures_WhenComparisonsNotOk()
    {
        // Arrange
        var comparisonContainer = GetArrangedComparisonContainer(false);

        var loggerMock = new Mock<ILogger<JUnitReporter>>();
        var jUnitReporter = new JUnitReporter(loggerMock.Object);

        // Act
        await jUnitReporter.WriteJUnitReport(comparisonContainer, ReportFilePath);

        // Assert
        File.Exists(ReportFilePath).Should().BeTrue();
        (await File.ReadAllTextAsync(ReportFilePath))
            .Should().Contain("value comparison mismatch");

        // Clean up
        File.Delete(ReportFilePath);
    }

    private static ComparisonContainer GetArrangedComparisonContainer(bool withEqualEntries)
    {
        var comparisonContainer = new ComparisonContainer("TestContainer");
        comparisonContainer.AddEntry("IntValue", 10, 10);

        var childContainer = comparisonContainer.NewContainer("ChildContainer");
        childContainer.AddEntry("StringValue", "same", withEqualEntries ? "same" : "not same");

        return comparisonContainer;
    }
}
