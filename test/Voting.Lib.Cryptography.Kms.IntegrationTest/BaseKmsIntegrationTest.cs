// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Cryptography.Kms.Configuration;
using Voting.Lib.Cryptography.Kms.Exceptions;
using Xunit;

namespace Voting.Lib.Cryptography.Kms.IntegrationTest;

public class BaseKmsIntegrationTest : IAsyncLifetime
{
    private const string KeyPrefix = "KGitLabVotingLibIntegrationTest";

    private readonly ServiceProvider _serviceProvider;
    private readonly List<string> _macKeys = new();
    private readonly List<string> _aesKeys = new();
    private readonly List<string> _ecdsaSha384Keys = new();

    protected BaseKmsIntegrationTest()
    {
        _serviceProvider = new ServiceCollection()
            .AddVotingLibKms(new KmsConfig
            {
                Endpoint = new Uri("https://pre.kmp.abraxas-apis.ch/api/"),
                Username = Environment.GetEnvironmentVariable("KMS_USERNAME") ?? throw new InvalidOperationException("KMS_USERNAME not set"),
                Password = Environment.GetEnvironmentVariable("KMS_PASSWORD") ?? throw new InvalidOperationException("KMS_PASSWORD not set"),
                AesKeyLabels = new Dictionary<string, string> { { "name", KeyPrefix } },
                MacKeyLabels = new Dictionary<string, string> { { "name", KeyPrefix } },
                EcdsaSha384KeyLabels = new Dictionary<string, string> { { "name", KeyPrefix } },
            })
            .BuildServiceProvider();
        CryptoProvider = _serviceProvider.GetRequiredService<ICryptoProvider>();
    }

    protected ICryptoProvider CryptoProvider { get; }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        foreach (var keyId in _macKeys)
        {
            await CryptoProvider.DeleteMacSecretKey(keyId);
        }

        foreach (var keyId in _aesKeys)
        {
            await CryptoProvider.DeleteAesSecretKey(keyId);
        }

        foreach (var keyId in _ecdsaSha384Keys)
        {
            await CryptoProvider.DeleteEcdsaSha384Key(keyId);
        }

        await _serviceProvider.DisposeAsync();
    }

    protected async Task<string> GetOrCreateMacKey(string name, bool temp = false)
    {
        try
        {
            return await CreateMacKey(name, temp);
        }
        catch (KmsKeyAlreadyExistsException)
        {
            return await CryptoProvider.GetMacSecretKeyId(BuildKeyName(name, temp));
        }
    }

    protected async Task<string> CreateMacKey(string name, bool temp = true)
    {
        var keyName = BuildKeyName(name, temp);
        var id = await CryptoProvider.GenerateMacSecretKey(keyName);

        if (temp)
        {
            _macKeys.Add(id);
        }

        return id;
    }

    protected async Task<string> GetOrCreateAesKey(string name, bool temp = false)
    {
        try
        {
            return await CreateAesKey(name, temp);
        }
        catch (KmsKeyAlreadyExistsException)
        {
            return await CryptoProvider.GetAesSecretKeyId(BuildKeyName(name, temp));
        }
    }

    protected async Task<string> GetOrCreateEcdsaSha384Key(string name, bool temp = false)
    {
        try
        {
            return await CreateEcdsaSha384Key(name, temp);
        }
        catch (KmsKeyAlreadyExistsException)
        {
            return await CryptoProvider.GetEcdsaSha384SecretKeyId(BuildKeyName(name, temp));
        }
    }

    protected async Task<string> CreateAesKey(string name, bool temp = true)
    {
        var keyName = BuildKeyName(name, temp);
        var id = await CryptoProvider.GenerateAesSecretKey(keyName);

        if (temp)
        {
            _aesKeys.Add(id);
        }

        return id;
    }

    protected async Task<string> CreateEcdsaSha384Key(string name, bool temp = true)
    {
        var keyName = BuildKeyName(name, temp);
        var id = await CryptoProvider.GenerateEcdsaSha384SecretKey(keyName);

        if (temp)
        {
            _ecdsaSha384Keys.Add(id);
        }

        return id;
    }

    private static string BuildKeyName(string name, bool temp)
    {
        var keyName = KeyPrefix + name;
        if (temp)
        {
            keyName += $"Tmp{Guid.NewGuid()}";
        }
        else
        {
            keyName += "DoNotDelete";
        }

        return keyName;
    }
}
