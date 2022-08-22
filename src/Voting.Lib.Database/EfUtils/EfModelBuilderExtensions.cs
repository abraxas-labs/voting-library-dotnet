// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// EF Core Model Builder extensions.
/// </summary>
public static class EfModelBuilderExtensions
{
    /// <summary>
    /// Sets the column type of the property to "date".
    /// </summary>
    /// <param name="propertyBuilder">The property builder.</param>
    /// <typeparam name="TProperty">The property.</typeparam>
    /// <returns>The updated property builder.</returns>
    public static PropertyBuilder<TProperty> HasDateType<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
        => propertyBuilder.HasColumnType("date");

    /// <summary>
    /// Sets a utc conversion on a DateTime property.
    /// </summary>
    /// <param name="builder">The PropertyBuilder.</param>
    /// <returns>The updated property builder.</returns>
    public static PropertyBuilder<DateTime> HasUtcConversion(this PropertyBuilder<DateTime> builder)
    {
        return builder.HasConversion(d => d, d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
    }

    /// <summary>
    /// Sets a utc conversion on a nullable DateTime property.
    /// </summary>
    /// <param name="builder">The PropertyBuilder.</param>
    /// <returns>The updated property builder.</returns>
    public static PropertyBuilder<DateTime?> HasUtcConversion(this PropertyBuilder<DateTime?> builder)
    {
        return builder.HasConversion(d => d, d => DateTime.SpecifyKind(d!.Value, DateTimeKind.Utc));
    }
}
