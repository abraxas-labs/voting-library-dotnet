// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using FluentAssertions;
using ImageMagick;
using Xunit;

namespace Voting.Lib.ImageProcessing.Test;

public class MagickImageProcessorTest
{
    private readonly MagickImageProcessor _processor = new();

    [Theory]
    [InlineData("test-png.png")]
    [InlineData("test-tiff.tiff")]
    [InlineData("test-svg.svg")]
    public void ShouldConvert(string srcFile)
    {
        var srcFileBytes = File.ReadAllBytes(GetTestFilePath(srcFile));

        var processedFileBytes = _processor.ConvertFormat(srcFileBytes, ImageFormat.Png);

        var image = new MagickImage(processedFileBytes);
        image.Format.Should().Be(MagickFormat.Png);
    }

    [Theory]
    [InlineData("test-png.png")]
    [InlineData("test-tiff.tiff")]
    [InlineData("test-svg.svg")]
    public void ShouldResize(string srcFile)
    {
        var srcFileBytes = File.ReadAllBytes(GetTestFilePath(srcFile));

        var processedFileBytes = _processor.Resize(srcFileBytes, 300, 300, true);

        var image = new MagickImage(processedFileBytes);
        image.BaseWidth.Should().Be(300);
        image.BaseHeight.Should().Be(225);
    }

    [Fact]
    public void ShouldLosslessCompressPng()
    {
        var srcFileBytes = File.ReadAllBytes(GetTestFilePath("test-png.png"));

        var processedFileBytes = _processor.LosslessCompress(srcFileBytes);
        processedFileBytes.Length.Should().BeLessThan(srcFileBytes.Length);
    }

    private string GetTestFilePath(string fileName)
    {
        var assemblyFolder = Path.GetDirectoryName(GetType().Assembly.Location)!;
        return Path.Combine(assemblyFolder, "TestFiles", fileName);
    }
}
