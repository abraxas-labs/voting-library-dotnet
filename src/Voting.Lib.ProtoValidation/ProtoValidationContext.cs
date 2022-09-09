// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.ProtoValidation;

/// <summary>
/// Contains data of the validation of the whole message.
/// </summary>
public class ProtoValidationContext
{
    /// <summary>
    /// Gets validation failures.
    /// </summary>
    public List<ProtoValidationError> Failures { get; } = new();
}
