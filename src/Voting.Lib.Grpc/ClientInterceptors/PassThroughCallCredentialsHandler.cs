// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Grpc.Configuration;
using Voting.Lib.Iam.Store;

namespace Voting.Lib.Grpc.ClientInterceptors;

internal class PassThroughCallCredentialsHandler
{
    private readonly PassThroughCallCredentialsConfig _config;

    internal PassThroughCallCredentialsHandler(PassThroughCallCredentialsConfig config)
    {
        _config = config;
    }

    internal Task Handle(AuthInterceptorContext ctx, Metadata metadata, IServiceProvider sp)
    {
        var auth = sp.GetRequiredService<IAuth>();
        if (!_config.ThrowIfUnauthenticated && !auth.IsAuthenticated)
        {
            return Task.CompletedTask;
        }

        metadata.Add(_config.TokenHeaderName, _config.TokenHeaderValuePrefix + auth.AccessToken);
        metadata.Add(_config.TenantHeaderName, auth.Tenant.Id);
        return Task.CompletedTask;
    }
}
