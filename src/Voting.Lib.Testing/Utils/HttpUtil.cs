// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// A helper class for http related things.
/// </summary>
public static class HttpUtil
{
    private const string FormFieldNameFile = "file";
    private const string FormFieldNameData = "data";

    /// <summary>
    /// Prepares the multipart form data content for a http request with file and data.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request data.</typeparam>
    /// <param name="filePath">Path of the file.</param>
    /// <param name="request">The request.</param>
    /// <param name="processor">The processor of the http operation.</param>
    /// <returns>A http response message.</returns>
    public static async Task<HttpResponseMessage> RequestWithFileAndData<TRequest>(string? filePath, TRequest? request, Func<MultipartFormDataContent, Task<HttpResponseMessage>> processor)
    {
        StreamContent? fileContent = null;
        StringContent? dataContent = null;

        if (filePath != null)
        {
            fileContent = new StreamContent(File.OpenRead(filePath));
            fileContent.Headers.Add("Content-Type", MediaTypeNames.Text.Xml);
        }

        if (request != null)
        {
            dataContent = new StringContent(JsonSerializer.Serialize(request, request.GetType()));
        }

        try
        {
            using var content = new MultipartFormDataContent("boundary-f7611587-0a7b-4eef-994b-31f9c2231cd8");

            if (dataContent != null)
            {
                content.Add(dataContent, FormFieldNameData);
            }

            if (filePath != null)
            {
                content.Add(fileContent!, FormFieldNameFile, Path.GetFileName(filePath));
            }

            return await processor(content);
        }
        finally
        {
            dataContent?.Dispose();
            fileContent?.Dispose();
        }
    }
}
