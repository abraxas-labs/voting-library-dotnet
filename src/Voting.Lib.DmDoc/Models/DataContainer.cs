// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// A DmDoc data container.
/// </summary>
public class DataContainer
{
    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this data container is global.
    /// </summary>
    public bool Global { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this data container is editable.
    /// </summary>
    public bool Editable { get; set; }

    /// <summary>
    /// Gets or sets the internal name.
    /// </summary>
    public string InternName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data container name.
    /// </summary>
    public string DataContainerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data container fields.
    /// </summary>
    public List<DataField> Fields { get; set; } = new();
}
