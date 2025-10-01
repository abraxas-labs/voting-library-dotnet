// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Ech.Test;

public class EchSchemaFinderTest
{
    [Fact]
    public void ShouldReturnSchemaWithRootNamespaceInDefaultAttribute()
    {
        var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <delivery xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.ech.ch/xmlns/eCH-0157/5"">
                <deliveryHeader>
                    <senderId xmlns=""http://www.ech.ch/xmlns/eCH-0058/5"">ABX-VOTING</senderId>
                </deliveryHeader>
        </delivery>
        ";

        var ms = BufferToTemporaryStream(xml);
        var validSchemas = new[] { "eCH-0157/4", "eCH-0157/5" };
        var foundSchema = EchSchemaFinder.GetSchema(ms, validSchemas);
        foundSchema.Should().Be("eCH-0157/5");
    }

    [Fact]
    public void ShouldReturnSchemaWithRootNamespaceInRootElementName()
    {
        var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <eCH-0157:delivery xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:eCH-0157=""http://www.ech.ch/xmlns/eCH-0157/5"" xmlns:eCH-0058=""http://www.ech.ch/xmlns/eCH-0058/5"">
                <eCH-0058:deliveryHeader>
                    <eCH-0058:senderId xmlns=""http://www.ech.ch/xmlns/eCH-0058/5"">ABX-VOTING</eCH-0058:senderId>
                </eCH-0058:deliveryHeader>
        </eCH-0157:delivery>
        ";

        var ms = BufferToTemporaryStream(xml);
        var validSchemas = new[] { "eCH-0157/4", "eCH-0157/5" };
        var foundSchema = EchSchemaFinder.GetSchema(ms, validSchemas);
        foundSchema.Should().Be("eCH-0157/5");
    }

    [Fact]
    public void ShouldReturnNoSchemaWithNoMatch()
    {
        var xml = @"<book xmlns=""eCH-0000"" xmlns:eCH-0222=""http://www.ech.ch/xmlns/eCH-0222/2"">1</book>";
        var ms = BufferToTemporaryStream(xml);
        var validSchemas = new[] { "eCH-0222/1", "eCH-0222/2" };
        var foundSchema = EchSchemaFinder.GetSchema(ms, validSchemas);
        foundSchema.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnNoSchemaWithNoExplicitRootNamespace()
    {
        var xml = "<book>1</book>";
        var ms = BufferToTemporaryStream(xml);
        var validSchemas = new[] { "eCH-0000" };
        var foundSchema = EchSchemaFinder.GetSchema(ms, validSchemas);
        foundSchema.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnNoSchemaWithNoValidSchema()
    {
        var xml = "<book>1</book>";
        var ms = BufferToTemporaryStream(xml);
        var foundSchema = EchSchemaFinder.GetSchema(ms, Array.Empty<string>());
        foundSchema.Should().BeNull();
    }

    private Stream BufferToTemporaryStream(string xml)
    {
        var ms = new MemoryStream();
        using (StreamWriter writer = new StreamWriter(ms, Encoding.UTF8, 1024, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }

        ms.Seek(0, SeekOrigin.Begin);
        return ms;
    }
}
