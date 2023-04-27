// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
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
    private const ulong DefaultPssSaltLength = 64; // Salt length in bytes, length of the signature.

    private readonly IMechanismParams _defaultAsymmetricMechanismParams = new MechanismParamsFactory().CreateCkRsaPkcsPssParams(
        ConvertUtils.UInt64FromCKM(CKM.CKM_SHA512_RSA_PKCS_PSS),
        ConvertUtils.UInt64FromCKG(CKG.CKG_MGF1_SHA512),
        DefaultPssSaltLength);

    private readonly ILogger<HsmAdapter> _logger;
    private readonly Pkcs11Config _pkcs11Config;
    private readonly Pkcs11InteropFactories _factories = new();
    private readonly ReaderWriterLockSlim _sessionLock = new();

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
    public IEnumerable<byte[]> BulkCreateSignature(ICollection<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
    {
        return BulkCreateSignatureByMechanism(bulkData, pkcs11Config, DefaultAsymmetricMechanism, _defaultAsymmetricMechanismParams);
    }

    /// <inheritdoc />
    public IEnumerable<byte[]> BulkCreateEcdsaSha384Signature(ICollection<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
    {
        return BulkCreateSignatureByMechanism(bulkData, pkcs11Config, CKM.CKM_ECDSA_SHA384);
    }

    /// <inheritdoc />
    public byte[] EncryptAesMac(byte[] cipher, Pkcs11Config pkcs11Config)
    {
        return AesMacPattern(cipher, pkcs11Config, (session, mechanism, secretKey, encryptedCipher) => session.Encrypt(mechanism, secretKey, encryptedCipher));
    }

    /// <inheritdoc />
    public byte[] DecryptAesMac(byte[] encryptedCipher, Pkcs11Config pkcs11Config)
    {
        return AesMacPattern(encryptedCipher, pkcs11Config, (session, mechanism, secretKey, cipher) => session.Decrypt(mechanism, secretKey, cipher));
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

    private IEnumerable<byte[]> BulkCreateSignatureByMechanism(ICollection<byte[]> bulkData, Pkcs11Config? pkcs11Config, CKM mechanismType, IMechanismParams? mechanismParams = null)
    {
        try
        {
            var pkcs11Configuration = pkcs11Config ?? _pkcs11Config;

            if (bulkData.Count == 0)
            {
                throw new CryptographyException("Data to sign may not be empty");
            }

            return WithSession<IEnumerable<byte[]>>(pkcs11Configuration, session =>
            {
                var privateKey = GetPrivateKey(session, pkcs11Configuration);

                IMechanism mechanism;

                if (mechanismParams == null)
                {
                    mechanism = session.Factories.MechanismFactory.Create(mechanismType);
                }
                else
                {
                    mechanism = session.Factories.MechanismFactory.Create(mechanismType, mechanismParams);
                }

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

    private byte[] AesMacPattern(byte[] dataToProcess, Pkcs11Config pkcs11Config, Func<ISession, IMechanism?, IObjectHandle?, byte[], byte[]> processAction)
    {
        try
        {
            if (dataToProcess.Length == 0)
            {
                throw new CryptographyException("dataToProcess may not be empty");
            }

            if (string.IsNullOrEmpty(pkcs11Config.PrivateKeyCkaLabel))
            {
                throw new CryptographyException("PrivateKeyCkaLabel should be set for AesMacPattern");
            }

            if (!string.IsNullOrEmpty(pkcs11Config.PublicKeyCkaLabel))
            {
                throw new CryptographyException("PublicKeyCkaLabel should not be set for AesMacPattern");
            }

            var processedData = WithSession<byte[]>(pkcs11Config, session =>
            {
                var secretKey = GetSecretKey(session, pkcs11Config);
                var mechanism = session.Factories.MechanismFactory.Create(CKM.CKM_AES_MAC);

                return processAction.Invoke(session, mechanism, secretKey, dataToProcess);
            });

            return processedData;
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
            return session.GetSecretKey(pkcs11Config.PrivateKeyCkaLabel)
                ?? throw new Pkcs11Exception("PrivateKey is null");
        }
        catch (Exception ex)
        {
            throw new Pkcs11Exception("Could not load the pkcs11 secret key", ex);
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

    private T WithSession<T>(Pkcs11Config pkcs11Config, Func<ISession, T> f)
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
