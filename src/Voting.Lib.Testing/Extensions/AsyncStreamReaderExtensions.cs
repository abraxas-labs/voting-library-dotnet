// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Grpc.Core;

/// <summary>
/// Extensions for the <see cref="IAsyncStreamReader{T}"/>.
/// </summary>
public static class AsyncStreamReaderExtensions
{
    /// <summary>
    /// Reads N responses and ignores cancellation exceptions.
    /// </summary>
    /// <param name="responseStream">The response stream to read.</param>
    /// <param name="responseCount">The count of responses to read.</param>
    /// <param name="ct">The cancellation token to use.</param>
    /// <typeparam name="T">The type of responses.</typeparam>
    /// <returns>Returns a list of responses, which should contain N responses or fewer if a cancellation occured.</returns>
    public static async Task<List<T>> ReadNIgnoreCancellation<T>(
        this IAsyncStreamReader<T> responseStream,
        int responseCount,
        CancellationToken ct)
    {
        var responses = new List<T>();

        try
        {
            await foreach (var resp in responseStream.ReadAllAsync(ct))
            {
                responses.Add(resp);

                if (responseCount == responses.Count)
                {
                    return responses;
                }
            }
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.Cancelled)
        {
            // we ignore cancellation of the request as stated in the method name
        }

        return responses;
    }
}
