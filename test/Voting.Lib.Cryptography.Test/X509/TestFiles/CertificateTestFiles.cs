// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;

namespace Voting.Lib.Cryptography.Test.X509.TestFiles;

public static class CertificateTestFiles
{
    public static string FourLevelChainRawPem => GetCertificateRawPem("four-level-chain.pem");

    public static string FourLevelChainWithMultiLeafRawPem => GetCertificateRawPem("four-level-chain-with-multi-leaf.pem");

    public static string FourLevelChainWithDuplicatesRawPem => GetCertificateRawPem("four-level-chain-with-duplicates.pem");

    public static string FourLevelChainWithMultiRootRawPem => GetCertificateRawPem("four-level-chain-with-multi-root.pem");

    public static string IsolatedRootRawPem => GetCertificateRawPem("isolated-root.pem");

    public static string IsolatedLeafRawPem => GetCertificateRawPem("isolated-leaf.pem");

    public static string ExpiredChainRawPem => GetCertificateRawPem("expired-chain.pem");

    private static string GetCertificateRawPem(string fileName)
    {
        var assemblyFolder = Path.GetDirectoryName(typeof(CertificateTestFiles).Assembly.Location);
        return File.ReadAllText(Path.Join(assemblyFolder, $"X509/TestFiles/{fileName}"));
    }
}
