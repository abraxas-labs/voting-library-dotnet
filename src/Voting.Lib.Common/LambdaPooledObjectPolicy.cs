// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.ObjectPool;

namespace Voting.Lib.Common;

/// <summary>
/// A simple <see cref="IPooledObjectPolicy{T}"/> implementation accepting function parameters.
/// </summary>
/// <typeparam name="T">The type of the pooled object.</typeparam>
public class LambdaPooledObjectPolicy<T> : PooledObjectPolicy<T>
    where T : class
{
    private readonly Func<T> _activator;
    private readonly Action<T> _reset;

    /// <summary>
    /// Initializes a new instance of the <see cref="LambdaPooledObjectPolicy{T}"/> class.
    /// </summary>
    /// <param name="activator">The function used to activate a new object.</param>
    /// <param name="reset">The reset function called before returning an object to the pool.</param>
    public LambdaPooledObjectPolicy(Func<T> activator, Action<T> reset)
    {
        _activator = activator;
        _reset = reset;
    }

    /// <inheritdoc />
    public override T Create()
        => _activator();

    /// <inheritdoc />
    public override bool Return(T obj)
    {
        _reset(obj);
        return true;
    }
}
