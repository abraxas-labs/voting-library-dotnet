// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

// copied from https://github.com/StephenClearyExamples/AsyncDynamicZip/blob/86a908d7715fe8601bef8dc4e88a327ffe5bf896/Example/src/WebApplication/WriteOnlyStreamWrapper.cs
namespace Voting.Lib.Rest.Utils;

/// <summary>
/// Wraps a stream to make it write-only.
/// </summary>
public class WrappedWriteOnlyStream : Stream
{
    private readonly Stream _stream;
    private long _position;

    /// <summary>
    /// Initializes a new instance of the <see cref="WrappedWriteOnlyStream"/> class.
    /// </summary>
    /// <param name="stream">The stream to wrap.</param>
    public WrappedWriteOnlyStream(Stream stream)
    {
        _stream = stream;
    }

    /// <inheritdoc />
    public override bool CanRead => false;

    /// <inheritdoc />
    public override bool CanSeek => false;

    /// <inheritdoc />
    public override bool CanWrite => true;

    /// <inheritdoc />
    public override int ReadTimeout
    {
        get => _stream.ReadTimeout;
        set => _stream.ReadTimeout = value;
    }

    /// <inheritdoc />
    public override int WriteTimeout
    {
        get => _stream.WriteTimeout;
        set => _stream.WriteTimeout = value;
    }

    /// <inheritdoc />
    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override bool CanTimeout => _stream.CanTimeout;

    /// <inheritdoc />
    public override long Length => throw new NotSupportedException();

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        _stream.Write(buffer, offset, count);
        _position += count;
    }

    /// <inheritdoc />
    public override void WriteByte(byte value)
    {
        _stream.WriteByte(value);
        _position += 1;
    }

    /// <inheritdoc />
    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        await _stream.WriteAsync(buffer, offset, count, cancellationToken);
        _position += count;
    }

    /// <inheritdoc />
    public override void Flush() => _stream.Flush();

    /// <inheritdoc />
    public override Task FlushAsync(CancellationToken cancellationToken) => _stream.FlushAsync(cancellationToken);

    /// <inheritdoc />
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        => _stream.CopyToAsync(destination, bufferSize, cancellationToken);

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    /// <inheritdoc />
    public override void SetLength(long value) => throw new NotSupportedException();

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _stream.Dispose();
        }

        base.Dispose(disposing);
    }
}
