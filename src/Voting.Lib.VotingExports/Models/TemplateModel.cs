// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Voting.Lib.VotingExports.Models;

/// <summary>
/// A template model represents a single template.
/// </summary>
public record TemplateModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateModel"/> class.
    /// </summary>
    internal TemplateModel()
    {
    }

    /// <summary>
    /// Gets the key which uniquely identifies a template.
    /// This is also used to resolve the dm doc template.
    /// </summary>
    public string Key { get; init; } = string.Empty;

    /// <summary>
    /// Gets the filename used for the export.
    /// </summary>
    public string Filename { get; init; } = string.Empty;

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets the file format.
    /// </summary>
    public ExportFileFormat Format { get; init; }

    /// <summary>
    /// Gets the entity type of this template.
    /// </summary>
    public EntityType EntityType { get; init; }

    /// <summary>
    /// Gets the result type of this template.
    /// </summary>
    public ResultType? ResultType { get; init; }

    /// <summary>
    /// Gets the app, which can generate this export.
    /// </summary>
    public VotingApp GeneratedBy { get; init; }

    /// <summary>
    /// Gets the supported <see cref="DomainOfInfluenceType"/>s of this template.
    /// If this is <c>null</c>, all types are considered as supported.
    /// </summary>
    public IReadOnlySet<DomainOfInfluenceType>? DomainOfInfluenceTypes { get; init; }

    /// <summary>
    /// Gets a value indicating whether this template is specific per <see cref="DomainOfInfluenceType"/>.
    /// </summary>
    public bool PerDomainOfInfluenceType { get; init; }

    /// <summary>
    /// Validates this template model.
    /// </summary>
    /// <exception cref="ValidationException">Thrown when this template model is invalid.</exception>
    internal void Validate()
    {
        if (PerDomainOfInfluenceType
            && ResultType is not Models.ResultType.MultiplePoliticalBusinessesResult and not Models.ResultType.MultiplePoliticalBusinessesCountingCircleResult)
        {
            throw new ValidationException($"{PerDomainOfInfluenceType} can only be true for multiple business exports");
        }
    }

    /// <summary>
    /// Whether this template model is available for a provided <see cref="DomainOfInfluenceTypes"/>.
    /// </summary>
    /// <param name="t">The type of the domain of influence.</param>
    /// <returns>Whether this template model is available for the provided argument.</returns>
    public bool MatchesDomainOfInfluenceType(DomainOfInfluenceType t)
        => DomainOfInfluenceTypes?.Contains(t) != false;
}
