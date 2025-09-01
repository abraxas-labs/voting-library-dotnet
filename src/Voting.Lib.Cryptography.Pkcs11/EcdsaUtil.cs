// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography;

namespace Voting.Lib.Cryptography.Pkcs11;

internal static class EcdsaUtil
{
    private const string EcdsaP256NamedCurveValueHsm = "NIST-P256";
    private const string EcdsaP384NamedCurveValueHsm = "NIST-P384";
    private const string EcdsaP521NamedCurveValueHsm = "NIST-P521";

    /// <summary>
    /// Decodes a DER encoded EC point with ANSI X9.69 content of type 'octed' and extracts the x and y coordinates
    /// into its ECPoint representation. This implementation only supports the X9.69 uncompressed
    /// format (compression type = 0x04).
    /// Format: | DER content type | DER content length | ANSI X9.69 compression type | DER payload |
    /// Sample: | 0x04 | 0x68 | 0x04 | {x-coordinate:48-byes} | {y-coordinate:48-bytes} |.
    /// </summary>
    /// <param name="encodedEcPoint">The encoded ec point.</param>
    /// <param name="curve">The curve.</param>
    /// <returns>The parsed ECPoint as <see cref="System.Security.Cryptography.ECPoint"/>.</returns>
    /// <exception cref="NotSupportedException">If the encoded point / curve combination is not supported.</exception>
    /// <exception cref="ArgumentException">If the key has an invalid size.</exception>
    public static ECPoint DecodeEcPoint(ReadOnlySpan<byte> encodedEcPoint, ECCurve curve)
    {
        var ecPointX962 = AsnDecoder.ReadOctetString(encodedEcPoint, AsnEncodingRules.DER, out _);
        var ecPointCompression = ecPointX962[0];
        var ecPointContent = ecPointX962.Skip(1).ToList();
        var ecCurveLength = ecPointContent.Count / 2 * 8;
        var ecKeySize = ECDsa.Create(curve).KeySize;

        if (ecPointCompression != 0x04)
        {
            throw new NotSupportedException($"Unsupported X9.62 ECPoint compression type '{ecPointCompression:X2}' ");
        }

        if (ecPointContent.Count % 2 > 0)
        {
            throw new ArgumentException($"Invalid length of <{ecPointContent.Count()}> bytes for concatenated ECPoint [x,y] coordinates.");
        }

        if (ecCurveLength != ecKeySize)
        {
            throw new ArgumentException($"The ECPoint length ({ecCurveLength} bits) does not match with the desired ECCurve key size ({ecKeySize}).");
        }

        return new ECPoint
        {
            X = ecPointContent.Take(ecKeySize / 8).ToArray(),
            Y = ecPointContent.Skip(ecKeySize / 8).ToArray(),
        };
    }

    public static ECCurve MapFromHsmNamedCurve(string namedCurve)
        => namedCurve.ToUpper() switch
        {
            EcdsaP256NamedCurveValueHsm => ECCurve.NamedCurves.nistP256,
            EcdsaP384NamedCurveValueHsm => ECCurve.NamedCurves.nistP384,
            EcdsaP521NamedCurveValueHsm => ECCurve.NamedCurves.nistP521,
            _ => throw new NotSupportedException($"The named curve {namedCurve} provided by the HSM is not supported."),
        };
}
