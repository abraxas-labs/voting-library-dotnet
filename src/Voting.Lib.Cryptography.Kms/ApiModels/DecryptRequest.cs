// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Kms.ApiModels;

internal record DecryptRequest(string Id, string Ciphertext, string Tag, string Iv, string Mode);
