// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Net.Pkcs11Interop.Common;
using Voting.Lib.Cryptography.Pkcs11.Configuration;
using Pkcs11Exception = Voting.Lib.Cryptography.Pkcs11.Exceptions.Pkcs11Exception;

namespace Net.Pkcs11Interop.HighLevelAPI;

internal static class SessionExtensions
{
    internal static IObjectHandle GetKey(this ISession session, KeyType keyType, string ckaLabel)
    {
        var ckaKeyType = GetCkoKeyType(keyType);
        return session.GetKey(ckaKeyType, ckaLabel);
    }

    internal static bool KeyExists(this ISession session, KeyType keyType, string ckaLabel)
    {
        var ckaKeyType = GetCkoKeyType(keyType);
        return session.KeyExists(ckaKeyType, ckaLabel);
    }

    private static bool KeyExists(this ISession session, CKO ckaKeyType, string ckaLabel)
    {
        var attributes = new List<IObjectAttribute>
            {
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, ckaKeyType),
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true),
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, ckaLabel),
            };

        var objects = session.FindAllObjects(attributes);
        return objects.Count > 0;
    }

    private static IObjectHandle GetKey(this ISession session, CKO ckaKeyType, string ckaLabel)
    {
        var attributes = new List<IObjectAttribute>
            {
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, ckaKeyType),
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true),
                session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, ckaLabel),
            };

        var objects = session.FindAllObjects(attributes);

        return objects.Count switch
        {
            1 => objects[0],
            0 => throw new Pkcs11Exception($"{ckaKeyType} '{ckaLabel}' not found."),
            _ => throw new Pkcs11Exception($"More than one {ckaKeyType} with label '{ckaLabel}' were found. Use a more specific identifier."),
        };
    }

    private static CKO GetCkoKeyType(KeyType keyType)
    {
        var ckaKeyType = keyType switch
        {
            KeyType.PrivateKey => CKO.CKO_PRIVATE_KEY,
            KeyType.PublicKey => CKO.CKO_PUBLIC_KEY,
            KeyType.SecretKey => CKO.CKO_SECRET_KEY,
            _ => throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null),
        };
        return ckaKeyType;
    }
}
