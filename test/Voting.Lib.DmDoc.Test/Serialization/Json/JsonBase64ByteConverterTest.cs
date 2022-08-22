// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text;
using System.Text.Json;
using FluentAssertions;
using Voting.Lib.DmDoc.Serialization.Json;
using Xunit;

namespace Voting.Lib.DmDoc.Test.Serialization.Json;

public class JsonBase64ByteConverterTest
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters =
            {
                JsonBase64ByteConverter.Instance,
            },
    };

    [Theory]
    [InlineData("simple", "Zm9vQmFy", "fooBar", "Zm9vQmFy")]
    [InlineData("with new lines", "Zm9vQmFy\\nRm9vQmFy\\nRm9vQmFy", "fooBarFooBarFooBar", "Zm9vQmFyRm9vQmFyRm9vQmFy")]
    [InlineData("with encoded new lines", "Zm9vQmFyCmZvb0Jhcgpmb29CYXIK", "fooBar\nfooBar\nfooBar\n", "Zm9vQmFyCmZvb0Jhcgpmb29CYXIK")]
    public void ShouldWork(string testName, string jsonContent, string expectedUtf8, string expectedSerialized)
    {
        var jsonString = $"\"{jsonContent}\"";

        // deserialize
        var data = JsonSerializer.Deserialize<byte[]>(jsonString, Options);
        data.Should().NotBeNull();
        Encoding.UTF8.GetString(data!)
            .Should()
            .Be(expectedUtf8, testName);

        // serialize
        JsonSerializer.Serialize(data, Options)
            .Should()
            .Be($"\"{expectedSerialized}\"", testName);
    }
}
