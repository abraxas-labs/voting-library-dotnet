// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateNotFoundException"/> class.
    /// </summary>
    /// <param name="serializationInfo">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected TemplateNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}
