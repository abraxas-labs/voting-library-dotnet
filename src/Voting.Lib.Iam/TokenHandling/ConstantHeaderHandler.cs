// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// Adds a constant header value to the http message if no header with the given name is present yet.
/// </summary>
public class ConstantHeaderHandler : HeaderHandler
{
    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstantHeaderHandler"/> class.
    /// </summary>
    /// <param name="name">The name of the header to add.</param>
    /// <param name="value">The value of the header to add.</param>
    public ConstantHeaderHandler(string name, string value)
        : base(name)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstantHeaderHandler"/> class.
    /// </summary>
    /// <param name="innerHandler">The inner handler.</param>
    /// <param name="name">The name of the header to add.</param>
    /// <param name="value">The value of the header to add.</param>
    public ConstantHeaderHandler(HttpMessageHandler innerHandler, string name, string value)
        : base(innerHandler, name)
    {
        _value = value;
    }

    /// <inheritdoc />
    protected override string GetValue() => _value;
}
