// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// Forwards a given header.
/// </summary>
public class ForwardHeaderHandler : HeaderHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForwardHeaderHandler"/> class.
    /// </summary>
    /// <param name="name">The name of the header to forward.</param>
    /// <param name="httpContextAccessor">The http context accessor.</param>
    public ForwardHeaderHandler(string name, IHttpContextAccessor httpContextAccessor)
        : base(name)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForwardHeaderHandler"/> class.
    /// </summary>
    /// <param name="innerHandler">The inner handler.</param>
    /// <param name="name">The name of the header to forward.</param>
    /// <param name="httpContextAccessor">The http context accessor.</param>
    public ForwardHeaderHandler(HttpMessageHandler innerHandler, string name, IHttpContextAccessor httpContextAccessor)
        : base(innerHandler, name)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    protected override string? GetValue()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(Name, out var headerValue) == true
            ? headerValue.ToString()
            : null;
    }
}
