// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Voting.Lib.Common;

/// <summary>
/// A stream to validate an XML while writing it to somewhere else.
/// Make sure to call <see cref="WaitForValidation"/> to ensure that the whole XML has been validated.
/// </summary>
public sealed class XmlValidationOnWriteStream : Stream
{
#pragma warning disable CA2213
    // Do not dispose this stream, is handled by the caller
    private readonly Stream _targetStream;
#pragma warning restore CA2213

    private readonly Pipe _pipe;
    private readonly XmlReader _xmlReader;
    private readonly Task _validationTask;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlValidationOnWriteStream"/> class.
    /// </summary>
    /// <param name="targetStream">The stream to write to.</param>
    /// <param name="schemaSet">The schemas used for validation.</param>
    public XmlValidationOnWriteStream(Stream targetStream, XmlSchemaSet schemaSet)
    {
        _targetStream = targetStream;
        _pipe = new Pipe();
        _xmlReader = XmlUtil.CreateReaderWithSchemaValidation(_pipe.Reader.AsStream(), schemaSet, new XmlReaderSettings { Async = true });
        _validationTask = ValidateXml();
    }

    /// <inheritdoc />
    public override bool CanRead => false;

    /// <inheritdoc />
    public override bool CanSeek => false;

    /// <inheritdoc />
    public override bool CanWrite => _targetStream.CanWrite;

    /// <inheritdoc />
    public override long Length => _targetStream.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _targetStream.Position;
        set => _targetStream.Position = value;
    }

    /// <inheritdoc />
    public override void Flush()
        => _targetStream.Flush();

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
        => throw new InvalidOperationException("Can't read from this stream");

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
        => throw new InvalidOperationException("Stream does not support seeking");

    /// <inheritdoc />
    public override void SetLength(long value)
        => _targetStream.SetLength(value);

    /// <inheritdoc />
    public override void Write(ReadOnlySpan<byte> buffer)
    {
        _pipe.Writer.Write(buffer);
        _targetStream.Write(buffer);
    }

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        Write(buffer.AsSpan(offset, count));
    }

    /// <summary>
    /// Wait until the whole XML has been validated.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task WaitForValidation()
    {
        await _pipe.Writer.CompleteAsync().ConfigureAwait(false);
        await _validationTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        _xmlReader.Dispose();
        base.Dispose(disposing);
    }

    private async Task ValidateXml()
    {
        while (await _xmlReader.ReadAsync().ConfigureAwait(false))
        {
            // Nothing to do here, just reading to trigger the schema validation
        }
    }
}
