// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Voting.Lib.DocPipe.Exceptions;
using Voting.Lib.DocPipe.Models;
using Voting.Lib.DocPipe.Serialization;

namespace Voting.Lib.DocPipe.Extensions;

internal static class DocPipeHttpResponseMessageExtensions
{
    /// <summary>
    /// Ensures the response has a status code which indicates success.
    /// If it doesn't indicate success, the response is tried to be parsed as DocPipe response,
    /// and its error description is thrown as <see cref="DocPipeException"/>.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <returns>A <see cref="Task"/> representing the async operation.</returns>
    /// <exception cref="DocPipeException">If the status doesn't indicate success.</exception>
    public static async Task EnsureSuccessStatusOrThrowDocPipeEx(this HttpResponseMessage response)
    {
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            var errorDescription = e.Message;
            try
            {
                var errorResponse = await JsonSerializer.DeserializeAsync<DocPipeErrorResponse>(
                    await response.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    DocPipeJsonOptions.Instance).ConfigureAwait(false);
                errorDescription = errorResponse?.GetErrorDescription() ?? e.Message;
            }
            catch (Exception jsonReadEx)
            {
                errorDescription += " (could not read DocPipe error response: " + jsonReadEx.Message + ")";
            }

            throw new DocPipeException("DocPipe call failed " + errorDescription, e);
        }
    }
}
