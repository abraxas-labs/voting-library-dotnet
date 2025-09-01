// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Kms.ApiModels;

internal record EncryptRequest(string Id, string Plaintext, string Mode);
