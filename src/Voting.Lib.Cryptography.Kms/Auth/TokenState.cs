// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Cryptography.Kms.Auth;

internal record TokenState(string Token, DateTimeOffset Expiration, string? RefreshToken);
