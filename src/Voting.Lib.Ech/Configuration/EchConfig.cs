// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Reflection;

namespace Voting.Lib.Ech.Configuration;

/// <summary>
/// eCH related configurations.
/// </summary>
public class EchConfig
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EchConfig"/> class.
    /// The entry assembly is used to retrieve product information.
    /// </summary>
    public EchConfig()
        : this(Assembly.GetEntryAssembly())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EchConfig"/> class.
    /// </summary>
    /// <param name="assembly">The assembly used to retrieve product information.</param>
    public EchConfig(Assembly? assembly)
    {
        SetProductNameFromAssembly(assembly);
        SetProductVersionFromAssembly(assembly);
    }

    /// <summary>
    /// Gets or sets the sender id used in the delivery header.
    /// </summary>
    public string SenderId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the deliveries produced are test deliveries.
    /// </summary>
    public bool TestDeliveryFlag { get; set; } = true;

    /// <summary>
    /// Gets or sets the manufacturer used in the delivery header.
    /// </summary>
    public string Manufacturer { get; set; } = "Abraxas Informatik AG";

    /// <summary>
    /// Gets or sets the product name used in the delivery header.
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product version used in the delivery header.
    /// </summary>
    public string ProductVersion { get; set; } = string.Empty;

    /// <summary>
    /// Set the product name from the assembly of the provided type.
    /// </summary>
    /// <typeparam name="T">The type to infer the assembly from.</typeparam>
    public void SetProductNameFromAssemblyOfType<T>()
        => SetProductNameFromAssembly(typeof(T).Assembly);

    /// <summary>
    /// Set the product version from the assembly of the provided type.
    /// </summary>
    /// <typeparam name="T">The type to infer the assembly from.</typeparam>
    public void SetProductVersionFromAssemblyOfType<T>()
        => SetProductVersionFromAssembly(typeof(T).Assembly);

    /// <summary>
    /// Set the product version from the provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly to use the product version from.</param>
    public void SetProductVersionFromAssembly(Assembly? assembly)
    {
        ProductVersion = assembly
            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion
            ?? throw new InvalidOperationException("Could not find the assembly version.");
    }

    /// <summary>
    /// Set the product name from the provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly to use the product name from.</param>
    public void SetProductNameFromAssembly(Assembly? assembly)
    {
        Product = assembly?.GetName().Name ?? string.Empty;
    }
}
