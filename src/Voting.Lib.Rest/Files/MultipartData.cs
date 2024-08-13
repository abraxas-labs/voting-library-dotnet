// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;

namespace Voting.Lib.Rest.Files;

/// <summary>
/// Represents a file uploaded with a json request.
/// </summary>
/// <param name="FileName">The name of the uploaded file.</param>
/// <param name="FileContent">The content of the uploaded file.</param>
/// <param name="ContentType">The content type of the uploaded file.</param>
/// <param name="RequestData">The json data of the request.</param>
/// <typeparam name="TData">The type of the data of the request.</typeparam>
public record MultipartData<TData>(string? FileName, Stream FileContent, string? ContentType, TData RequestData);
