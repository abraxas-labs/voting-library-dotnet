// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Cryptography.Kms.ApiModels;

internal record Page<T>(List<T>? Resources);
