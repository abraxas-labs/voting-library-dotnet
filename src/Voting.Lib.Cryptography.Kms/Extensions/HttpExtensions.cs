// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Voting.Lib.Cryptography.Kms.ApiModels;
using Voting.Lib.Cryptography.Kms.Exceptions;

namespace Voting.Lib.Cryptography.Kms.Extensions;

internal static class HttpExtensions
{
    private static readonly JsonSerializerOptions KmsJsonOptions = new(JsonSerializerDefaults.Web);

    public static async Task<TResp> PostJson<TReq, TResp>(this HttpClient http, string requestUri, TReq request, JsonSerializerOptions? jsonOptions = null)
    {
        jsonOptions ??= KmsJsonOptions;
        var resp = await http.PostAsJsonAsync(
            requestUri,
            request,
            jsonOptions);
        return await resp.ReadResponse<TResp>(jsonOptions);
    }

    public static async Task<TResp> GetJson<TResp>(this HttpClient http, string requestUri)
    {
        var resp = await http.GetAsync(requestUri);
        return await resp.ReadResponse<TResp>();
    }

    public static async Task<TResp> ReadResponse<TResp>(this HttpResponseMessage resp, JsonSerializerOptions? jsonOptions = null)
    {
        await resp.EnsureSuccess();
        return await resp.Content.ReadFromJsonAsync<TResp>(jsonOptions ?? KmsJsonOptions)
               ?? throw new KmsException("Could not deserialize KMS response");
    }

    private static async Task EnsureSuccess(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        try
        {
            // error responses are always camelCase,
            // also for the auth endpoints, for which regular responses and requests are snake_case.
            var err = JsonSerializer.Deserialize<ErrorResponse>(responseContent, KmsJsonOptions);
            if (err == null)
            {
                throw new KmsException($"KMS returned an error: {responseContent}");
            }

            throw new KmsException(err.Code, err.CodeDesc, err.Message);
        }
        catch (Exception e) when (e is not KmsException)
        {
            throw new KmsException($"KMS returned an error: {responseContent}", e);
        }
    }
}
