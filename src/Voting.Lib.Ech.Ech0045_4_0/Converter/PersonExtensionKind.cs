// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Ech.Ech0045_4_0.Converter;

/// <summary>
/// The shape of the eCH-0045 person extension to write or expect when reading.
/// </summary>
public enum PersonExtensionKind
{
    /// <summary>
    /// The VOTING extension shape. No xsi:type is written or expected.
    /// </summary>
    VotingVoterExtension = 0,

    /// <summary>
    /// The eCH-0045 voter extension (eCH-0045-voter-extension-1-0.xsd). Written and read with an explicit xsi:type.
    /// </summary>
    EVotingVoterExtension_1_0 = 1,
}
