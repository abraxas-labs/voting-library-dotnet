// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Voting.Lib.Database.Models;
using Xunit;

namespace Voting.Lib.Database.Test.Models;

public class MarkdownStringTest
{
    [Fact]
    public void ConstructorShouldSetMarkdown()
    {
        var md = new MarkdownString("hello");
        md.Markdown.Should().Be("hello");
    }

    [Fact]
    public void EmptyShouldHaveEmptyMarkdown()
    {
        MarkdownString.Empty.Markdown.Should().BeEmpty();
    }

    [Fact]
    public void HtmlShouldRenderMarkdown()
    {
        var md = new MarkdownString("**bold**");
        md.Html.Should().Contain("<strong>bold</strong>");
    }

    [Fact]
    public void HtmlShouldBeCached()
    {
        var md = new MarkdownString("**bold**");
        var html1 = md.Html;
        var html2 = md.Html;
        html1.Should().BeSameAs(html2);
    }

    [Fact]
    public void HtmlShouldReturnEmptyForEmptyMarkdown()
    {
        MarkdownString.Empty.Html.Should().BeEmpty();
    }

    [Fact]
    public void PlaintextShouldRenderMarkdown()
    {
        var md = new MarkdownString("**bold**");
        md.Plaintext.Should().Be("bold");
    }

    [Fact]
    public void PlaintextShouldBeCached()
    {
        var md = new MarkdownString("**bold**");
        var plaintext1 = md.Plaintext;
        var plaintext2 = md.Plaintext;
        plaintext1.Should().BeSameAs(plaintext2);
    }

    [Fact]
    public void PlaintextShouldReturnEmptyForEmptyMarkdown()
    {
        MarkdownString.Empty.Plaintext.Should().BeEmpty();
    }

    [Fact]
    public void IsEmptyShouldReturnTrueForEmptyString()
    {
        var md = new MarkdownString(string.Empty);
        md.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsEmptyShouldReturnFalseForWhitespace()
    {
        var md = new MarkdownString("   ");
        md.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void IsEmptyShouldReturnFalseForContent()
    {
        var md = new MarkdownString("hello");
        md.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void IsEmptyOrWhiteSpaceShouldReturnTrueForEmptyString()
    {
        var md = new MarkdownString(string.Empty);
        md.IsEmptyOrWhiteSpace.Should().BeTrue();
    }

    [Fact]
    public void IsEmptyOrWhiteSpaceShouldReturnTrueForWhitespace()
    {
        var md = new MarkdownString("   ");
        md.IsEmptyOrWhiteSpace.Should().BeTrue();
    }

    [Fact]
    public void IsEmptyOrWhiteSpaceShouldReturnTrueForFormattingOnlyWhitespace()
    {
        var md = new MarkdownString("** **");
        md.IsEmptyOrWhiteSpace.Should().BeTrue();
    }

    [Fact]
    public void IsEmptyOrWhiteSpaceShouldReturnFalseForContent()
    {
        var md = new MarkdownString("**bold**");
        md.IsEmptyOrWhiteSpace.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversionToStringShouldReturnMarkdown()
    {
        var md = new MarkdownString("hello");
        string result = md;
        result.Should().Be("hello");
    }

    [Fact]
    public void ImplicitConversionFromStringShouldCreateMarkdownString()
    {
        MarkdownString md = "hello";
        md.Markdown.Should().Be("hello");
    }

    [Fact]
    public void ToStringShouldReturnMarkdown()
    {
        var md = new MarkdownString("hello");
        md.ToString().Should().Be("hello");
    }

    [Fact]
    public void EqualsShouldReturnTrueForSameMarkdown()
    {
        var a = new MarkdownString("hello");
        var b = new MarkdownString("hello");
        a.Equals(b).Should().BeTrue();
    }

    [Fact]
    public void EqualsShouldReturnFalseForDifferentMarkdown()
    {
        var a = new MarkdownString("hello");
        var b = new MarkdownString("world");
        a.Equals(b).Should().BeFalse();
    }

    [Fact]
    public void EqualsShouldReturnFalseForNull()
    {
        var a = new MarkdownString("hello");
        a.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void EqualsWithComparisonTypeShouldWork()
    {
        var a = new MarkdownString("Hello");
        var b = new MarkdownString("hello");
        a.Equals(b).Should().BeFalse();
        a.Equals(b, StringComparison.OrdinalIgnoreCase).Should().BeTrue();
    }

    [Fact]
    public void EqualsWithComparisonTypeShouldReturnFalseForNull()
    {
        var a = new MarkdownString("hello");
        a.Equals(null, StringComparison.OrdinalIgnoreCase).Should().BeFalse();
    }

    [Fact]
    public void EqualsObjectShouldReturnTrueForEqualMarkdownString()
    {
        var a = new MarkdownString("hello");
        object b = new MarkdownString("hello");
        a.Equals(b).Should().BeTrue();
    }

    [Fact]
    public void EqualsObjectShouldReturnFalseForNonMarkdownString()
    {
        var a = new MarkdownString("hello");
        a.Equals((object)42).Should().BeFalse();
    }

    [Fact]
    public void EqualityOperatorShouldWork()
    {
        var a = new MarkdownString("hello");
        var b = new MarkdownString("hello");
        (a == b).Should().BeTrue();
    }

    [Fact]
    public void InequalityOperatorShouldWork()
    {
        var a = new MarkdownString("hello");
        var b = new MarkdownString("world");
        (a != b).Should().BeTrue();
    }

    [Fact]
    public void EqualityOperatorShouldHandleNulls()
    {
        MarkdownString? a = null;
        MarkdownString? b = null;
        (a == b).Should().BeTrue();
        (a == new MarkdownString("hello")).Should().BeFalse();
        (new MarkdownString("hello") == b).Should().BeFalse();
    }

    [Fact]
    public void GetHashCodeShouldBeEqualForEqualInstances()
    {
        var a = new MarkdownString("hello");
        var b = new MarkdownString("hello");
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void GetHashCodeShouldDifferForDifferentInstances()
    {
        var a = new MarkdownString("hello");
        var b = new MarkdownString("world");
        a.GetHashCode().Should().NotBe(b.GetHashCode());
    }
}
