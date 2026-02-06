// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Cryptography.Kms.ApiModels;

internal record CreateKeyRequest(
    string Name,
    KeyUsageMask UsageMask,
    string Algorithm,
    Dictionary<string, string>? Labels = null,
    int? Size = null,
    bool Unexportable = true,
    bool AssignSelfAsOwner = true,
    string? CurveId = null);
