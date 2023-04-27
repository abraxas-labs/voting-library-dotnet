// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;

namespace Voting.Lib.Common;

/// <summary>
/// Validates an object with <see cref="Validator"/>.
/// </summary>
public class AttributeValidator
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeValidator"/> class.
    /// </summary>
    /// <param name="serviceProvider">A service provider instance.</param>
    public AttributeValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Ensures an object instance is valid, throws otherwise.
    /// </summary>
    /// <param name="instance">The object to validate.</param>
    /// <exception cref="ValidationException">If the object is not valid.</exception>
    public void EnsureValid(object instance)
    {
        Validator.ValidateObject(
            instance,
            new ValidationContext(instance, _serviceProvider, null),
            true);
    }
}
