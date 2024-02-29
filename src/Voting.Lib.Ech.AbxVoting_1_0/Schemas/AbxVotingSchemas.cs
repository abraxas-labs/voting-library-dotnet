// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;
using Voting.Lib.Common;

namespace Voting.Lib.Ech.AbxVoting_1_0.Schemas;

public sealed class AbxVotingSchemas
{
    [SuppressMessage(
        "SonarQube",
        "S5332: Using http protocol is insecure. Use https instead.",
        Justification = "These URLs are only used as XML namespaces")]
    private static readonly Dictionary<string, string> Schemas = new()
    {
        ["http://www.ech.ch/xmlns/eCH-0006/2"] = "eCH-0006-2-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0007/5"] = "eCH-0007-5-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0007/6"] = "eCH-0007-6-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0008/3"] = "eCH-0008-3-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0010/5"] = "eCH-0010-5-1.xsd",
        ["http://www.ech.ch/xmlns/eCH-0011/8"] = "eCH-0011-8-1.xsd",
        ["http://www.ech.ch/xmlns/eCH-0021/7"] = "eCH-0021-7-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0044/4"] = "eCH-0044-4-1.xsd",
        ["http://www.ech.ch/xmlns/eCH-0058/5"] = "eCH-0058-5-0.xsd",
        ["http://www.ech.ch/xmlns/eCH-0135/1"] = "eCH-0135-1-0.xsd",
        ["http://www.abraxas.ch/xmlns/ABX-Voting/1"] = "ABX-Voting-1-0.xsd",
    };

    public static XmlSchemaSet LoadAbxVotingSchemas()
        => XmlUtil.LoadSchemasFromResources<AbxVotingSchemas>(Schemas);
}
