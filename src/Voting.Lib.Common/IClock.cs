// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Common;

/// <summary>
/// Clock abstraction.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets the current date/time in the utc time zone.
    /// </summary>
    DateTime UtcNow { get; }
}
