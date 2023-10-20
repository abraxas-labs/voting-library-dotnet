// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Voting.Lib.TestReporting.Models.Comparison;

/// <summary>
/// A comparison container holding comparison entries, descriptions and evaluation methods.
/// </summary>
public class ComparisonContainer
{
    private const string DescriptionHierarchySeparator = " > ";
    private const string DescriptionSeparator = " ";

    private readonly List<ComparisonContainer> _children = new();
    private readonly List<IComparisonEntry> _entries = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ComparisonContainer"/> class.
    /// </summary>
    /// <param name="parentDescription">The parent description.</param>
    /// <param name="descriptions">Further detailed descriptions.</param>
    public ComparisonContainer(string parentDescription, params string[] descriptions)
    {
        Description = BuildDescription(parentDescription, descriptions);
    }

    /// <summary>
    /// Gets the comparison container description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the total comparison entries count of the container including all children.
    /// </summary>
    public int EntriesCount => _children.Sum(c => c.EntriesCount) + _entries.Count;

    /// <summary>
    /// Gets all comparison entries of the container including all children.
    /// </summary>
    public IEnumerable<IComparisonEntry> AllEntries => _entries.Concat(_children.SelectMany(c => c.AllEntries));

    /// <summary>
    /// Gets all entries where the comparison result is not equal.
    /// </summary>
    public IEnumerable<IComparisonEntry> NotEqualEntries
        => _entries
            .Where(x => !x.Equal)
            .Concat(_children.SelectMany(x => x.NotEqualEntries));

    /// <summary>
    /// Gets a value indicating whether the comparison state is ok or not ok, where true means all entries are equal.
    /// </summary>
    public bool Ok => !NotEqualEntries.Any();

    /// <summary>
    /// Adds an entry to the containers comparison entry list.
    /// </summary>
    /// <typeparam name="T">The type of the comparison entry.</typeparam>
    /// <param name="name">The verbose comparison name.</param>
    /// <param name="valueLeft">The compare left value.</param>
    /// <param name="valueRight">The compare right value.</param>
    public void AddEntry<T>(string name, T valueLeft, T valueRight)
        where T : IComparable<T>
        => _entries.Add(new ComparisonEntry<T>(Description, name, valueLeft, valueRight));

    /// <summary>
    /// Adds an entry to the containers comparison entry list.
    /// </summary>
    /// <typeparam name="T">The type of the comparison entry.</typeparam>
    /// <param name="valueLeft">The compare left value.</param>
    /// <param name="valueRight">The compare right value.</param>
    /// <param name="leftName">The <see cref="CallerArgumentExpressionAttribute"/> for the parameter 'valueLeft'.</param>
    /// <param name="rightName">The <see cref="CallerArgumentExpressionAttribute"/> for the parameter 'valueRight'.</param>
    public void AddEntry<T>(
        T valueLeft,
        T valueRight,
        [CallerArgumentExpression("valueLeft")] string leftName = "",
        [CallerArgumentExpression("valueRight")] string rightName = "")
        where T : IComparable<T>
    {
        var lastExpressionMemberNameLeft = GetLastExpressionMemberName(leftName);
        var lastExpressionMemberNameRight = GetLastExpressionMemberName(rightName);

        if (lastExpressionMemberNameLeft != lastExpressionMemberNameRight)
        {
            throw new InvalidOperationException($"{nameof(valueLeft)} / {nameof(valueRight)} do not use the same member name ({leftName} vs {rightName}), use the explicit overload.");
        }

        AddEntry(lastExpressionMemberNameLeft, valueLeft, valueRight);
    }

