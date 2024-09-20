// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections;

namespace Voting.Lib.Common;

/// <summary>
/// Fixes environment variables that contain a period.
/// </summary>
public static class EnvironmentVariablesFixer
{
    private const string EnvDotPlaceholder = "_DOT_";
    private const string EnvDotReplacement = ".";

    /// <summary>
    /// Copies environment variables with _DOT_ by replacing _DOT_ with .
    /// For example this is needed to configure logging levels on *nix systems (including gitlab ci).
    /// Workaround for https://github.com/dotnet/runtime/issues/35989.
    /// </summary>
    public static void FixDotEnvironmentVariables()
    {
        foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
        {
            var key = environmentVariable.Key.ToString();
            var value = environmentVariable.Value?.ToString();
            if (key == null || value == null || !key.Contains(EnvDotPlaceholder))
            {
                continue;
            }

            var newKey = key.Replace(EnvDotPlaceholder, EnvDotReplacement, StringComparison.Ordinal);
            if (Environment.GetEnvironmentVariable(newKey) == null)
            {
                Environment.SetEnvironmentVariable(newKey, value);
            }
        }
    }
}
