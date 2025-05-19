// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace System;

/// <summary>
/// <see cref="TimeProvider"/> extensions.
/// </summary>
public static class TimeProviderExtensions
{
    /// <summary>
    /// Returns the current <see cref="DateTime"/> in UTC.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTime GetUtcNowDateTime(this TimeProvider timeProvider)
        => timeProvider.GetUtcNow().UtcDateTime;

    /// <summary>
    /// Returns the current <see cref="DateTime"/> in local time.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTime GetLocalNowDateTime(this TimeProvider timeProvider)
        => timeProvider.GetLocalNow().LocalDateTime;

    /// <summary>
    /// Returns the current date in UTC.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTime GetUtcToday(this TimeProvider timeProvider)
        => timeProvider.GetUtcNow().Date;

    /// <summary>
    /// Returns the current date in local time.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTime GetLocalToday(this TimeProvider timeProvider)
        => timeProvider.GetLocalNow().Date;
}