    /// <summary>
    /// Processes comparison of lists including count comparison entry.
    /// In case of unequal list entries count further element comparisons will not take place.
    /// </summary>
    /// <typeparam name="TListElement">The list item type.</typeparam>
    /// <typeparam name="TContext">The context item type.</typeparam>
    /// <param name="ctx">The compare context.</param>
    /// <param name="listLeft">The list for left comparison.</param>
    /// <param name="listRight">The list for right comparison.</param>
    /// <param name="listItemIdentifierExtractor">The list item identifier extractor function.</param>
    /// <param name="listItemCompareFunction">The list item compare function to execute.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task CompareListAsync<TListElement, TContext>(
        TContext ctx,
        IReadOnlyList<TListElement> listLeft,
        IReadOnlyList<TListElement> listRight,
        Func<TListElement, string> listItemIdentifierExtractor,
        Func<TContext, ComparisonContainer, TListElement, TListElement, Task> listItemCompareFunction)
    {
        AddEntry(listLeft.Count, listRight.Count);

        for (var i = 0; i < listLeft.Count; i++)
        {
            if (!TryAddEqualIdentifierEntry(listLeft, listRight, i, listItemIdentifierExtractor))
            {
                continue;
            }

            var detailLeft = listLeft[i];
            var detailRight = listRight[i];

            await listItemCompareFunction(ctx, this, detailLeft, detailRight).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Processes comparison of lists including count comparison entry.
    /// In case of unequal list entries count further element comparisons will not take place.
    /// </summary>
    /// <typeparam name="TListElement">The list item type.</typeparam>
    /// <typeparam name="TContext">The context item type.</typeparam>
    /// <param name="ctx">The compare context.</param>
    /// <param name="listLeft">The list for left comparison.</param>
    /// <param name="listRight">The list for right comparison.</param>
    /// <param name="listItemIdentifierExtractor">The list item identifier extractor function.</param>
    /// <param name="listItemCompareFunction">The list item compare function to execute.</param>
    public void CompareList<TListElement, TContext>(
        TContext ctx,
        IReadOnlyList<TListElement> listLeft,
        IReadOnlyList<TListElement> listRight,
        Func<TListElement, string> listItemIdentifierExtractor,
        Action<TContext, ComparisonContainer, TListElement, TListElement> listItemCompareFunction)
    {
        AddEntry(listLeft.Count, listRight.Count);

        for (var i = 0; i < Math.Max(listLeft.Count, listRight.Count); i++)
        {
            if (!TryAddEqualIdentifierEntry(listLeft, listRight, i, listItemIdentifierExtractor))
            {
                continue;
            }

            var detailLeft = listLeft[i];
            var detailRight = listRight[i];

            listItemCompareFunction(ctx, this, detailLeft, detailRight);
        }
    }

    /// <summary>
    /// Adds a new <see cref="ComparisonContainer"/> to the current container's children list.
    /// </summary>
    /// <param name="descriptions">The new container's description.</param>
    /// <returns>The new created <see cref="ComparisonContainer"/>.</returns>
    public ComparisonContainer NewContainer(params string[] descriptions)
    {
        var container = new ComparisonContainer(Description, descriptions);
        _children.Add(container);
        return container;
    }

    /// <summary>
    /// Builds the container's description joining the parent description and further optional descriptions.
    /// </summary>
    /// <param name="parentDescription">The parent description.</param>
    /// <param name="descriptions">Further optional descriptions to join.</param>
    /// <returns>The container's full description.</returns>
    private static string BuildDescription(string parentDescription, IReadOnlyCollection<string> descriptions)
    {
        if (descriptions.Count == 0)
        {
            return parentDescription;
        }

        var description = string.Join(DescriptionSeparator, descriptions);
        return string.IsNullOrWhiteSpace(parentDescription)
            ? description
            : parentDescription + DescriptionHierarchySeparator + description;
    }

    private static string GetLastExpressionMemberName(string argumentExpression) =>
        argumentExpression[(argumentExpression.LastIndexOf('.') + 1)..];

    private static string GetListItemIdentifier<T>(IReadOnlyList<T> list, Func<T, string> listItemIdentifierExtractor, int index) =>
        index >= list.Count
            ? string.Empty
            : listItemIdentifierExtractor(list[index]);

    private bool TryAddEqualIdentifierEntry<T>(
        IReadOnlyList<T> listLeft,
        IReadOnlyList<T> listRight,
        int index,
        Func<T, string> listItemIdentifierExtractor)
    {
        var listItemIdentifierLeft = GetListItemIdentifier(listLeft, listItemIdentifierExtractor, index);
        var listItemIdentifierRight = GetListItemIdentifier(listRight, listItemIdentifierExtractor, index);

        var identifierEntry = new ComparisonEntry<string>(
            Description,
            $"ListIndex_{index}",
            listItemIdentifierLeft,
            listItemIdentifierRight);

        _entries.Add(identifierEntry);

        return identifierEntry.Equal;
    }
}
