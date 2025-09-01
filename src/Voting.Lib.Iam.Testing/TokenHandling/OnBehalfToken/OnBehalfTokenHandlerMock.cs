// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.Iam.Testing.AuthenticationScheme;
using Voting.Lib.Iam.TokenHandling;

namespace Voting.Lib.Iam.Testing.TokenHandling.OnBehalfToken;

/// <summary>
/// A mock for the on behalf token handler which always returns a static on behalf token.
/// </summary>
public class OnBehalfTokenHandlerMock : ITokenHandler
{
    /// <inheritdoc />
    public Task<string> GetToken(CancellationToken cancellationToken)
        => Task.FromResult(SecureConnectTestDefaults.OnBehalfToken);
}
