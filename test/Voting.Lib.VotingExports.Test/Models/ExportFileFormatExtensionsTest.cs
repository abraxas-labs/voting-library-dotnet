// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using FluentAssertions;
using Voting.Lib.VotingExports.Models;
using Xunit;

namespace Voting.Lib.VotingExports.Test.Models;

public class ExportFileFormatExtensionsTest
{
    [Theory]
    [InlineData(ExportFileFormat.Csv, "text/csv")]
    [InlineData(ExportFileFormat.Pdf, "application/pdf")]
    [InlineData(ExportFileFormat.Xml, "application/xml")]
    public void GetMimeTypeShouldWork(ExportFileFormat format, string mime)
    {
        format.GetMimeType().Should().Be(mime);
    }

    [Fact]
    public void GetMimeTypeShouldThrowForUnknownFormat()
    {
        Assert.Throws<ArgumentException>(() => ExportFileFormat.Unspecified.GetMimeType());
        Assert.Throws<ArgumentException>(() => ((ExportFileFormat)int.MaxValue).GetMimeType());
    }
}
