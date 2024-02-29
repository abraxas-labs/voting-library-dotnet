// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Reflection;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// Utility class for .NET projects.
/// </summary>
public static class ProjectUtil
{
    private const string SolutionFileSearchPattern = "*.sln";

    /// <summary>
    /// Finds the directory where the solution file for the executing project resides.
    /// </summary>
    /// <returns>The path to the solution directory.</returns>
    /// <exception cref="InvalidOperationException">In case the solution directory is not found.</exception>
    public static string FindSolutionDirectory()
    {
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new InvalidOperationException();

        do
        {
            if (Directory.GetFiles(dir, SolutionFileSearchPattern, SearchOption.TopDirectoryOnly).Length > 0)
            {
                return dir;
            }

            dir = Path.GetDirectoryName(dir);
        }
        while (dir != null);

        throw new InvalidOperationException();
    }
}
