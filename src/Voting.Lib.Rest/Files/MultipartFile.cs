// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;

namespace Voting.Lib.Rest.Files;

/// <summary>
/// Represents an uploaded file.
/// </summary>
/// <param name="FileName">The name of the uploaded file.</param>
/// <param name="Content">The content of the uploaded file.</param>
/// <param name="ContentType">The content type of the uploaded file.</param>
/// <param name="FormFieldName">The name of the form field in which this file was uploaded.</param>
public record MultipartFile(string? FileName, Stream Content, string? ContentType, string FormFieldName);
