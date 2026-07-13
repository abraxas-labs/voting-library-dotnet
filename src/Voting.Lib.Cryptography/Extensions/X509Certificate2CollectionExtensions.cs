// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace System.Security.Cryptography.X509Certificates;

/// <summary>
/// Extension methods for <see cref="X509Certificate2Collection"/>.
/// </summary>
public static class X509Certificate2CollectionExtensions
{
    /// <summary>
    /// Disposes all certificates in the collection.
    /// </summary>
    /// <param name="collection">The X.509 certificate collection.</param>
    public static void DisposeCertificates(this X509Certificate2Collection collection)
    {
        foreach (var cert in collection)
        {
            cert.Dispose();
        }
    }
}
