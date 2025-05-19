// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Diagnostics.CodeAnalysis;

namespace Voting.Lib.UserNotifications;

/// <summary>
/// A user notification.
/// </summary>
/// <param name="RecipientEmail">The recipient email.</param>
/// <param name="Subject">The subject.</param>
/// <param name="HtmlBody">The html body.</param>
public record UserNotification(
    string RecipientEmail,
    string Subject,
    [StringSyntax("html")] string HtmlBody);
