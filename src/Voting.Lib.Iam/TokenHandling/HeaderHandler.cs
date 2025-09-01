// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// Adds a header value to the http message if no header with the given name is present yet.
/// </summary>
public abstract class HeaderHandler : DelegatingHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderHandler"/> class.
    /// </summary>
    /// <param name="name">The name of the header to add.</param>
    protected HeaderHandler(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderHandler"/> class.
    /// </summary>
    /// <param name="innerHandler">The inner handler.</param>
    /// <param name="name">The name of the header.</param>
    protected HeaderHandler(HttpMessageHandler innerHandler, string name)
        : base(innerHandler)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of the header.
    /// </summary>
    protected string Name { get; }

    /// <inheritdoc />
    protected override HttpResponseMessage Send(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        AddHeader(request);
        return base.Send(request, cancellationToken);
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        AddHeader(request);
        return base.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// Provides the value to be added to the request.
    /// </summary>
    /// <returns>The header value.</returns>
    protected abstract string? GetValue();

    private void AddHeader(HttpRequestMessage request)
    {
        if (!request.Headers.Contains(Name)
            && GetValue() is { } value)
        {
            request.Headers.Add(Name, value);
        }
    }
}
