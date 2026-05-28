// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// A cached access token together with the timestamp at which it should be refreshed.
/// </summary>
/// <param name="Token">The cached access token.</param>
/// <param name="ValidTo">The timestamp until which the token is considered valid.</param>
internal sealed record TokenWithExpiration(string Token, DateTimeOffset ValidTo);
