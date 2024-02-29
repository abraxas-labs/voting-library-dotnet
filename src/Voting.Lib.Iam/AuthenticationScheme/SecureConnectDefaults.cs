// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text.Json;
using System.Text.Json.Serialization;
using Voting.Lib.Common.Json;

namespace Voting.Lib.Iam.AuthenticationScheme;

/// <summary>
/// Constant default values for secure connect.
/// </summary>
public static class SecureConnectDefaults
{
    /// <summary>
    /// Default value for AuthenticationScheme property in the SecureConnectOptions.
    /// </summary>
    public const string AuthenticationScheme = "SecureConnect";

    /// <summary>
    /// Name of the http client to be used for backchannel communication.
    /// </summary>
    public const string BackchannelHttpClientName = "SecureConnect";

    internal const string BearerScheme = "Bearer";

    /// <summary>
    /// The client id scope namespace prefix to be sent to secure connect.
    /// </summary>
    internal const string ScopeNamespacePrefix = "urn:abraxas:iam:audience_client_id:";

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonSnakeCaseNamingPolicy.Instance,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };
}
