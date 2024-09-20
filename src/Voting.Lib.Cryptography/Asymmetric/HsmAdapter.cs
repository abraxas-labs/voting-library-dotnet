// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Extensions.Logging;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.HighLevelAPI.Factories;
using Net.Pkcs11Interop.HighLevelAPI.MechanismParams;
using Voting.Lib.Common;
using Voting.Lib.Cryptography.Configuration;
using Voting.Lib.Cryptography.Exceptions;
using Pkcs11Exception = Voting.Lib.Cryptography.Exceptions.Pkcs11Exception;

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// An adapter implementation to access a hardware security module via PKCS11.
/// The PKCS11 specs and also the dotnet implementation state that it is thread safe (see https://stackoverflow.com/a/44764769/3302887).
/// The loading of the library is synchronized (not thread safe according to the PKCS11 docs).
/// </summary>
public class HsmAdapter : IPkcs11DeviceAdapter, IDisposable
{
    private const CKM DefaultAsymmetricMechanism = CKM.CKM_SHA512_RSA_PKCS_PSS;
    private const ulong DefaultPssSaltSizeInBytes = 64;
    private const int AesGcmNonceSizeInBytes = 12;
    private const int AesGcmTagLengthSizeInBytes = 16;

    private readonly IMechanismParams _defaultAsymmetricMechanismParams = new MechanismParamsFactory().CreateCkRsaPkcsPssParams(
        ConvertUtils.UInt64FromCKM(CKM.CKM_SHA512_RSA_PKCS_PSS),
        ConvertUtils.UInt64FromCKG(CKG.CKG_MGF1_SHA512),
        DefaultPssSaltSizeInBytes);

    private readonly ILogger<HsmAdapter> _logger;
    private readonly Pkcs11Config _pkcs11Config;
    private readonly Pkcs11InteropFactories _factories = new();
    private readonly ReaderWriterLockSlim _sessionLock = new();
    private readonly ConcurrentDictionary<string, ECParameters> _ecdsaPublicKeyParametersCache = new();

    private IPkcs11Library? _library;
    private volatile ISession? _session;
    private volatile bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="HsmAdapter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="pkcs11Config">The PKCS11 configuration.</param>
    public HsmAdapter(ILogger<HsmAdapter> logger, Pkcs11Config pkcs11Config)
    {
        _logger = logger;
        _pkcs11Config = pkcs11Config;
    }

    /// <inheritdoc />
    public byte[] CreateSignature(byte[] data, Pkcs11Config? pkcs11Config = null)
    {
        var bulkData = new List<byte[]> { data };
        return BulkCreateSignatureByMechanism(bulkData, pkcs11Config, DefaultAsymmetricMechanism, _defaultAsymmetricMechanismParams).Single();
    }

    /// <inheritdoc />
    public byte[] CreateEcdsaSha384Signature(byte[] data, Pkcs11Config? pkcs11Config = null)
    {
        var bulkData = new List<byte[]> { data };
        return BulkCreateSignatureByMechanism(bulkData, pkcs11Config, CKM.CKM_ECDSA_SHA384).Single();
    }

