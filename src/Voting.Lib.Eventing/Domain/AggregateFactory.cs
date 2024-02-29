// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Voting.Lib.Eventing.Domain;

/// <inheritdoc/>
public class AggregateFactory : IAggregateFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateFactory"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public AggregateFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public TAggregate New<TAggregate>()
        where TAggregate : BaseEventSourcingAggregate
    {
        return _serviceProvider.GetRequiredService<TAggregate>();
    }
}
