// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Voting.Lib.TestReporting.Models.Comparison;
using Xunit;

namespace Voting.Lib.TestReporting.Test;

public class ComparisonContainerTest
{
    private const string ParentDescription = "Parent";
    private const string FurtherDescription = "with child";
    private const string ChildDescription = "Child";
    private const string ComparisonNameInt = "int value comparison";
    private const string ComparisonNameStr = "string value comparison";

    [Fact]
    public void ComparisonContainer_ShouldBeOk_WhenAllEntriesEqual()
    {
        // Arrange
        var comparisonContainer = new ComparisonContainer(ParentDescription, FurtherDescription);
        var childContainer = comparisonContainer.NewContainer(ChildDescription);

        // Act
        comparisonContainer.AddEntry(ComparisonNameInt, 10, 10);
        childContainer.AddEntry(ComparisonNameStr, "same", "same");

        // Assert
        comparisonContainer.EntriesCount.Should().Be(2);
        comparisonContainer.Ok.Should().BeTrue();
        comparisonContainer.Description.Should().Be($"{ParentDescription} > {FurtherDescription}");

        childContainer.EntriesCount.Should().Be(1);
        childContainer.Ok.Should().BeTrue();
        childContainer.Description.Should().Be($"{ParentDescription} > {FurtherDescription} > {ChildDescription}");
    }

    [Fact]
    public void ComparisonContainer_ShouldBeNotOk_WhenEntriesAreNotEqual()
    {
        // Arrange
        var comparisonContainer = new ComparisonContainer(ParentDescription, FurtherDescription);
        var childContainer = comparisonContainer.NewContainer(ChildDescription);

        // Act
        comparisonContainer.AddEntry(ComparisonNameInt, 10, 20);
        childContainer.AddEntry(ComparisonNameStr, "same", "not same");

        // Assert
        comparisonContainer.EntriesCount.Should().Be(2);
        comparisonContainer.NotEqualEntries.Count().Should().Be(2);
        comparisonContainer.Ok.Should().BeFalse();
        comparisonContainer.AllEntries.First().Description.Should().Be($"[{ComparisonNameInt}]: Left: 10, Right: 20");

        childContainer.EntriesCount.Should().Be(1);
        childContainer.Ok.Should().BeFalse();
        childContainer.AllEntries.First().Description.Should().Be($"[{ComparisonNameStr}]: Left: same, Right: not same");
    }

    [Fact]
    public void ComparisonContainer_ShouldBeOk_WhenComparingIdenticalLists()
    {
        // Arrange
        var comparerContext = new Mock<IComparerContext>().Object;
        var comparisonContainer = new ComparisonContainer(ParentDescription, FurtherDescription);

        var listToCompare = new List<string>
        {
            "value1",
            "value2",
        };

        // Act
        comparisonContainer.CompareList(comparerContext, listToCompare, listToCompare, item => item, CompareResult);

        // Assert
        comparisonContainer.Ok.Should().BeTrue();
        comparisonContainer.EntriesCount.Should().Be(5);
    }

    [Fact]
    public async Task ComparisonContainer_ShouldBeNotOk_WhenComparingNotEqualListsAsync()
    {
        // Arrange
        var comparerContext = new Mock<IComparerContext>().Object;
        var comparisonContainer = new ComparisonContainer(ParentDescription, FurtherDescription);

        var listLeft = new List<string>
        {
            "value1",
            "value2",
        };

        var listRight = new List<string>
        {
            "value1",
            "value3",
        };

        // Act
        await comparisonContainer
            .CompareListAsync(comparerContext, listLeft, listRight, item => item, CompareResultAsync)
            .ConfigureAwait(false);

        // Assert
        comparisonContainer.Ok.Should().BeFalse();
        comparisonContainer.EntriesCount.Should().Be(4);
    }

    private static void CompareResult(
        IComparerContext ctx,
        ComparisonContainer comparisons,
        string valueLeft,
        string valueRight)
    {
        var listEntryComparisons = comparisons.NewContainer(valueLeft);

        listEntryComparisons.AddEntry(
            "Name",
            valueLeft,
            valueRight);
    }

    private static Task CompareResultAsync(
        IComparerContext ctx,
        ComparisonContainer comparisons,
        string valueLeft,
        string valueRight)
    {
        CompareResult(ctx, comparisons, valueLeft, valueRight);
        return Task.CompletedTask;
    }
}
