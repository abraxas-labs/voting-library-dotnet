// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
        var results = new List<(bool, string, TMessage)?>();

        foreach (var message in messages)
        {
            try
            {
                Validator.Validate(message);
                results.Add((true, string.Empty, message));
            }
            catch (Exception e)
            {
                results.Add((false, e.Message, message));
            }
        }

        var result = results.Find(x => x!.Value.Item1 != valid);

        if (result != null)
        {
            throw new ValidationException($"{result.Value.Item2}, {result.Value.Item3}");
        }
    }
}
