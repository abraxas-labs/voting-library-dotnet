// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;
using Voting.Lib.Common;

namespace Voting.Lib.Ech.Ech0072_1_0.Schemas;

public sealed class Ech0072Schemas
{
    [SuppressMessage(
        "SonarQube",
        "S5332: Using http protocol is insecure. Use https instead.",
        Justification = "These URLs are only used as XML namespaces")]
    private static readonly Dictionary<string, string> Schemas = new()
    {
        ["http://www.ech.ch/xmlns/eCH-0072/1"] = "eCH-0072-1-0.xsd",
    };

    public static XmlSchemaSet LoadEch0072Schemas()
        => XmlUtil.LoadSchemasFromResources<Ech0072Schemas>(Schemas);
}
