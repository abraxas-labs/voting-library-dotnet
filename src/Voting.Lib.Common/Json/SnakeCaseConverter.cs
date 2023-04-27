// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Globalization;
using System.Linq;

namespace Voting.Lib.Common.Json;

internal static class SnakeCaseConverter
{
    public static string ConvertToSnakeCase(string s)
    {
        // Copied from https://gist.github.com/vkobel/d7302c0076c64c95ef4b
        return string.Concat(
                s.Select(
                    (x, i) => i > 0 && char.IsUpper(x)
                        ? "_" + x
                        : x.ToString(CultureInfo.InvariantCulture)))
            .ToLower(CultureInfo.InvariantCulture);
    }
}
