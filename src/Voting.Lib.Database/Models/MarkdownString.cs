// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Voting.Lib.Common;

namespace Voting.Lib.Database.Models;

/// <summary>
/// Represents a string that contains markdown content.
/// The HTML representation is rendered lazily on first access and cached.
/// </summary>
public sealed class MarkdownString : IEquatable<MarkdownString>
{
    /// <summary>
    /// Empty markdown string.
    /// </summary>
    public static readonly MarkdownString Empty = new(string.Empty);

    private string? _html;
    private string? _plaintext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownString"/> class.
    /// </summary>
    /// <param name="markdown">The markdown content.</param>
    public MarkdownString(string markdown)
    {
        Markdown = markdown;
    }

    /// <summary>
    /// Gets the markdown content.
    /// </summary>
    public string Markdown { get; }

    /// <summary>
    /// Gets a value indicating whether the markdown content is empty.
    /// </summary>
    public bool IsEmpty => Markdown.Length == 0;

    /// <summary>
    /// Gets a value indicating whether the rendered plaintext content is empty or consists only of white-space characters.
    /// </summary>
    public bool IsEmptyOrWhiteSpace => string.IsNullOrWhiteSpace(Plaintext);

    /// <summary>
    /// Gets the rendered HTML content.
    /// The HTML is rendered lazily on first access and cached.
    /// </summary>
    public string Html => _html ??= MarkdownRenderer.RenderHtml(Markdown);

    /// <summary>
    /// Gets the rendered plaintext content.
    /// The plaintext is rendered lazily on first access and cached.
    /// </summary>
    public string Plaintext => _plaintext ??= MarkdownRenderer.RenderPlaintext(Markdown);

    /// <summary>
    /// Implicitly converts a <see cref="MarkdownString"/> to a string.
    /// </summary>
    /// <param name="markdownString">The markdown string to convert.</param>
    public static implicit operator string(MarkdownString markdownString) => markdownString.Markdown;

    /// <summary>
    /// Implicitly converts a string to a <see cref="MarkdownString"/>.
    /// </summary>
    /// <param name="markdown">The string to convert.</param>
    public static implicit operator MarkdownString(string markdown) => new(markdown);

    /// <summary>
    /// Determines whether two specified instances of <see cref="MarkdownString"/> are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns> <c>true</c> if left and right are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(MarkdownString? left, MarkdownString? right) => Equals(left, right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="MarkdownString"/> are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns> <c>true</c> if left and right are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(MarkdownString? left, MarkdownString? right) => !Equals(left, right);

    /// <inheritdoc />
    public override string ToString() => Markdown;

    /// <inheritdoc />
    public bool Equals(MarkdownString? other) => other is not null && string.Equals(Markdown, other.Markdown, StringComparison.Ordinal);

    /// <summary>
    /// Determines whether this instance and another <see cref="MarkdownString"/> have the same markdown content
    /// using the specified comparison rules.
    /// </summary>
    /// <param name="other">The other instance to compare.</param>
    /// <param name="comparisonType">The string comparison to use.</param>
    /// <returns><c>true</c> if the markdown content is equal; otherwise, <c>false</c>.</returns>
    public bool Equals(MarkdownString? other, StringComparison comparisonType) => other is not null && string.Equals(Markdown, other.Markdown, comparisonType);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MarkdownString other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Markdown);
}
