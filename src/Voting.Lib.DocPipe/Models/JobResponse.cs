// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DocPipe.Models;

internal class JobResponse<T>
{
    public int JobId { get; set; }

    public int JobStatus { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? CustomResultData { get; set; }
}
