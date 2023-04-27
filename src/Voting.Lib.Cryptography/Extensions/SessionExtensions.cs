// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Net.Pkcs11Interop.Common;
using Pkcs11Exception = Voting.Lib.Cryptography.Exceptions.Pkcs11Exception;

namespace Net.Pkcs11Interop.HighLevelAPI;

internal static class SessionExtensions
{
    internal static IObjectHandle GetPublicKey(this ISession session, string ckaLabel)
        => session.GetKey(CKO.CKO_PUBLIC_KEY, ckaLabel);

    internal static IObjectHandle GetPrivateKey(this ISession session, string ckaLabel)
        => session.GetKey(CKO.CKO_PRIVATE_KEY, ckaLabel);

    internal static IObjectHandle GetSecretKey(this ISession session, string ckaLabel)
        => session.GetKey(CKO.CKO_SECRET_KEY, ckaLabel);

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
}
