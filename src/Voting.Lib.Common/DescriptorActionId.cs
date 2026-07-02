// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Common;

/// <summary>
/// Action identifier based on the action name and the action parameters as strings.
/// </summary>
public class DescriptorActionId : IActionId
{
    private readonly List<object> _actionParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptorActionId"/> class.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="actionParameters">The action parameters (should identify the action uniquely including the state of the target entity).</param>
    protected DescriptorActionId(string actionName, params object[] actionParameters)
    {
        _actionParameters = [actionName, .. actionParameters];
    }

    /// <summary>
    /// Initializes a new instance of the <see>
    ///     <cref>SecondFactorTransactionActionId</cref>
    /// </see>
    /// class.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="actionParameters">The action parameters (should identify the action uniquely including the state of the target entity).</param>
    /// <returns>The created instance.</returns>
    public static DescriptorActionId Create(string actionName, params object[] actionParameters)
        => new(actionName, actionParameters);

    /// <inheritdoc />
    public virtual string ComputeHash()
        => HashUtil.GetSHA256Hash(string.Join("-", _actionParameters));
}
