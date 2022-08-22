// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using Voting.Lib.Common;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// A helper class for language related things.
/// </summary>
public static class LanguageUtil
{
    /// <summary>
    /// Mocks content for all languages.
    /// </summary>
    /// <param name="content">The base content to use.</param>
    /// <returns>A dictionary containing content for all languages.</returns>
    public static Dictionary<string, string> MockAllLanguages(string content)
    {
        return Languages.All.ToDictionary(l => l, l => $"{content} {l}");
    }
}
