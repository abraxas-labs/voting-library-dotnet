// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Voting.Lib.DokConnector.Models;
using Voting.Lib.DokConnector.Service;
using Xunit;

namespace Voting.Lib.DokConnector.Test;

public class EaiDokConnectorTest
{
    private const string Host = "http://localhost/";

    [Fact]
    public async Task UploadShouldWork()
    {
        using var client = MockClient(new ConnectContentMatcher("fooBar", "file.txt", "my-message-type", "my-user"));
        var connector = new EaiDokConnector(new() { UserName = "my-user" }, client, NullLogger<EaiDokConnector>.Instance);

        await using var ms = new MemoryStream(Encoding.UTF8.GetBytes("fooBar"));
        var response = await connector.Upload("my-message-type", "file.txt", ms, CancellationToken.None);
        response.FileId.Should().Be("my-file-id");
    }

    private HttpClient MockClient(IMockedRequestMatcher matcher)
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When(Host + "upload")
            .With(matcher)
            .Respond("text/plain", "my-file-id");
        var client = new HttpClient(mockHttp);
        client.BaseAddress = new Uri(Host);
        return client;
    }

    private class ConnectContentMatcher : IMockedRequestMatcher
    {
        private readonly string _expectedFileContent;
        private readonly string _expectedFileName;
        private readonly string _expectedMessageType;
        private readonly string _expectedUserName;

        public ConnectContentMatcher(string expectedFileContent, string expectedFileName, string expectedMessageType, string expectedUserName)
        {
            _expectedFileContent = expectedFileContent;
            _expectedFileName = expectedFileName;
            _expectedMessageType = expectedMessageType;
            _expectedUserName = expectedUserName;
        }

        public bool Matches(HttpRequestMessage message)
        {
            var boundary = message.Content
                ?.Headers
                .ContentType
                ?.Parameters
                .FirstOrDefault(x => "boundary".Equals(x.Name, StringComparison.OrdinalIgnoreCase))
                ?.Value;
            if (boundary == null)
            {
                return false;
            }

            // trim the leading and trailing quote
            boundary = boundary.Substring(1, boundary.Length - 2);

            using var contentStream = message.Content!.ReadAsStream();
            var reader = new MultipartReader(boundary, contentStream);
            var jsonSection = reader.ReadNextSectionAsync().GetAwaiter().GetResult();
            if (jsonSection == null)
            {
                return false;
            }

            var fileUploadRequest = JsonSerializer.Deserialize<FileUploadRequest>(jsonSection.Body, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (fileUploadRequest == null
                || !_expectedFileName.Equals(fileUploadRequest.FileName)
                || !_expectedMessageType.Equals(fileUploadRequest.MessageType)
                || !_expectedUserName.Equals(fileUploadRequest.UserName))
            {
                return false;
            }

            var contentSection = reader.ReadNextSectionAsync().GetAwaiter().GetResult();
            if (contentSection == null)
            {
                return false;
            }

            using var fileContentStreamReader = new StreamReader(contentSection.Body);
            var fileContent = fileContentStreamReader.ReadToEnd();
            return fileContent.Equals(_expectedFileContent);
        }
    }
}
