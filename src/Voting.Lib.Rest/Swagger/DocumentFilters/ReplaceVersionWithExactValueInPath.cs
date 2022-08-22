// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Voting.Lib.Rest.Swagger.DocumentFilters;

/// <summary>
/// Swagger document filter to replace a version placeholder with the exact version.
/// </summary>
public class ReplaceVersionWithExactValueInPath : IDocumentFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths();
        swaggerDoc.Paths.ToList().ForEach(p => paths.Add(
            p.Key.Replace("v{version}", $"v{swaggerDoc.Info.Version}", StringComparison.OrdinalIgnoreCase),
            p.Value));

        swaggerDoc.Paths = paths;
    }
}
