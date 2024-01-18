// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.DocPipe.Models;

internal class JobRequest
{
    public JobRequest(
        string application,
        string client,
        string instance,
        Dictionary<string, string> jobVariables,
        TimeSpan? timeout)
    {
        Application = application;
        Client = client;
        Instance = instance;
        JobVariables = jobVariables;
        WaitTimeout = timeout == null ? -1 : (int)timeout.Value.TotalMilliseconds;
    }

    public string Application { get; set; }

    public string Client { get; set; }

    public string Instance { get; set; }

    public Dictionary<string, string> JobVariables { get; set; }

    public bool Wait { get; set; } = true;

    public int WaitTimeout { get; set; }
}
