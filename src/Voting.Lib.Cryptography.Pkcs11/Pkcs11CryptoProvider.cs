// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.HighLevelAPI.Factories;
using Net.Pkcs11Interop.HighLevelAPI.MechanismParams;
using Voting.Lib.Common;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Exceptions;
using Voting.Lib.Cryptography.Pkcs11.Configuration;
using Voting.Lib.Cryptography.Pkcs11.Exceptions;
using Pkcs11Exception = Net.Pkcs11Interop.Common.Pkcs11Exception;

namespace Voting.Lib.Cryptography.Pkcs11;

/// <summary>
/// An adapter implementation to access a hardware security module via PKCS11.
/// The PKCS11 specs and also the dotnet implementation state that it is thread safe (see https://stackoverflow.com/a/44764769/3302887).
/// The loading of the library is synchronized (not thread safe according to the PKCS11 docs).
/// </summary>
public class Pkcs11CryptoProvider : ICryptoProvider, IDisposable
{
    private const CKM DefaultAsymmetricMechanism = CKM.CKM_SHA512_RSA_PKCS_PSS;
    private const ulong DefaultPssSaltSizeInBytes = 64;
    private const int AesGcmNonceSizeInBytes = 12;
    private const int AesGcmTagLengthSizeInBytes = 16;
    private const int CkuCsUtimacoGeneric = 0x83;

    private readonly IMechanismParams _defaultAsymmetricMechanismParams = new MechanismParamsFactory().CreateCkRsaPkcsPssParams(
        ConvertUtils.UInt64FromCKM(CKM.CKM_SHA512_RSA_PKCS_PSS),
        ConvertUtils.UInt64FromCKG(CKG.CKG_MGF1_SHA512),
        DefaultPssSaltSizeInBytes);

    private readonly ILogger<Pkcs11CryptoProvider> _logger;
    private readonly Pkcs11Config _pkcs11Config;
    private readonly Pkcs11InteropFactories _factories = new();
    private readonly ReaderWriterLockSlim _sessionLock = new();
    private readonly ConcurrentDictionary<string, ECParameters> _ecdsaPublicKeyParametersCache = new();
    private readonly HashSet<(ulong SlotId, HsmUserType UserType)> _loggedInSessions = [];
    private readonly Dictionary<(ulong SlotId, HsmUserType UserType, bool ReadWrite), ISession> _sharedSessions = [];

    private IPkcs11Library? _library;

    private volatile bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pkcs11CryptoProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="pkcs11Config">The PKCS11 configuration.</param>
    public Pkcs11CryptoProvider(ILogger<Pkcs11CryptoProvider> logger, Pkcs11Config pkcs11Config)
    {
        _logger = logger;
        _pkcs11Config = pkcs11Config;
    }

    /// <inheritdoc />
    public Task<string> GenerateAesSecretKey(string keyLabel)
    {
        GenerateSecretKey(CKM.CKM_AES_KEY_GEN, keyLabel);
        return Task.FromResult(keyLabel);
    }

    /// <inheritdoc />
    public Task<string> GetAesSecretKeyId(string keyLabel) => Task.FromResult(keyLabel);

    /// <inheritdoc />
    public Task DeleteAesSecretKey(string keyId) => DeleteSecretKey(keyId);

    /// <inheritdoc />
    public Task<string> GenerateMacSecretKey(string keyLabel)
    {
        GenerateSecretKey(CKM.CKM_GENERIC_SECRET_KEY_GEN, keyLabel);
        return Task.FromResult(keyLabel);
    }

    /// <inheritdoc />
    public Task<string> GetMacSecretKeyId(string keyLabel) => Task.FromResult(keyLabel);

    /// <inheritdoc />
    public Task DeleteMacSecretKey(string keyId) => DeleteSecretKey(keyId);

