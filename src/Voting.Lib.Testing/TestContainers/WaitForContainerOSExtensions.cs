// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using DotNet.Testcontainers.Configurations;

namespace Voting.Lib.Testing.TestContainers;

/// <summary>
/// Extensions for <see cref="IWaitForContainerOS"/>.
/// </summary>
public static class WaitForContainerOsExtensions
{
    /// <summary>
    /// Adds a waiter which waits until a curl command to a container port and path succeeds.
    /// </summary>
    /// <param name="wait">The wait builder.</param>
    /// <param name="port">The container port.</param>
    /// <param name="path">The path of the health check (eg. /health/live).</param>
    /// <returns>The updated wait builder.</returns>
    public static IWaitForContainerOS UntilCurlHttpOk(this IWaitForContainerOS wait, int port, string path)
    {
        var curlCommand = "curl "
            + "--connect-timeout 30 "
            + "--max-time 35 "
            + "--retry 20 "
            + "--retry-connrefused "
            + "--retry-delay 1 "
            + "--fail "
            + "--insecure "
            + $"http://localhost:{port}{path}";

        return wait.UntilCommandIsCompleted(curlCommand);
    }
}
