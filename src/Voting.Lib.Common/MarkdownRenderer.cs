// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using Ganss.Xss;
using Markdig;
using Markdig.Extensions.EmphasisExtras;

namespace Voting.Lib.Common;

/// <summary>
/// A markdown renderer that uses Markdig for parsing and HtmlSanitizer for output sanitization.
/// Only the following markup is allowed in the output:
/// <c>&lt;strong&gt;</c> (bold), <c>&lt;em&gt;</c> (italic), <c>&lt;del&gt;</c> (strikethrough),
/// <c>&lt;br&gt;</c> (line break), <c>&lt;p&gt;</c> (paragraphs), <c>&lt;ol&gt;</c>/<c>&lt;ul&gt;</c>/<c>&lt;li&gt;</c> (lists), and <c>&lt;sup&gt;</c> (superscript).
/// Disallowed raw HTML is encoded rather than stripped.
/// </summary>
public static class MarkdownRenderer
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseEmphasisExtras(EmphasisExtraOptions.Strikethrough)
        .Build();

    private static readonly HtmlSanitizer Sanitizer = CreateSanitizer();

    /// <summary>
    /// Renders the supported markdown subset to sanitized HTML.
    /// Disallowed raw HTML in the input is encoded (visible as text), not stripped.
    /// </summary>
    /// <param name="markdown">The markdown input string.</param>
    /// <returns>The rendered and sanitized HTML string.</returns>
    public static string RenderHtml(string? markdown)
    {
        if (string.IsNullOrEmpty(markdown))
        {
            return string.Empty;
        }

        var html = Markdown.ToHtml(markdown, Pipeline);
        return Sanitizer.Sanitize(html);
    }

    /// <summary>
    /// Renders the supported markdown subset to plain text.
    /// </summary>
    /// <param name="markdown">The markdown input string.</param>
    /// <returns>The rendered plain text string.</returns>
    public static string RenderPlaintext(string? markdown)
    {
        if (string.IsNullOrEmpty(markdown))
        {
            return string.Empty;
        }

        return Markdown.ToPlainText(markdown, Pipeline).TrimEnd('\r', '\n');
    }

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedTags.UnionWith(new HashSet<string>
        {
            "strong",
            "em",
            "del",
            "br",
            "sup",
            "p",
            "ol",
            "ul",
            "li",
        });
        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedCssProperties.Clear();
        sanitizer.AllowedSchemes.Clear();

        // don't just remove unknown tags, encode them instead.
        sanitizer.RemovingTag += (_, e) =>
        {
            e.Cancel = true;
            e.Tag.OuterHtml = WebUtility.HtmlEncode(e.Tag.OuterHtml);
        };

        return sanitizer;
    }
}
