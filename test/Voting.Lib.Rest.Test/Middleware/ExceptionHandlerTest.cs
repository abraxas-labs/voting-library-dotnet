// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging.Abstractions;
using Voting.Lib.Rest.Middleware;
using Xunit;

namespace Voting.Lib.Rest.Test.Middleware;

public class ExceptionHandlerTest
{
    [Fact]
    public async Task OkEndpointShouldReturn()
    {
        var result = await Run<TestExceptionHandler, string>(client => client.GetStringAsync("/ok"));
        result.Should().Be("ok");
    }

    [Fact]
    public Task NoDetailedErrorsNoExposedExceptionShouldOnlyMapStatusCode()
    {
        return AssertExceptionResponse<TestExceptionHandler>(
            client => client.GetAsync("/exception"),
            "{'title':'Exception','status':424}",
            HttpStatusCode.FailedDependency);
    }

    [Fact]
    public Task NoDetailedErrorsExposedExceptionShouldMapStatusCodeAndExceptionType()
    {
        return AssertExceptionResponse<TestExceptionHandler>(
            client => client.GetAsync("/exposed-exception"),
            "{'title':'ExposedTestException','status':409}",
            HttpStatusCode.Conflict);
    }

    [Fact]
    public Task DetailedErrorsShouldExposeDetails()
    {
        return AssertExceptionResponse<TestDetailedExceptionHandler>(
            client => client.GetAsync("/exception"),
            "{'title':'TestException','status':424,'detail':'test message'}",
            HttpStatusCode.FailedDependency);
    }

    [Fact]
    public Task DefaultHandlerConfigurationShouldNotExposeDetails()
    {
        return AssertExceptionResponse<DefaultExceptionHandler>(
            client => client.GetAsync("/exception"),
            "{'title':'Exception','status':500}",
            HttpStatusCode.InternalServerError);
    }

    private async Task AssertExceptionResponse<TMiddleware>(Func<HttpClient, Task<HttpResponseMessage>> runTest, string expectedContent, HttpStatusCode expectedStatusCode)
    {
        var response = await Run<TMiddleware, HttpResponseMessage>(runTest);
        response.StatusCode.Should().Be(expectedStatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be(expectedContent.Replace('\'', '"'));
    }

    private async Task<TResponse> Run<TMiddleware, TResponse>(Func<HttpClient, Task<TResponse>> runTest)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        await using var app = builder.Build();
        app.UseMiddleware<TMiddleware>();
        app.MapGet("/ok", () => "ok");
        app.MapGet("/exposed-exception", new Func<string>(() => throw new ExposedTestException()));
        app.MapGet("/exception", new Func<string>(() => throw new TestException()));
        await app.StartAsync();
        return await runTest(app.GetTestClient());
    }

    private class DefaultExceptionHandler : ExceptionHandler
    {
        public DefaultExceptionHandler(RequestDelegate next)
            : base(next, NullLogger<ExceptionHandler>.Instance)
        {
        }
    }

    private class TestDetailedExceptionHandler : TestExceptionHandler
    {
        public TestDetailedExceptionHandler(RequestDelegate next)
            : base(next, true)
        {
        }
    }

    private class TestExceptionHandler : ExceptionHandler
    {
        public TestExceptionHandler(RequestDelegate next)
            : this(next, false)
        {
        }

        protected TestExceptionHandler(RequestDelegate next, bool enableDetailedErrors)
            : base(next, NullLogger<ExceptionHandler>.Instance, enableDetailedErrors)
        {
        }

        protected override bool ExposeExceptionType(Exception ex)
            => ex is ExposedTestException;

        protected override int MapExceptionToStatus(Exception ex)
        {
            return ex switch
            {
                TestException => StatusCodes.Status424FailedDependency,
                ExposedTestException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError,
            };
        }
    }

    private class TestException : Exception
    {
        public TestException()
            : base("test message")
        {
        }
    }

    private class ExposedTestException : Exception
    {
        public ExposedTestException()
            : base("exposed test message")
        {
        }
    }
}
