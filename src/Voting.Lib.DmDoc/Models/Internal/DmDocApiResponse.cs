// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Voting.Lib.DmDoc.Models.Internal;

internal class DmDocApiResponse
{
    public bool Success { get; set; }

    public string? Error { get; set; }

    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    public List<string>? Errors { get; set; }

    public string GetErrorDescription()
    {
        if (Errors == null)
        {
            return Error ?? Message ?? "No error information provided";
        }

        return
            Error ?? Message ?? string.Empty
            + Environment.NewLine
            + string.Join(
                Environment.NewLine,
                Errors);
    }
}
