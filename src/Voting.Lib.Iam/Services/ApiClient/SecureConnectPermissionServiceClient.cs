// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Abraxas.Foundation.ApiClient.Base.DotNet.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Voting.Lib.Iam.Services.ApiClient.Permission;

/// <summary>
/// Generated client for the secure connect permission service.
/// </summary>
public partial class SecureConnectPermissionServiceClient
{
    // settings from https://gitlab.abraxas-tools.ch/iks/apps/iks-internes-kontrollsystem/iks-service/-/blob/f227aceffdfadf6d245557ea1207615b77304524/src/SecureConnect.ApiClients/SecureConnect.ApiClient.DotNet/Extensions/ServiceClient.cs
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
