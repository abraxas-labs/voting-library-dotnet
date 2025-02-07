// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;
using Voting.Lib.Common;

namespace Voting.Lib.Ech.Ech0252_2_0.Schemas;

/// <summary>
/// eCH-0252 schema loader.
/// </summary>
public sealed class Ech0252Schemas
{
    [SuppressMessage(
        "SonarQube",
        "S5332: Using http protocol is insecure. Use https instead.",
        Justification = "These URLs are only used as XML namespaces")]
    private static readonly Dictionary<string, string> Schemas = new()
    {
        ["http://www.ech.ch/xmlns/eCH-0006/2"] = "eCH-0006-2-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0007/6"] = "eCH-0007-6-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0008/3"] = "eCH-0008-3-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0010/6"] = "eCH-0010-6-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0044/4"] = "eCH-0044-4-1.xsd",
        ["http://www.ech.ch/xmlns/eCH-0058/5"] = "eCH-0058-5-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0155/5"] = "eCH-0155-5-1.xsd",
        ["http://www.ech.ch/xmlns/eCH-0252/2"] = "eCH-0252-2-0.xsd",
    };

    private Ech0252Schemas()
    {
    }

    /// <summary>
    /// Builds an eCH-0252 schema set.
    /// </summary>
    /// <returns>The created schema set.</returns>
    public static XmlSchemaSet LoadEch0252Schemas()
        => XmlUtil.LoadSchemasFromResources<Ech0252Schemas>(Schemas);
}
