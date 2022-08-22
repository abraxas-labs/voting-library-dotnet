// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;

namespace System.Threading;

/// <summary>
/// Extensions for <see cref="CancellationToken"/>.
/// </summary>
public static class CancellationTokenExtensions
{
    /// <summary>
    /// Returns a task which waits for the cancellation token to fire.
    /// Does not throw task canceled exceptions since they are expected here.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task which completes when the cancellation token is cancelled.</returns>
    public static async Task WaitForCancellation(this CancellationToken ct)
    {
        try
        {
            await Task.Delay(Timeout.Infinite, ct).ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            // ignored as this is expected here (this method is only used to wait for the cancellation)
            // see also method comment
        }
    }
}
