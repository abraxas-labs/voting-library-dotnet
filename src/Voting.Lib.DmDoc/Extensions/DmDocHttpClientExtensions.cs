// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.DmDoc.Exceptions;
using Voting.Lib.DmDoc.Models.Internal;
using Voting.Lib.DmDoc.Serialization.Json;

namespace Voting.Lib.DmDoc.Extensions;

internal static class DmDocHttpClientExtensions
{
    public static async Task<T> GetDmDoc<T>(this HttpClient client, string url, CancellationToken ct = default)
        where T : notnull
    {
        try
        {
            var response = await client.GetFromJsonAsync<DmDocDataApiResponse<T>>(url, DmDocJsonOptions.Instance, ct).ConfigureAwait(false);
            return response.EnsureSuccessAndUnwrap();
        }
        catch (HttpRequestException ex)
        {
            throw new DmDocException(ex);
        }
    }

    public static async Task<TResponse> PostDmDoc<TContent, TResponse>(
        this HttpClient client,
        string url,
        TContent content,
        CancellationToken ct = default)
        where TResponse : notnull
    {
        using var stringContent = BuildDmDocContent(content);

        using var response = await client.PostAsync(url, stringContent, ct).ConfigureAwait(false);
        await response.EnsureSuccessStatusOrThrowDmDocEx().ConfigureAwait(false);

        var responseContent = await response.Content.ReadFromJsonAsync<DmDocDataApiResponse<TResponse>>(DmDocJsonOptions.Instance, ct).ConfigureAwait(false);
        return responseContent.EnsureSuccessAndUnwrap();
    }

    public static async Task<TResponse> PutDmDoc<TResponse>(
        this HttpClient client,
        string url,
        CancellationToken ct = default)
        where TResponse : DmDocApiResponse
    {
        using var response = await client.PutAsync(url, null, ct).ConfigureAwait(false);
        await response.EnsureSuccessStatusOrThrowDmDocEx().ConfigureAwait(false);

        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>(DmDocJsonOptions.Instance, ct).ConfigureAwait(false);
        responseContent.EnsureSuccess();
        return responseContent!;
    }

    public static async Task<TResponse> PutDmDoc<TContent, TResponse>(
        this HttpClient client,
        string url,
        TContent content,
        CancellationToken ct = default)
        where TResponse : notnull
    {
        using var stringContent = BuildDmDocContent(content);

        using var response = await client.PutAsync(url, stringContent, ct).ConfigureAwait(false);
        await response.EnsureSuccessStatusOrThrowDmDocEx().ConfigureAwait(false);

        var responseContent = await response.Content.ReadFromJsonAsync<DmDocDataApiResponse<TResponse>>(DmDocJsonOptions.Instance, ct).ConfigureAwait(false);
        return responseContent.EnsureSuccessAndUnwrap();
    }

    private static HttpContent BuildDmDocContent<TContent>(TContent content)
    {
        // set length explicitly to disable chunked transfer encoding
        // (dm doc doesnt support chunked transfer encoding)
        // and the dotnet json extension methods only support chunked encoding.
        var jsonContent = JsonSerializer.Serialize(content, DmDocJsonOptions.Instance);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

        stringContent.Headers.ContentLength = jsonContent.Length;
        return stringContent;
    }
}
