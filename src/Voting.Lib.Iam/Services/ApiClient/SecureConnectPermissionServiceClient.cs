// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Voting.Lib.ApiClientBase.DotNet.Converters;

namespace Voting.Lib.Iam.Services.ApiClient.Permission;

/// <summary>
/// Generated client for the secure connect permission service.
/// </summary>
public partial class SecureConnectPermissionServiceClient
{
    partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Add(new JsonDateTimeOffsetConverter());
        settings.Converters.Add(new JsonDateTimeConverter());

        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        settings.Formatting = Formatting.Indented;
        settings.DateParseHandling = DateParseHandling.None;
        settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        settings.NullValueHandling = NullValueHandling.Ignore;
    }
}
