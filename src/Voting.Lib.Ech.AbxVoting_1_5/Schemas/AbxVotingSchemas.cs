// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;
using Voting.Lib.Common;

namespace Voting.Lib.Ech.AbxVoting_1_5.Schemas;

public sealed class AbxVotingSchemas
{
    [SuppressMessage(
        "SonarQube",
        "S5332: Using http protocol is insecure. Use https instead.",
        Justification = "These URLs are only used as XML namespaces")]
    private static readonly Dictionary<string, string> Schemas = new()
    {
        ["http://www.ech.ch/xmlns/eCH-0006/3"] = "eCH-0006-3-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0007/6"] = "eCH-0007-6-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0008/3"] = "eCH-0008-3-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0010/8"] = "eCH-0010-8-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0011/9"] = "eCH-0011-9-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0021/8"] = "eCH-0021-8-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0044/4"] = "eCH-0044-4-1.xsd",
        ["http://www.ech.ch/xmlns/eCH-0058/5"] = "eCH-0058-5-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0135/2"] = "eCH-0135-2-0.xsd",
        ["http://www.abraxas.ch/xmlns/ABX-Voting/1"] = "ABX-Voting-1-5.xsd",
    };

    public static XmlSchemaSet LoadAbxVotingSchemas()
        => XmlUtil.LoadSchemasFromResources<AbxVotingSchemas>(Schemas);
}
