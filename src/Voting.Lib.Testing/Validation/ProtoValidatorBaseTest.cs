// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.ProtoValidation;
using Xunit;

namespace Voting.Lib.Testing.Validation;

/// <summary>
/// Base class for tests of proto messages with <see cref="ProtoValidator"/>.
/// </summary>
/// <typeparam name="TMessage">The proto message which gets validated.</typeparam>
public abstract class ProtoValidatorBaseTest<TMessage> : IAsyncDisposable
    where TMessage : IMessage, new()
{
    private readonly ServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtoValidatorBaseTest{TMessage}"/> class.
    /// </summary>
    protected ProtoValidatorBaseTest()
    {
        _serviceProvider = new ServiceCollection()
            .AddProtoValidators()
            .BuildServiceProvider();

        Validator = _serviceProvider.GetRequiredService<ProtoValidator>();
    }

    /// <summary>
    /// Gets the proto validator.
    /// </summary>
    protected ProtoValidator Validator { get; }

    /// <summary>
    /// Checks whether all <see cref="OkMessages"/> are valid.
    /// </summary>
    [Fact]
    public void ValidateOk() => AssertValidations(OkMessages(), true);

    /// <summary>
    /// Checks whether all <see cref="NotOkMessages"/> are invalid.
    /// </summary>
    [Fact]
    public void ValidateNotOk() => AssertValidations(NotOkMessages(), false);

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets an enumerable with valid messages.
    /// </summary>
    /// <returns>An enumerable with valid messages.</returns>
    protected abstract IEnumerable<TMessage> OkMessages();

    /// <summary>
    /// Gets an enumerable with invalid messages.
    /// </summary>
    /// <returns>An enumerable with invalid messages.</returns>
    protected abstract IEnumerable<TMessage> NotOkMessages();

    private void AssertValidations(IEnumerable<TMessage> messages, bool valid)
    {
        var results = new List<bool>();

        foreach (var message in messages)
        {
            try
            {
                Validator.Validate(message);
                results.Add(true);
            }
            catch (Exception)
            {
                results.Add(false);
            }
        }

        results.All(x => x == valid).Should().BeTrue();
    }
}
