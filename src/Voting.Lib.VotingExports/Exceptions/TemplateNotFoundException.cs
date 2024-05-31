// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.VotingExports.Exceptions;

/// <summary>
/// A template couldn't be found.
/// </summary>
[Serializable]
public class TemplateNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateNotFoundException"/> class.
    /// </summary>
    /// <param name="key">The template key which wasn't found.</param>
    public TemplateNotFoundException(string key)
        : base($"Template with key {key} not found")
    {
    }
}
