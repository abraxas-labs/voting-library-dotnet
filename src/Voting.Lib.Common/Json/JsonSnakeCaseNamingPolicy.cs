// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text.Json;

namespace Voting.Lib.Common.Json;

/// <summary>
/// snake_case_naming_policy for System.Text.Json.
/// </summary>
public sealed class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
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
        => SnakeCaseConverter.ConvertToSnakeCase(name);
}
