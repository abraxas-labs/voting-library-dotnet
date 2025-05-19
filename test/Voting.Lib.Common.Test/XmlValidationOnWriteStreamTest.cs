// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Voting.Lib.Ech.Ech0159_4_0.Schemas;
using Xunit;

namespace Voting.Lib.Common.Test;

public class XmlValidationOnWriteStreamTest
{
    [Fact]
    public async Task ShouldThrowOnInvalidXml()
    {
        await using var xmlSourceStream = new MemoryStream("<?xml version=\"1.0\" encoding=\"utf-8\"?>"u8.ToArray());
        await using var targetStream = new MemoryStream();

        await using var validationStream = new XmlValidationOnWriteStream(targetStream, Ech0159Schemas.LoadEch0159Schemas());
        await xmlSourceStream.CopyToAsync(validationStream);

        await Assert.ThrowsAsync<XmlException>(() => validationStream.WaitForValidation());
    }
}
