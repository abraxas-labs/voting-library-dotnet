// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Voting.Lib.Common;
using Voting.Lib.Rest.Files;

namespace Voting.Lib.Rest.Utils;

// parts copied from https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0#upload-large-files-with-streaming

/// <summary>
/// Multipart request helper provides methods to deserialize
/// file and form-data provided in a multipart request.
/// </summary>
public class MultipartRequestHelper
{
    private const int ContentTypeLenghtLimit = 2048;

    private const string FormFieldNameFile = "file";
    private const string FormFieldNameData = "data";

    private const string MultipartContentTypePrefix = "multipart/";
    private const string DispositionTypeFormData = "form-data";

    private readonly ILogger<MultipartRequestHelper> _logger;
    private readonly IOptions<JsonOptions> _jsonOptions;
    private readonly AttributeValidator _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultipartRequestHelper"/> class.
    /// </summary>
    /// <param name="logger">A logger.</param>
    /// <param name="jsonOptions">The json options how to deserialize the json object in the form-data content.</param>
    /// <param name="validator">The validator which is used to validate the request object.</param>
    public MultipartRequestHelper(
        ILogger<MultipartRequestHelper> logger,
        IOptions<JsonOptions> jsonOptions,
        AttributeValidator validator)
    {
        _logger = logger;
        _jsonOptions = jsonOptions;
        _validator = validator;
    }

    /// <summary>
    /// Reads a multipart HTTP request with one file section. The section needs to be named like <see cref="FormFieldNameFile"/>.
    /// </summary>
    /// <param name="request">The HTTP request to read data from.</param>
    /// <param name="processRequest">The function to process the request content.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>The read data.</returns>
    /// <exception cref="ValidationException">If the request format or section does not match the expected format or sections.</exception>
    public async Task<TResult> ReadFile<TResult>(
        HttpRequest request,
        Func<MultipartFile, Task<TResult>> processRequest)
    {
        await foreach (var section in ReadSections(request))
        {
            var multipartFile = ReadMultipartFile(section);
            if (multipartFile == null || !FormFieldNameFile.Equals(multipartFile.FormFieldName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            return await processRequest(multipartFile).ConfigureAwait(false);
        }

        throw new ValidationException($"No file uploaded. Use a multipart request with a section named {FormFieldNameFile}.");
    }

    /// <summary>
    /// Reads a multipart HTTP request with multiple file sections.
    /// </summary>
    /// <param name="request">The HTTP request to read data from.</param>
    /// <param name="processFile">The function to process the file.</param>
    /// <returns>The read data.</returns>
    /// <exception cref="ValidationException">If the request format or section does not match the expected format or sections.</exception>
    public async Task ReadFiles(
        HttpRequest request,
        Func<MultipartFile, Task> processFile)
    {
        await foreach (var section in ReadSections(request))
        {
            var multipartFile = ReadMultipartFile(section);
            if (multipartFile == null)
            {
                continue;
            }

            await processFile(multipartFile).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Reads a multipart HTTP request with two sections. One with a file and one named data with the json encoded data.
    /// </summary>
    /// <param name="request">The HTTP request to read data from.</param>
    /// <param name="processRequest">The function to process the request content.</param>
    /// <param name="processRequestWithoutFile">The function to process the request data if no file was provided.</param>
    /// <typeparam name="TData">The type of the json encoded request data.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>The read data.</returns>
    /// <exception cref="ValidationException">If the request format or section does not match the expected format or sections.</exception>
    public async Task<TResult> ReadFileAndData<TData, TResult>(
        HttpRequest request,
        Func<MultipartData<TData>, Task<TResult>> processRequest,
        Func<TData, Task<TResult>>? processRequestWithoutFile = null)
    {
        var data = new MultipartRequestData<TData>();
        await foreach (var section in ReadSections(request))
        {
            var fileAndRequestData = await ProcessSection(section, data).ConfigureAwait(false);
            if (fileAndRequestData != null)
            {
                return await processRequest(fileAndRequestData).ConfigureAwait(false);
            }
        }

        if (processRequestWithoutFile == null)
        {
            throw new ValidationException($"No file uploaded. Use a multipart request with a section named {FormFieldNameFile}.");
        }

        if (data.RequestData == null)
        {
            throw new ValidationException($"no request data provided. Use a multipart request with a section named {FormFieldNameData} with json encoded content");
        }

        return await processRequestWithoutFile(data.RequestData).ConfigureAwait(false);
    }

    private static async IAsyncEnumerable<MultipartSection> ReadSections(HttpRequest request)
    {
        if (!IsMultipartContentType(request.ContentType))
        {
            throw new ValidationException("Not a multipart request");
        }

        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType), ContentTypeLenghtLimit);
        var reader = new MultipartReader(boundary, request.Body);
        while (await reader.ReadNextSectionAsync().ConfigureAwait(false) is { } section)
        {
            yield return section;
        }
    }

    private static MultipartFile? ReadMultipartFile(MultipartSection section)
    {
        if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
        {
            throw new ValidationException("encountered request section without content disposition");
        }

        var sectionName = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value!;
        if (!HasFileContentDisposition(contentDisposition))
        {
            return null;
        }

        var fileName = ReadFileName(contentDisposition);
        return new MultipartFile(fileName, section.Body, sectionName);
    }

    private static string ReadFileName(ContentDispositionHeaderValue contentDisposition)
    {
        var fileName = contentDisposition.FileNameStar.ToString();
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = contentDisposition.FileName.ToString();
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ValidationException("No filename defined.");
        }

        return fileName;
    }

    // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
    // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
    private static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        if (boundary.Length > lengthLimit)
        {
            throw new InvalidDataException(
                $"Multipart boundary length limit {lengthLimit} exceeded.");
        }

        return boundary;
    }

