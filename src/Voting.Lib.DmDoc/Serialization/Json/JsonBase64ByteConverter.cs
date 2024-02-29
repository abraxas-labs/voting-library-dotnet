// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Voting.Lib.DmDoc.Serialization.Json;

// since dm doc encodes base64 with \n chars in it,
// the default dotnet base64 byte converter cannot handle it :(
internal sealed class JsonBase64ByteConverter : JsonConverter<byte[]>
{
    public static readonly JsonBase64ByteConverter Instance = new();

    private JsonBase64ByteConverter()
    {
    }

    public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();
        return s == null
            ? null
            : Convert.FromBase64String(s); // Convert.FromBase64String can handle \n
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(Convert.ToBase64String(value));
    }
}