    /// <inheritdoc />
    public IReadOnlyList<byte[]> BulkCreateSignature(IEnumerable<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
    {
        return BulkCreateSignatureByMechanism(bulkData, pkcs11Config, DefaultAsymmetricMechanism, _defaultAsymmetricMechanismParams);
    }

    /// <inheritdoc />
    public IReadOnlyList<byte[]> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
    {
        return BulkCreateSignatureByMechanism(bulkData, pkcs11Config, CKM.CKM_ECDSA_SHA384);
    }

    /// <inheritdoc />
    public byte[] EncryptAes(byte[] plainText, Pkcs11Config pkcs11Config)
    {
        try
        {
            if (plainText.Length == 0)
            {
                throw new CryptographyException("Plain text to encrypt may not be empty.");
            }

            if (string.IsNullOrEmpty(pkcs11Config.SecretKeyCkaLabel))
            {
                throw new CryptographyException("Secret key CKA label must be set.");
            }

            return WithSession<byte[]>(pkcs11Config, session =>
            {
                var secretKey = GetSecretKey(session, pkcs11Config);
                var nonce = new byte[AesGcmNonceSizeInBytes];
                RandomNumberGenerator.Fill(nonce);
                var mechanism = CreateAesGcmMechanism(session, nonce);
                var cipherTextWithLeadingTag = session.Encrypt(mechanism, secretKey, plainText);

                var cipherData = new byte[cipherTextWithLeadingTag.Length + nonce.Length];
                Buffer.BlockCopy(cipherTextWithLeadingTag, 0, cipherData, 0, cipherTextWithLeadingTag.Length);
                Buffer.BlockCopy(nonce, 0, cipherData, cipherTextWithLeadingTag.Length, nonce.Length);
                return cipherData;
            });
        }
        catch (Exception ex)
        {
            HandleError(ex);
            throw;
        }
    }

    /// <inheritdoc />
    public byte[] DecryptAes(byte[] cipherText, Pkcs11Config pkcs11Config)
    {
        try
        {
            if (cipherText.Length <= AesGcmNonceSizeInBytes + AesGcmTagLengthSizeInBytes)
            {
                throw new CryptographyException("Cipher text must be greater than the sum of nonce and tag.");
            }

            if (string.IsNullOrEmpty(pkcs11Config.SecretKeyCkaLabel))
            {
                throw new CryptographyException("Secret key CKA label must be set.");
            }

            return WithSession<byte[]>(pkcs11Config, session =>
            {
                var secretKey = GetSecretKey(session, pkcs11Config);
                var nonce = cipherText.AsSpan(^AesGcmNonceSizeInBytes..);
                var cipherTextWithLeadingTag = cipherText.AsSpan(..^AesGcmNonceSizeInBytes);
                var mechanism = CreateAesGcmMechanism(session, nonce);
                return session.Decrypt(mechanism, secretKey, cipherTextWithLeadingTag.ToArray());
            });
        }
        catch (Exception ex)
        {
            HandleError(ex);
            throw;
        }
    }

    /// <inheritdoc />
    public bool VerifySignature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null)
    {
        try
        {
            var pkcs11Configuration = pkcs11Config ?? _pkcs11Config;

            if (data.Length == 0 || signature.Length == 0)
            {
                throw new CryptographyException("Data or signature may not be empty");
            }

            return WithSession(pkcs11Configuration, session =>
            {
                var publicKey = GetPublicKey(session, pkcs11Configuration);

                var mechanism = session.Factories.MechanismFactory.Create(DefaultAsymmetricMechanism, _defaultAsymmetricMechanismParams);
                session.Verify(mechanism, publicKey, data, signature, out var isValid);
                return isValid;
            });
        }
        catch (Exception ex)
        {
            HandleError(ex);
            throw;
        }
    }

