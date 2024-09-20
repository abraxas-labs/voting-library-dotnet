// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Rest.Files;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Rest.Test.Files;

public class SingleFileResultTest
{
    [Fact]
    public async Task SingleFileShouldReturn()
    {
        var result = SingleFileResult.Create(BuildSingleFile());
        result.ContentType.Should().Be("plain/text");
        result.FileDownloadName.Should().Be("test-file.txt");
        var responseBuffer = await FetchResponse(result);
        Encoding.UTF8.GetString(responseBuffer).Should().Be("Content of test-file.txt");
    }

    [Fact]
    public async Task MultipleFilesShouldZip()
    {
        var result = SingleFileResult.CreateZipFile(BuildFiles(3), "files.zip");
        result.ContentType.Should().Be("application/zip");
        result.FileDownloadName.Should().Be("files.zip");
        var responseBuffer = await FetchResponse(result);

        await using var zipStream = new MemoryStream(responseBuffer);
        using var unzipped = new ZipArchive(zipStream, ZipArchiveMode.Read, false, Encoding.UTF8);
        unzipped.Entries.Should().HaveCount(3);

        var i = 0;
        foreach (var entry in unzipped.Entries.OrderBy(x => x.Name))
        {
            entry.Name.Should().Be($"test-file-{i}.txt");

            await using var entryContent = entry.Open();
            using var reader = new StreamReader(entryContent);
            var entryContentText = await reader.ReadToEndAsync();
            entryContentText.Should().Be($"Content of test-file-{i}.txt");
            i++;
        }
    }

    [Fact]
    public async Task ZipEntriesShouldHaveTimezoneDateTime()
    {
        var clock = new MockedClock();
        var result = SingleFileResult.CreateZipFile(BuildFiles(3), "files.zip", clock.UtcNow.ConvertUtcTimeToSwissTime());
        result.ContentType.Should().Be("application/zip");
        result.FileDownloadName.Should().Be("files.zip");
        var responseBuffer = await FetchResponse(result);

        await using var zipStream = new MemoryStream(responseBuffer);
        using var unzipped = new ZipArchive(zipStream, ZipArchiveMode.Read, false, Encoding.UTF8);
        unzipped.Entries.Should().HaveCount(3);

        foreach (var entry in unzipped.Entries.OrderBy(x => x.Name))
        {
            entry.LastWriteTime.Should().Be(new DateTime(2020, 1, 10, 14, 12, 10));
        }
    }

    private async Task<byte[]> FetchResponse(IActionResult actionResult)
    {
        await using var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        await using var responseStream = new MemoryStream();
        var features = new FeatureCollection();
        features.Set<IHttpRequestFeature>(new HttpRequestFeature());
        features.Set<IHttpResponseFeature>(new HttpResponseFeature());
        features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(responseStream));

        var httpContext = new DefaultHttpContext(features);
        httpContext.ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        var actionContext = new ActionContext(httpContext, new(), new());
        await actionResult.ExecuteResultAsync(actionContext);
        await actionContext.HttpContext.Response.BodyWriter.FlushAsync();
        Index lengthIndex = (int)responseStream.Length; // we don't test long responses here, shouldn't be a problem to cast to int
        return responseStream.GetBuffer()[..lengthIndex];
    }

    private async IAsyncEnumerable<IFile> BuildFiles(int count)
    {
        for (var i = 0; i < count; i++)
        {
            await Task.Delay(1);
            yield return new File($"test-file-{i}.txt");
        }
    }

    private IFile BuildSingleFile()
    {
        return new File("test-file.txt");
    }

    private class File : IFile
    {
        public File(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; }

        public string MimeType => "plain/text";

        public async Task Write(PipeWriter writer, CancellationToken ct = default)
        {
            await writer.WriteAsync(Encoding.UTF8.GetBytes("Content of " + Filename), ct);
        }
    }
}
