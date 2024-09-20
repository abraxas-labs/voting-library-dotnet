// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Test;

namespace Voting.Lib.Database.Testing.Test.NQueryDetector;

public class NQueryDetectorTestContext : TestDbContext
{
    private readonly bool _useGlobalInterceptor;

    public NQueryDetectorTestContext(bool useGlobalInterceptor)
    {
        _useGlobalInterceptor = useGlobalInterceptor;
    }

    public NQueryDetectorTestContext()
        : this(true)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddNQueryDetector(_useGlobalInterceptor);
    }
}