    /// <inheritdoc />
    public bool VerifyEcdsaSha384Signature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null)
    {
        return VerifySignatureByMechanism(data, signature, pkcs11Config, CKM.CKM_ECDSA_SHA384);
    }

    /// <inheritdoc />
    public EcdsaPublicKey ExportEcdsaPublicKey(Pkcs11Config? pkcs11Config = null)
    {
        pkcs11Config ??= _pkcs11Config;

        var keyParameters = _ecdsaPublicKeyParametersCache.GetOrAdd(pkcs11Config.PublicKeyCkaLabel, _ => WithSession(pkcs11Config, session =>
        {
            var publicKey = GetPublicKey(session, pkcs11Config);
            var ecdsaAttributes = session.GetAttributeValue(publicKey, new List<CKA> { CKA.CKA_EC_PARAMS, CKA.CKA_EC_POINT });
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
        return new EcdsaPublicKey(ECDsa.Create(keyParameters));
    }

    /// <inheritdoc />
    public bool IsHealthy(Pkcs11Config? pkcs11Config = null)
    {
        try
        {
            var pkcs11Configuration = pkcs11Config ?? _pkcs11Config;
            WithSession(pkcs11Configuration, session => GetPublicKey(session, pkcs11Configuration));
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, ex, "Could not login into the hsm and retrieve a value from its store");
            HandleError();
            return false;
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
            _session?.Dispose();
            _library?.Dispose();
            _defaultAsymmetricMechanismParams.Dispose();
        }
        finally
        {
            _sessionLock.ExitWriteLock();
        }

        _sessionLock.Dispose();
    }

    private IReadOnlyList<byte[]> BulkCreateSignatureByMechanism(IEnumerable<byte[]> bulkData, Pkcs11Config? pkcs11Config, CKM mechanismType, IMechanismParams? mechanismParams = null)
    {
        try
        {
            var pkcs11Configuration = pkcs11Config ?? _pkcs11Config;

            return WithSession(pkcs11Configuration, session =>
            {
                var privateKey = GetPrivateKey(session, pkcs11Configuration);

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

                    signatures.Add(session.Sign(mechanism, privateKey, data));
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

    private bool VerifySignatureByMechanism(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config, CKM mechanismType, IMechanismParams? mechanismParams = null)
    {
        try
        {
            if (data.Length == 0 || signature.Length == 0)
            {
                throw new CryptographyException("Data or signature may not be empty");
            }

            var pkcs11Configuration = pkcs11Config ?? _pkcs11Config;

            return WithSession(pkcs11Configuration, session =>
            {
                var publicKey = GetPublicKey(session, pkcs11Configuration);

                var mechanism = mechanismParams == null
                    ? session.Factories.MechanismFactory.Create(mechanismType)
                    : session.Factories.MechanismFactory.Create(mechanismType, mechanismParams);

                session.Verify(mechanism, publicKey, data, signature, out var isValid);

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

    private ISession GetExistingOrOpenNewSession(Pkcs11Config pkcs11Config)
    {
        if (_session != null)
        {
            return _session;
        }

        _sessionLock.EnterWriteLock();
        if (_session != null)
        {
            _sessionLock.ExitWriteLock();
            return _session;
        }

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
                ?? throw new Pkcs11Exception($"Could not locate the pkcs11 token slot with id '{pkcs11Config.SlotId}'");

            session = slot.OpenSession(SessionType.ReadOnly) ?? throw new Pkcs11Exception("Session is null");
            session.Login(CKU.CKU_USER, pkcs11Config.LoginPin);

            // assign as the very last step
            // since other threads may check on this variable
            // so it needs to be ensured it is initialized completely
            _library = library;
            return _session = session;
        }
        catch (Exception ex)
        {
            session?.Dispose();
            library?.Dispose();
            throw new Pkcs11Exception("Could not open a pkcs11 session", ex);
        }
        finally
        {
            _sessionLock.ExitWriteLock();
        }
    }

    private IObjectHandle GetPrivateKey(ISession session, Pkcs11Config pkcs11Config)
    {
        try
        {
            return session.GetPrivateKey(pkcs11Config.PrivateKeyCkaLabel)
                ?? throw new Pkcs11Exception("PrivateKey is null");
        }
        catch (Exception ex)
        {
            throw new Pkcs11Exception("Could not load the pkcs11 private key", ex);
        }
    }

    private IObjectHandle GetSecretKey(ISession session, Pkcs11Config pkcs11Config)
    {
        try
        {
            return session.GetSecretKey(pkcs11Config.SecretKeyCkaLabel)
                ?? throw new Pkcs11Exception("SecretKeyCkaLabel is null.");
        }
        catch (Exception ex)
        {
            throw new Pkcs11Exception("Could not load the pkcs11 secret key.", ex);
        }
    }

    private IObjectHandle GetPublicKey(ISession session, Pkcs11Config pkcs11Config)
    {
        try
        {
            return session.GetPublicKey(pkcs11Config.PublicKeyCkaLabel)
                ?? throw new Pkcs11Exception("PublicKey is null");
        }
        catch (Exception ex)
        {
            throw new Pkcs11Exception("Could not load the pkcs11 public key", ex);
        }
    }

    private IPkcs11Library LoadLibrary(Pkcs11Config pkcs11Config)
    {
        try
        {
            if (string.IsNullOrEmpty(pkcs11Config.LibraryPath))
            {
                throw new Pkcs11Exception($"{nameof(Pkcs11Config)} {nameof(Pkcs11Config.LibraryPath)} must be set");
            }

            return _factories.Pkcs11LibraryFactory.LoadPkcs11Library(_factories, pkcs11Config.LibraryPath, AppType.MultiThreaded)
                ?? throw new Pkcs11Exception("Library is null");
        }
        catch (Exception ex)
        {
            throw new Pkcs11Exception("Could not load the pkcs11 library", ex);
        }
    }

    private TOut WithSession<TOut>(Pkcs11Config pkcs11Config, Func<ISession, TOut> f)
    {
        _sessionLock.EnterUpgradeableReadLock();
        try
        {
            return f(GetExistingOrOpenNewSession(pkcs11Config));
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
            _session?.Dispose();
            _library?.Dispose();

            _session = null;
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
            Pkcs11Exception pe => pe,
            _ => new Pkcs11Exception($"A exception occured in {nameof(HsmAdapter)}", ex),
        };

        throw mappedEx;
    }
}
