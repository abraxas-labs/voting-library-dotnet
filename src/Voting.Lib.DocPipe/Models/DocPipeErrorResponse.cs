// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Voting.Lib.DocPipe.Models;

internal class DocPipeErrorResponse
{
    public List<ProblemDetails>? Errors { get; set; }

    public string GetErrorDescription()
    {
        if (Errors == null || Errors.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        foreach (var error in Errors)
        {
            sb.Append(error.Title);
            sb.Append(':');
            sb.AppendLine(error.Detail);
        }

        return sb.ToString();
    }
}
