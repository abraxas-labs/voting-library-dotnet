// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Voting.Lib.ProtoValidation;

namespace Voting.Lib.Grpc.Interceptors;

/// <summary>
/// Interceptor which validates all proto request objects with <see cref="ProtoValidator"/>.
/// </summary>
public class RequestProtoValidatorInterceptor : RequestInterceptor
{
    private readonly ProtoValidator _protoValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestProtoValidatorInterceptor"/> class.
    /// </summary>
    /// <param name="protoValidator">Proto validator.</param>
    public RequestProtoValidatorInterceptor(ProtoValidator protoValidator)
    {
        _protoValidator = protoValidator;
    }

    /// <inheritdoc/>
    protected override Task InterceptRequest<TRequest>(TRequest request, ServerCallContext context)
    {
        if (request is not IMessage protoRequest)
        {
            return Task.CompletedTask;
        }

        _protoValidator.Validate(protoRequest);
        return Task.CompletedTask;
    }
}
