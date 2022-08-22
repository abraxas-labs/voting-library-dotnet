// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using ImageMagick;

namespace Voting.Lib.ImageProcessing;

/// <inheritdoc />
public class MagickImageProcessor : IImageProcessor
{
    /// <inheritdoc />
    public byte[] ConvertFormat(byte[] data, ImageFormat format)
    {
        using var image = new MagickImage(data);
        image.Format = GetFormat(format);
        return image.ToByteArray();
    }

    /// <inheritdoc />
    public byte[] LosslessCompress(byte[] data)
    {
        using var ms = new MemoryStream(data);

        var optimizer = new ImageOptimizer();
        optimizer.LosslessCompress(ms);

        return ms.ToArray();
    }

    /// <inheritdoc />
    public byte[] Resize(byte[] data, int width, int height, bool maintainAspectRatio)
    {
        using var image = new MagickImage(data);

        var size = new MagickGeometry(width, height);
        size.IgnoreAspectRatio = !maintainAspectRatio;

        image.Resize(size);
        return image.ToByteArray();
    }

    private MagickFormat GetFormat(ImageFormat format) =>
        format switch
        {
            ImageFormat.Png => MagickFormat.Png,
            _ => throw new ArgumentException($"Format {format} is not supported"),
        };
}
