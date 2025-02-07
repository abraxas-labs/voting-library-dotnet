// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Time.Testing;
using Voting.Lib.Common;

namespace Voting.Lib.Testing.Mocks;

/// <summary>
/// Provides a fixed timestamp clock implementation.
/// </summary>
public class MockedClock : TimeProvider, IClock
{
    /// <summary>
    /// Gets a fixed timestamp of 2020-01-10 13:12:10.200 UTC.
    /// </summary>
    public static DateTime UtcNowDate { get; } = new(2020, 1, 10, 13, 12, 10, 200, DateTimeKind.Utc);

    /// <summary>
    /// Gets a fixed date only value of 2020-01-10.
    /// </summary>
    public static DateOnly NowDateOnly { get; } = new(2020, 1, 10);

    /// <summary>
    /// Gets a fixed timestamp of 2020-01-10 13:12:10.200 UTC.
    /// </summary>
    public static DateTimeOffset UtcNowOffset { get; } = new(UtcNowDate);

    /// <summary>
    /// Gets a fixed timestamp of 2020-01-10 13:12:10.200 UTC.
    /// </summary>
    public static Timestamp UtcNowTimestamp { get; } = UtcNowDate.ToTimestamp();

    /// <summary>
    /// Gets or sets a fixed timestamp of 2020-01-10 13:12:10.200 UTC or an overwritten value.
    /// </summary>
    public DateTime UtcNow { get; set; } = UtcNowDate;

    /// <summary>
    /// Gets or sets a fixed date only value of 2020-01-10 or an overwritten value.
    /// </summary>
    public DateOnly Today { get; set; } = NowDateOnly;

    /// <summary>
    /// Returns a fixed timestamp of 2020-01-10 13:12:10.200 UTC modified by the provided delta.
    /// </summary>
    /// <param name="dayDelta">Days to add.</param>
    /// <param name="hoursDelta">Hours to add.</param>
    /// <returns>The timestamp.</returns>
    public static DateTime GetDate(int dayDelta = 0, int hoursDelta = 0)
        => UtcNowDate.AddDays(dayDelta).AddHours(hoursDelta);

    /// <summary>
    /// Returns a fixed timestamp of 2020-01-10 13:12:10.200 UTC modified by the provided delta.
    /// </summary>
    /// <param name="dayDelta">Days to add.</param>
    /// <param name="hoursDelta">Hours to add.</param>
    /// <returns>The timestamp.</returns>
    public static Timestamp GetTimestamp(int dayDelta = 0, int hoursDelta = 0)
        => GetDate(dayDelta, hoursDelta).ToTimestamp();

    /// <summary>
    /// Returns a fixed date of 2020-01-10 UTC modified by the provided delta.
    /// </summary>
    /// <param name="dayDelta">Days to add.</param>
    /// <returns>The timestamp.</returns>
    public static Timestamp GetTimestampDate(int dayDelta = 0)
        => GetDate(dayDelta).Date.ToTimestamp();

    /// <summary>
    /// Creates a <see cref="FakeTimeProvider"/> instance.
    /// The default timestamp is set to <see cref="UtcNowDate"/>,
    /// the timezone to zurich.
    /// </summary>
    /// <returns>The created instance.</returns>
    public static FakeTimeProvider CreateFakeTimeProvider()
    {
        var mockedClock = new FakeTimeProvider(UtcNowOffset);
        mockedClock.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Europe/Zurich"));
        return mockedClock;
    }

    /// <summary>
    /// Gets a fixed timestamp of 2020-01-10 13:12:10.200 UTC or an overwritten value.
    /// </summary>
    /// <returns>The fixed timestamp.</returns>
    public override DateTimeOffset GetUtcNow() => UtcNow;
}
