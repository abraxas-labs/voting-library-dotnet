// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Voting.Lib.DmDoc.Exceptions;
using Voting.Lib.DmDoc.Models.Internal;
using Voting.Lib.DmDoc.Serialization.Json;

namespace Voting.Lib.DmDoc.Extensions;

internal static class DmDocHttpResponseMessageExtensions
{
    /// <summary>
    /// Ensures the response has a status code which indicates success.
    /// If it doesn't indicate success, the response is tried to be parsed as dmDoc response,
    /// and its error description is throwd as <see cref="DmDocException"/>.
    /// </summary>
    /// <param name="response">The http response.</param>
    /// <returns>A <see cref="Task"/> representing the async operation.</returns>
    /// <exception cref="DmDocException">If the status doesn't indicate success.</exception>
    public static async Task EnsureSuccessStatusOrThrowDmDocEx(this HttpResponseMessage response)
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
                var resp = await JsonSerializer.DeserializeAsync<DmDocApiResponse>(
                    await response.Content.ReadAsStreamAsync().ConfigureAwait(false),
                    DmDocJsonOptions.Instance).ConfigureAwait(false);
                errorDescription = resp?.GetErrorDescription() ?? e.Message;
            }
            catch (Exception jsonReadEx)
            {
                errorDescription += " (could not read dmDoc error response: " + jsonReadEx.Message + ")";
            }

            throw new DmDocException(
                "documatrix call failed " + errorDescription,
                e);
        }
    }
}
