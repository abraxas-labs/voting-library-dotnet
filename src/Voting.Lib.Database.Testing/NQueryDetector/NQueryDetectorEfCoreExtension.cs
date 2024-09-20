// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Voting.Lib.Database.Testing.NQueryDetector;

internal sealed class NQueryDetectorEfCoreExtension : IDbContextOptionsExtension
{
    private readonly NQueryDetectorInterceptor _interceptorInstance;
    private DbContextOptionsExtensionInfo? _info;

    internal NQueryDetectorEfCoreExtension(NQueryDetectorInterceptor interceptorInstance)
    {
        _interceptorInstance = interceptorInstance;
    }

    public DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this);

    // the service provider get's rebuilt each time the service provider hash code of the extension info changes.
    // a scope is created for each DbContext.
    public void ApplyServices(IServiceCollection services)
    {
        services.TryAddSingleton(_interceptorInstance);
    }

    public void Validate(IDbContextOptions options)
    {
        // no need to validate anything but implementation is required due to interface.
    }

    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public new NQueryDetectorEfCoreExtension Extension => (NQueryDetectorEfCoreExtension)base.Extension;

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "using VOTING-Lib-NQueryDetector";

        public override int GetServiceProviderHashCode() => Extension._interceptorInstance.GetHashCode();

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            => debugInfo["Voting.Lib:NQueryDetector"] = "1";

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other.Extension is NQueryDetectorEfCoreExtension otherNqueryDetectorEfCoreExtension
                && otherNqueryDetectorEfCoreExtension._interceptorInstance.Equals(Extension._interceptorInstance);
        }
    }
}
