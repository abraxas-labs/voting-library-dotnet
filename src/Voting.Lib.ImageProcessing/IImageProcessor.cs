// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.ImageProcessing;

/// <summary>
/// Interface for an image processor.
/// </summary>
public interface IImageProcessor
{
    /// <summary>
    /// Resizes the image.
    /// </summary>
    /// <param name="data">Image bytes.</param>
    /// <param name="width">New width.</param>
    /// <param name="height">New height.</param>
    /// <param name="maintainAspectRatio">Whether the original aspect ratio should be maintained.</param>
    /// <returns>The bytes of the processed image.</returns>
    byte[] Resize(byte[] data, int width, int height, bool maintainAspectRatio);

    /// <summary>
    /// Converts the image to a new format.
    /// </summary>
    /// <param name="data">Image bytes.</param>
    /// <param name="format">New format.</param>
    /// <returns>The bytes of the processed image.</returns>
    byte[] ConvertFormat(byte[] data, ImageFormat format);

    /// <summary>
    /// Compresses the image lossless.
    /// </summary>
    /// <param name="data">Image bytes.</param>
    /// <returns>The bytes of the processed image.</returns>
    byte[] LosslessCompress(byte[] data);
}