    private static bool IsMultipartContentType(string? contentType)
    {
        return !string.IsNullOrEmpty(contentType)
               && contentType.Contains(MultipartContentTypePrefix, StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
    {
        return contentDisposition.DispositionType.Equals(DispositionTypeFormData)
               && string.IsNullOrEmpty(contentDisposition.FileName.Value)
               && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
    }

    private static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
    {
        return contentDisposition.DispositionType.Equals(DispositionTypeFormData)
               && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                   || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
    }

    private async Task<MultipartData<TData>?> ProcessSection<TData>(MultipartSection section, MultipartRequestData<TData> data)
    {
        if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
        {
            throw new ValidationException("encountered request section without content disposition");
        }

        var sectionName = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;

        if (HasFormDataContentDisposition(contentDisposition)
            && FormFieldNameData.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
        {
            if (data.RequestData != null)
            {
                throw new ValidationException("request data already read, provide request data exactly once!");
            }

            data.RequestData = await ReadSectionAsJson<TData>(section).ConfigureAwait(false);
            if (data.RequestData != null)
            {
                _validator.EnsureValid(data.RequestData);
            }

            return null;
        }

        if (!HasFileContentDisposition(contentDisposition)
            || !FormFieldNameFile.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (data.RequestData == null)
        {
            throw new ValidationException("request data has to be passed as the first section!");
        }

        var fileName = ReadFileName(contentDisposition);
        return new MultipartData<TData>(fileName, section.Body, data.RequestData);
    }

    private async Task<T> ReadSectionAsJson<T>(MultipartSection section)
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<T>(section.Body, _jsonOptions.Value.SerializerOptions).ConfigureAwait(false)
                   ?? throw new ValidationException("could not read json");
        }
        catch (JsonException e)
        {
            _logger.LogWarning(e, "could not read multipart request json data");
            throw new ValidationException("could not read json: " + e.Message);
        }
    }

    private sealed class MultipartRequestData<TData>
    {
        public TData? RequestData { get; set; }
    }
}
