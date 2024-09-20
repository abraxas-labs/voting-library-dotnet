// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.DocPipe.Serialization;

namespace Voting.Lib.DocPipe.Extensions;

internal static class DocPipeHttpClientExtensions
{
    public static async Task<TResponse?> PostDocPipe<TContent, TResponse>(
        this HttpClient client,
        string url,
        TContent content,
        CancellationToken ct = default)
        where TResponse : notnull
    {
        using var response = await client.PostAsJsonAsync(url, content, DocPipeJsonOptions.Instance, ct).ConfigureAwait(false);
        await response.EnsureSuccessStatusOrThrowDocPipeEx().ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<TResponse>(DocPipeJsonOptions.Instance, ct).ConfigureAwait(false);
    }
}
