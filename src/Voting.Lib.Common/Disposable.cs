// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Common;

/// <summary>
/// A wrapper for a disposable action.
/// </summary>
public sealed class Disposable : IDisposable
{
    private readonly Action _onDispose;

    /// <summary>
    /// Initializes a new instance of the <see cref="Disposable"/> class.
    /// </summary>
    /// <param name="onDispose">The action to perform during dispose.</param>
    public Disposable(Action onDispose)
    {
        _onDispose = onDispose;
    }

    /// <summary>
    /// Performs the defined action.
    /// </summary>
    public void Dispose()
        => _onDispose();
}
