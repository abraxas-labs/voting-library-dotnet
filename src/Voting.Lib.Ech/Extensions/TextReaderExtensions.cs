// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Ech.Extensions;

internal static class TextReaderExtensions
{
    public static async Task SeekAsync(this TextReader reader, int charsToSkip, CancellationToken ct = default)
    {
        using var owner = MemoryPool<char>.Shared.Rent();
        var buffer = owner.Memory[..charsToSkip];
        for (var i = 0; i < charsToSkip;)
        {
            var read = await reader.ReadBlockAsync(buffer, ct);
            if (read == 0)
            {
                return;
            }

            i += read;
        }
    }

    public static async Task CopyToAsync(this TextReader reader, TextWriter writer, int charsToCopy, CancellationToken ct = default)
    {
        using var owner = MemoryPool<char>.Shared.Rent();
        var buffer = owner.Memory[..charsToCopy];
        for (var i = 0; i < charsToCopy;)
        {
            var read = await reader.CopyBlockAsync(writer, buffer, ct);
            if (read == 0)
            {
                return;
            }

            i += read;
        }
    }

    public static async Task CopyToAsync(this TextReader reader, TextWriter writer, CancellationToken ct = default)
    {
        using var owner = MemoryPool<char>.Shared.Rent();
        var read = 1;
        while (read > 0)
        {
            read = await reader.CopyBlockAsync(writer, owner.Memory, ct);
        }
    }

    private static async Task<int> CopyBlockAsync(this TextReader reader, TextWriter writer, Memory<char> buffer, CancellationToken ct)
    {
        var read = await reader.ReadBlockAsync(buffer, ct);
        if (read == 0)
        {
            return read;
        }

        await writer.WriteAsync(buffer[..read], ct);
        return read;
    }
}
