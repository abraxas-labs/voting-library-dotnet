// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Rest;

/// <summary>
/// Well known abraxas http header names.
/// </summary>
public static class AbraxasHeaderNames
{
    /// <summary>
    /// The app header name.
    /// The header contains a secure connect app shortcut.
    /// </summary>
    public const string App = "x-app";

    /// <summary>
    /// The tenant header name.
    /// The header contains a secure connect tenant id.
    /// </summary>
    public const string Tenant = "x-tenant";

    /// <summary>
    /// The language header name.
    /// The header contains an i18n iso code.
    /// </summary>
    public const string Language = "x-language";

    /// <summary>
    /// A list of all well known abraxas http header names.
    /// </summary>
    public static readonly IReadOnlyCollection<string> All = new[]
    {
        App,
        Tenant,
        Language,
    };
}
