// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Common;

namespace Voting.Lib.Iam.SecondFactor.Models;

/// <inheritdoc />
public class SecondFactorTransactionActionId : ISecondFactorTransactionActionId
{
    private readonly List<object> _actionParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecondFactorTransactionActionId"/> class.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="actionParameters">The action parameters (should identify the action uniquely including the state of the target entity).</param>
    protected SecondFactorTransactionActionId(string actionName, params object[] actionParameters)
    {
        _actionParameters = [actionName, .. actionParameters];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecondFactorTransactionActionId"/> class.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="actionParameters">The action parameters (should identify the action uniquely including the state of the target entity).</param>
    /// <returns>The created instance.</returns>
    public static SecondFactorTransactionActionId Create(string actionName, params object[] actionParameters)
        => new(actionName, actionParameters);

    /// <inheritdoc />
    public virtual string ComputeHash()
        => HashUtil.GetSHA256Hash(string.Join("-", _actionParameters));
}
