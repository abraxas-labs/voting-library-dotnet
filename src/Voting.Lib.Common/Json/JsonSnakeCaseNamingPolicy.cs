// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace Voting.Lib.Common.Json;

/// <summary>
/// snake_case_naming_policy for System.Text.Json.
/// </summary>
public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static readonly JsonSnakeCaseNamingPolicy Instance = new();

    private JsonSnakeCaseNamingPolicy()
    {
    }

    /// <inheritdoc />
    public override string ConvertName(string name)
    {
        // Copied from https://gist.github.com/vkobel/d7302c0076c64c95ef4b
        return string.Concat(
                name.Select(
                    (x, i) => i > 0 && char.IsUpper(x)
                        ? "_" + x
                        : x.ToString(CultureInfo.InvariantCulture)))
            .ToLower(CultureInfo.InvariantCulture);
    }
}
