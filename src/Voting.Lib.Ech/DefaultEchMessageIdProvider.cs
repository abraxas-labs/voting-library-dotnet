// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Ech;

/// <inheritdoc />
public class DefaultEchMessageIdProvider : IEchMessageIdProvider
{
    /// <inheritdoc />
    public string NewId()
        => Guid.NewGuid().ToString("N");
}
