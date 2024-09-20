// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.DmDoc.Exceptions;
using Voting.Lib.DmDoc.Models.Internal;

namespace Voting.Lib.DmDoc.Extensions;

internal static class DmDocApiResponseExtensions
{
    public static void EnsureSuccess(this DmDocApiResponse? response)
    {
        if (response == null)
        {
            throw new DmDocException("Response content was empty.");
        }

        if (!response.Success)
        {
            throw new DmDocException($"Response was not successful: {response.GetErrorDescription()}");
        }
    }

    public static T EnsureSuccessAndUnwrap<T>(this DmDocDataApiResponse<T>? response)
        where T : notnull
    {
        response.EnsureSuccess();
        return response!.Data
            ?? throw new DmDocException("No response data provided.");
    }
}
