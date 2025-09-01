// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Kms.ApiModels;

internal record ErrorResponse(int Code, string CodeDesc, string? Message);
