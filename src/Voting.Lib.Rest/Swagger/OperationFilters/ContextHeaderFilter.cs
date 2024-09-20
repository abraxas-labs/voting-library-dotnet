// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Rest.Swagger.Configuration;

namespace Voting.Lib.Rest.Swagger.OperationFilters;

/// <summary>
/// Swagger operation filter to add header information.
/// </summary>
public class ContextHeaderFilter : IOperationFilter
{
    private readonly SwaggerConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextHeaderFilter"/> class.
    /// </summary>
    /// <param name="config">The swagger configuration.</param>
    public ContextHeaderFilter(IOptions<SwaggerConfig> config)
    {
        _config = config.Value;
    }

    /// <summary>
    /// Applies the filter to the specified operation using the given context.
    /// </summary>
    /// <param name="operation">The operation to apply the filter to.</param>
    /// <param name="context">The current operation filter context.</param>
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        if (!_config.EnableContextHeaderInjection)
        {
            return;
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = new SecureConnectOptions().AppHeaderName,
            In = ParameterLocation.Header,
            Description = "The application context identifier.",
            Required = true,
            Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString(_config.DefaultAppContextValue) },
        });

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = new SecureConnectOptions().TenantHeaderName,
            In = ParameterLocation.Header,
            Description = "The tenant context identifier.",
            Required = true,
            Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString(_config.DefaultTenantContextValue) },
        });
    }
}
