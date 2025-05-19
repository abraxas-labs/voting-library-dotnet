// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Class to access the current <see cref="EventProcessorContext"/> in a scoped event processor.
/// </summary>
public class EventProcessorContextAccessor
{
    private EventProcessorContext? _context;

    /// <summary>
    /// Gets the current context.
    /// </summary>
    public EventProcessorContext Context => _context ?? throw new InvalidOperationException("Context is not initialized");

    internal void SetContext(EventProcessorContext context)
    {
        if (_context != null)
        {
            throw new InvalidOperationException("Context already initialized");
        }

        _context = context;
    }
}