    /// <inheritdoc />
    public Task<byte[]> CreateSignature(byte[] data, string keyId)
    {
        var result = CreateSignature(data, keyId, KeyType.PrivateKey, DefaultAsymmetricMechanism, _defaultAsymmetricMechanismParams);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<byte[]> CreateEcdsaSha384Signature(byte[] data, string keyId)
    {
        var result = CreateSignature(data, keyId, KeyType.PrivateKey, CKM.CKM_ECDSA_SHA384);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<byte[]> CreateHmacSha256(byte[] data, string keyId)
    {
        var result = CreateSignature(data, keyId, KeyType.SecretKey, CKM.CKM_SHA256_HMAC);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateSignature(IEnumerable<byte[]> bulkData, string keyId)
    {
        var result = BulkCreateSignatureByMechanism(bulkData, keyId, KeyType.PrivateKey, DefaultAsymmetricMechanism, _defaultAsymmetricMechanismParams);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, string keyId)
    {
        var result = BulkCreateSignatureByMechanism(bulkData, keyId, KeyType.PrivateKey, CKM.CKM_ECDSA_SHA384);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateHmacSha256(IEnumerable<byte[]> bulkData, string keyId)
    {
        var result = BulkCreateSignatureByMechanism(bulkData, keyId, KeyType.SecretKey, CKM.CKM_SHA256_HMAC);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<byte[]> EncryptAesGcm(byte[] plainText, string keyId)
    {
        try
        {
            if (plainText.Length == 0)
            {
                throw new Exceptions.Pkcs11Exception("Plain text to encrypt may not be empty.");
            }

            var result = WithSession<byte[]>(_pkcs11Config, session =>
            {
                var secretKey = session.GetKey(KeyType.SecretKey, keyId);
                var nonce = new byte[AesGcmNonceSizeInBytes];
                RandomNumberGenerator.Fill(nonce);
                var mechanism = CreateAesGcmMechanism(session, nonce);
                var cipherTextWithLeadingTag = session.Encrypt(mechanism, secretKey, plainText);

                var cipherData = new byte[cipherTextWithLeadingTag.Length + nonce.Length];
                Buffer.BlockCopy(cipherTextWithLeadingTag, 0, cipherData, 0, cipherTextWithLeadingTag.Length);
                Buffer.BlockCopy(nonce, 0, cipherData, cipherTextWithLeadingTag.Length, nonce.Length);
                return cipherData;
            });
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            HandleError(ex);
            throw;
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkEncryptAesGcm(IEnumerable<byte[]> bulkPlainText, string keyId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<byte[]> DecryptAesGcm(byte[] cipherText, string keyId)
    {
        try
        {
            if (cipherText.Length <= AesGcmNonceSizeInBytes + AesGcmTagLengthSizeInBytes)
            {
                throw new CryptographyException("Cipher text must be greater than the sum of nonce and tag.");
            }

            var result = WithSession<byte[]>(_pkcs11Config, session =>
            {
                var secretKey = session.GetKey(KeyType.SecretKey, keyId);
                var nonce = cipherText.AsSpan(^AesGcmNonceSizeInBytes..);
                var cipherTextWithLeadingTag = cipherText.AsSpan(..^AesGcmNonceSizeInBytes);
                var mechanism = CreateAesGcmMechanism(session, nonce);
                return session.Decrypt(mechanism, secretKey, cipherTextWithLeadingTag.ToArray());
            });
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            HandleError(ex);
            throw;
        }
    }

    /// <inheritdoc />
    public Task<bool> VerifySignature(byte[] data, byte[] signature, string keyId)
    {
        var result = VerifySignatureByMechanism(data, signature, keyId, KeyType.PublicKey, DefaultAsymmetricMechanism, _defaultAsymmetricMechanismParams);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<bool> VerifyEcdsaSha384Signature(byte[] data, byte[] signature, string keyId)
    {
        var result = VerifySignatureByMechanism(data, signature, keyId, KeyType.PublicKey, CKM.CKM_ECDSA_SHA384);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<bool> VerifyHmacSha256(byte[] data, byte[] hash, string keyId)
    {
        var result = VerifySignatureByMechanism(data, hash, keyId, KeyType.SecretKey, CKM.CKM_SHA256_HMAC);
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<EcdsaPublicKey> ExportEcdsaPublicKey(string keyId)
    {
        var keyParameters = _ecdsaPublicKeyParametersCache.GetOrAdd(keyId, _ => WithSession(_pkcs11Config, session =>
        {
            var publicKey = session.GetKey(KeyType.PublicKey, keyId);
            var ecdsaAttributes = session.GetAttributeValue(publicKey, [CKA.CKA_EC_PARAMS, CKA.CKA_EC_POINT]);
            var ecdsaAttributeParams = ecdsaAttributes[0];
            var ecdsaAttributePoint = ecdsaAttributes[1];

            if (ecdsaAttributeParams.CannotBeRead)
            {
                throw new Exception("ECDSA public key object attribute <CKA_EC_PARAMS> cannot be exported");
            }

            if (ecdsaAttributePoint.CannotBeRead)
            {
                throw new Exception("ECDSA public key object attribute <CKA_EC_POINT> cannot be exported");
            }

            var curve = EcdsaUtil.MapFromHsmNamedCurve(ecdsaAttributeParams.GetValueAsString());
            return new ECParameters
            {
                Curve = curve,
                Q = EcdsaUtil.DecodeEcPoint(ecdsaAttributePoint.GetValueAsByteArray(), curve),
            };
        }));

        // ECDsa may not be thread safe depending on the platform / implementation.
        var result = new EcdsaPublicKey(ECDsa.Create(keyParameters));
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task<bool> IsHealthy(string? keyId = null)
    {
        try
        {
            WithSession(_pkcs11Config, _ => default(string));
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, ex, "Could not login into the pkcs#11 device and retrieve a value from its store");
            HandleError();
            return Task.FromResult(false);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    /// <param name="disposing">Whether native resources should be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
        {
            return;
        }

        _sessionLock.EnterWriteLock();
        try
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            foreach (var session in _sharedSessions.Values) { session.Dispose(); }
            _sharedSessions.Clear();
            _loggedInSessions.Clear();
            _library?.Dispose();
            _defaultAsymmetricMechanismParams.Dispose();
        }
        finally
        {
            _sessionLock.ExitWriteLock();
        }

        _sessionLock.Dispose();
    }

    private Task DeleteSecretKey(string ckaLabel)
    {
        WithReadWriteSession(_pkcs11Config, session =>
        {
            var key = session.GetKey(KeyType.SecretKey, ckaLabel);
            session.DestroyObject(key);
            return true;
        });
        return Task.CompletedTask;
    }

    private byte[] CreateSignature(byte[] data, string ckaLabel, KeyType keyType, CKM mechanism, IMechanismParams? mechanismParams = null)
    {
        var bulkData = new List<byte[]> { data };
        return BulkCreateSignatureByMechanism(bulkData, ckaLabel, keyType, mechanism, mechanismParams).Single();
    }

    private IReadOnlyList<byte[]> BulkCreateSignatureByMechanism(
        IEnumerable<byte[]> bulkData,
        string ckaLabel,
        KeyType keyType,
        CKM mechanismType,
        IMechanismParams? mechanismParams = null)
    {
        try
        {
            return WithSession(_pkcs11Config, session =>
            {
                var key = session.GetKey(keyType, ckaLabel);
                var mechanism = mechanismParams == null
                    ? session.Factories.MechanismFactory.Create(mechanismType)
                    : session.Factories.MechanismFactory.Create(mechanismType, mechanismParams);
                var signatures = new List<byte[]>();
                foreach (var data in bulkData)
                {
                    if (data.Length == 0)
                    {
                        throw new CryptographyException("Data to sign may not be empty");
                    }

                    signatures.Add(session.Sign(mechanism, key, data));
                }

                return signatures;
            });
        }
        catch (Exception ex)
        {
            HandleError(ex);
            throw;
        }
    }

    private bool VerifySignatureByMechanism(byte[] data, byte[] signature, string ckaLabel, KeyType keyType, CKM mechanismType, IMechanismParams? mechanismParams = null)
    {
        try
        {
            if (data.Length == 0 || signature.Length == 0)
            {
                throw new CryptographyException("Data or signature may not be empty");
            }

            return WithSession(_pkcs11Config, session =>
            {
                var key = session.GetKey(keyType, ckaLabel);
                var mechanism = mechanismParams == null
                    ? session.Factories.MechanismFactory.Create(mechanismType)
                    : session.Factories.MechanismFactory.Create(mechanismType, mechanismParams);
                session.Verify(mechanism, key, data, signature, out var isValid);
                return isValid;
            });
        }
        catch (Exception ex)
        {
            HandleError(ex);
            throw;
        }
    }

    private ICkGcmParams CreateGcmParameters(ISession session, ReadOnlySpan<byte> nonce)
        => session
            .Factories
            .MechanismParamsFactory
            .CreateCkGcmParams(nonce.ToArray(), AesGcmNonceSizeInBytes * 8, null, AesGcmTagLengthSizeInBytes * 8);

    private IMechanism CreateAesGcmMechanism(ISession session, ReadOnlySpan<byte> nonce)
        => session.Factories.MechanismFactory.Create(CKM.CKM_AES_GCM, CreateGcmParameters(session, nonce));

    private ISession GetOrOpenSharedSession(Pkcs11Config pkcs11Config, bool readWrite)
    {
        var loginKey = (pkcs11Config.SlotId, pkcs11Config.LoginUserType);
        var sessionKey = (pkcs11Config.SlotId, pkcs11Config.LoginUserType, readWrite);

        if (_sharedSessions.TryGetValue(sessionKey, out var existing))
        {
            return existing;
        }

        _sessionLock.EnterWriteLock();

        if (_sharedSessions.TryGetValue(sessionKey, out existing))
        {
            _sessionLock.ExitWriteLock();
            return existing;
        }

        try
        {
            ISession session;
            if (!_loggedInSessions.Contains(loginKey))
            {
                session = OpenNewSession(pkcs11Config, readWrite);
                _loggedInSessions.Add(loginKey);
            }
            else
            {
                session = OpenAdditionalSession(pkcs11Config, readWrite);
            }

            _sharedSessions.Add(sessionKey, session);
            return session;
        }
        finally
        {
            _sessionLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Opens a new PKCS#11 session for a given slot with specified access flags.
    /// A User-PIN allows only one active login per slot. Parallel logins or sessions as the same user will fail with CKR_USER_ALREADY_LOGGED_IN.
    /// This stricter behavior aligns with PKCS#11 R3 and differs from earlier versions like v4.45.
    /// </summary>
    private ISession OpenNewSession(Pkcs11Config pkcs11Config, bool readWrite)
    {
        IPkcs11Library? library = null;
        ISession? session = null;
        try
        {
            library = LoadLibrary(pkcs11Config);
            var slots = library.GetSlotList(SlotsType.WithOrWithoutTokenPresent);

            foreach (var slotWithOrWithoutToken in slots)
            {
                _logger.LogDebug(SecurityLogging.SecurityEventId, "Slot with or without Token found with slot id: {SlotId}", slotWithOrWithoutToken.SlotId);
            }

            var slot = slots.Find(s => s.SlotId == pkcs11Config.SlotId)
                ?? throw new Exceptions.Pkcs11Exception($"Could not locate the pkcs11 token slot with id '{pkcs11Config.SlotId}'");

            session = slot.OpenSession(readWrite ? SessionType.ReadWrite : SessionType.ReadOnly)
                ?? throw new Exceptions.Pkcs11Exception("Session is null");

            _logger.LogInformation(SecurityLogging.SecurityEventId, "Login to a new pkcs11 session with slot id {SlotId} and user {UserId}", slot.SlotId, pkcs11Config.LoginUserType);
            session.Login(MapUserType(pkcs11Config.LoginUserType), pkcs11Config.LoginPin);

            _library = library;
            return session;
        }
        catch (Exception ex)
        {
            session?.Dispose();
            library?.Dispose();
            throw new Exceptions.Pkcs11Exception("Could not open a pkcs11 session", ex);
        }
    }

    private ISession OpenAdditionalSession(Pkcs11Config pkcs11Config, bool readWrite)
    {
        var library = _library
            ?? throw new InvalidOperationException("Pkcs11 Library must be loaded before opening an additional session from an existing logged in session.");

        var slot = library.GetSlotList(SlotsType.WithOrWithoutTokenPresent)
                          .Find(s => s.SlotId == pkcs11Config.SlotId)
            ?? throw new Exceptions.Pkcs11Exception($"Slot {pkcs11Config.SlotId} not found");

        _logger.LogInformation(SecurityLogging.SecurityEventId, "Open an additional pkcs11 session with slot id {SlotId} and user {UserId}", slot.SlotId, pkcs11Config.LoginUserType);

        return slot.OpenSession(readWrite ? SessionType.ReadWrite : SessionType.ReadOnly)
            ?? throw new Exceptions.Pkcs11Exception("Failed to create an additional session.");
    }

    private void GenerateSecretKey(CKM keyMechanism, string keyId)
    {
        if (string.IsNullOrEmpty(keyId))
        {
            throw new ArgumentException($"{nameof(keyId)} must not be empty");
        }

        WithReadWriteSession(_pkcs11Config, session =>
        {
            if (session.KeyExists(KeyType.SecretKey, keyId))
            {
                throw new Pkcs11KeyAlreadyExistsException(CKO.CKO_SECRET_KEY, keyId);
            }

            var mechanism = session.Factories.MechanismFactory.Create(keyMechanism);

            var attributes = new List<IObjectAttribute>
            {
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, keyId),
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_VALUE_LEN, 32),
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true), // Required to persist the Key.
            };

            return session.GenerateKey(mechanism, attributes);
        });
    }

    private IPkcs11Library LoadLibrary(Pkcs11Config pkcs11Config)
    {
        try
        {
            if (string.IsNullOrEmpty(pkcs11Config.LibraryPath))
            {
                throw new Exceptions.Pkcs11Exception($"{nameof(Pkcs11Config)} {nameof(Pkcs11Config.LibraryPath)} must be set");
            }

            return _factories.Pkcs11LibraryFactory.LoadPkcs11Library(_factories, pkcs11Config.LibraryPath, AppType.MultiThreaded)
                ?? throw new Exceptions.Pkcs11Exception("Library is null");
        }
        catch (Exception ex) when (ex is not Pkcs11Exception)
        {
            throw new Exceptions.Pkcs11Exception($"Could not load the pkcs11 library {pkcs11Config.LibraryPath}", ex);
        }
    }

    private TOut WithSession<TOut>(Pkcs11Config pkcs11Config, Func<ISession, TOut> f)
    {
        _sessionLock.EnterUpgradeableReadLock();
        try
        {
            return f(GetOrOpenSharedSession(pkcs11Config, false));
        }
        finally
        {
            _sessionLock.ExitUpgradeableReadLock();
        }
    }

    private void WithReadWriteSession<TOut>(Pkcs11Config pkcs11Config, Func<ISession, TOut> f)
    {
        _sessionLock.EnterUpgradeableReadLock();
        try
        {
            f(GetOrOpenSharedSession(pkcs11Config, true));
        }
        finally
        {
            _sessionLock.ExitUpgradeableReadLock();
        }
    }

    private void HandleError(Exception? ex = null)
    {
        _sessionLock.EnterWriteLock();
        try
        {
            foreach (var session in _sharedSessions.Values) { session.Dispose(); }
            _sharedSessions.Clear();
            _loggedInSessions.Clear();
            _library?.Dispose();
            _library = null;
        }
        finally
        {
            _sessionLock.ExitWriteLock();
        }

        if (ex == null)
        {
            return;
        }

        var mappedEx = ex switch
        {
            Exceptions.Pkcs11Exception pe => pe,
            _ => new Exceptions.Pkcs11Exception($"A exception occured in {nameof(Pkcs11CryptoProvider)}", ex),
        };

        throw mappedEx;
    }

    private CKU MapUserType(HsmUserType userType)
    {
        switch (userType)
        {
            case HsmUserType.SecurityOfficer:
                return CKU.CKU_SO;
            case HsmUserType.User:
                return CKU.CKU_USER;
            case HsmUserType.ContextSpecific:
                return CKU.CKU_CONTEXT_SPECIFIC;
            case HsmUserType.UtimacoGeneric:
                return (CKU)CkuCsUtimacoGeneric;
            default:
                throw new Exceptions.Pkcs11Exception($"The provided user type '{userType}' couldn't be mapped.");
        }
    }
}
