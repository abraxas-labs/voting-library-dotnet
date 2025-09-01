// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Cryptography.Kms.Auth;

/// <summary>
/// Adds KMS auth to each request without an authorization header.
/// </summary>
public class AuthHandler : DelegatingHandler
{
    private readonly TokenProvider _tokenProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthHandler"/> class.
    /// </summary>
    /// <param name="tokenProvider">The token provider used to fetch the tokens.</param>
    public AuthHandler(TokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    /// <inheritdoc cref="DelegatingHandler"/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization ??= new AuthenticationHeaderValue("Bearer", await _tokenProvider.GetToken());
        return await base.SendAsync(request, cancellationToken);
    }
}
