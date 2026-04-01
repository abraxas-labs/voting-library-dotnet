// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Converters;
using Voting.Lib.Database.Models;

namespace Voting.Lib.Database.Extensions;

/// <summary>
/// Extensions for <see cref="ModelConfigurationBuilder"/>.
/// </summary>
public static class ModelConfigurationBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="MarkdownString"/> type to be converted using <see cref="MarkdownStringConverter"/> and compared using <see cref="MarkdownStringComparer"/>.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder.</param>
    /// <returns>The updated configuration builder.</returns>
    public static ModelConfigurationBuilder ConfigureMarkdownString(this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<MarkdownString>()
            .HaveConversion<MarkdownStringConverter, MarkdownStringComparer>();

        return configurationBuilder;
    }
}
