// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class MarkdownRendererTest
{
    [Theory]
    [InlineData(null, "", "")]
    [InlineData("", "", "")]
    public void ShouldReturnEmptyForNullOrEmpty(string? input, string expectedHtml, string expectedPlaintext)
    {
        MarkdownRenderer.RenderHtml(input).Should().Be(expectedHtml);
        MarkdownRenderer.RenderPlaintext(input).Should().Be(expectedPlaintext);
    }

    [Theory]
    [InlineData("**bold**", "<p><strong>bold</strong></p>\n", "bold")]
    [InlineData("*italic*", "<p><em>italic</em></p>\n", "italic")]
    [InlineData("~~strikethrough~~", "<p><del>strikethrough</del></p>\n", "strikethrough")]
    [InlineData("**bold *and italic* bold**", "<p><strong>bold <em>and italic</em> bold</strong></p>\n", "bold and italic bold")]
    [InlineData("**bold** and *italic* and ~~strike~~", "<p><strong>bold</strong> and <em>italic</em> and <del>strike</del></p>\n", "bold and italic and strike")]
    [InlineData("x<sup>2</sup>", "<p>x<sup>2</sup></p>\n", "x2")]
    public void ShouldRenderMarkdownFormatting(string input, string expectedHtml, string expectedPlaintext)
    {
        MarkdownRenderer.RenderHtml(input).Should().Be(expectedHtml);
        MarkdownRenderer.RenderPlaintext(input).Should().Be(expectedPlaintext);
    }

    [Theory]
    [InlineData("x<sup>2</sup>", "<p>x<sup>2</sup></p>\n")]
    [InlineData("line1<br />line2", "<p>line1<br>line2</p>\n")]
    [InlineData("a<br/>b", "<p>a<br>b</p>\n")]
    [InlineData("a<br>b", "<p>a<br>b</p>\n")]
    [InlineData("<ul><li>first</li><li>second</li></ul>", "<ul><li>first</li><li>second</li></ul>\n")]
    [InlineData("<ol><li>first</li><li>second</li></ol>", "<ol><li>first</li><li>second</li></ol>\n")]
    public void ShouldRenderAllowedHtmlTags(string input, string expected)
    {
        MarkdownRenderer.RenderHtml(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("- first\n- second", "<ul>\n<li>first</li>\n<li>second</li>\n</ul>\n")]
    [InlineData("1. first\n2. second", "<ol>\n<li>first</li>\n<li>second</li>\n</ol>\n")]
    [InlineData("2. second\n3. third", "<ol>\n<li>second</li>\n<li>third</li>\n</ol>\n")]
    public void ShouldRenderMarkdownLists(string input, string expected)
    {
        MarkdownRenderer.RenderHtml(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("just plain text", "<p>just plain text</p>\n", "just plain text")]
    [InlineData("a~b", "<p>a~b</p>\n", "a~b")]
    public void ShouldPassThroughPlainText(string input, string expectedHtml, string expectedPlaintext)
    {
        MarkdownRenderer.RenderHtml(input).Should().Be(expectedHtml);
        MarkdownRenderer.RenderPlaintext(input).Should().Be(expectedPlaintext);
    }

    [Theory]
    [InlineData("<script>alert('xss')</script>", "&lt;script&gt;alert('xss')&lt;/script&gt;\n")]
    [InlineData("![alt](http://example.com/img.png)", "<p>&lt;img src=\"http://example.com/img.png\" alt=\"alt\"&gt;</p>\n")]
    [InlineData("[click me](http://example.com)", "<p>&lt;a href=\"http://example.com\"&gt;click me&lt;/a&gt;</p>\n")]
    public void ShouldEncodeOrStripDisallowedHtml(string input, string expected)
    {
        MarkdownRenderer.RenderHtml(input).Should().Be(expected);
    }
}
