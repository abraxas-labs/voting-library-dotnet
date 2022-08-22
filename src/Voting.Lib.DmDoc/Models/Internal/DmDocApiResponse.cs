// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.DmDoc.Models.Internal;

internal class DmDocApiResponse
{
    public bool Success { get; set; }

    public string? Error { get; set; }

    public List<string>? Errors { get; set; }

    public string GetErrorDescription()
    {
        if (Errors == null)
        {
            return Error ?? string.Empty;
        }

        return
            Error ?? string.Empty
            + Environment.NewLine
            + string.Join(
                Environment.NewLine,
                Errors);
    }
}
