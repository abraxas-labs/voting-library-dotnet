// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Voting.Lib.Rest.Swagger.OperationFilters;

/// <summary>
/// Swagger operation filter to add more information to operations.
/// </summary>
public class SwaggerExtensionOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies the filter to the specified operation using the given context.
    /// </summary>
    /// <param name="operation">The operation to apply the filter to.</param>
    /// <param name="context">The current operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Summary != null && operation.Summary.Contains("{{controller}}", StringComparison.OrdinalIgnoreCase))
        {
            var controllerName = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor)?.ControllerName;
            operation.Summary = operation.Summary.Replace("{{controller}}", controllerName, StringComparison.OrdinalIgnoreCase);
        }

        if (string.IsNullOrWhiteSpace(operation.OperationId))
        {
            operation.OperationId = operation.Summary;
        }
    }
}
