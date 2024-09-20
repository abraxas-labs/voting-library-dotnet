// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.DmDoc.Configuration;

namespace Voting.Lib.DmDoc;

/// <summary>
/// A default DmDoc user name provider implementation which reads a static user name from the config.
/// </summary>
public class DmDocConfigUserNameProvider : IDmDocUserNameProvider
{
    private readonly DmDocConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocConfigUserNameProvider"/> class.
    /// </summary>
    /// <param name="config">The DmDoc configuration.</param>
    public DmDocConfigUserNameProvider(DmDocConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string UserName => _config.Username;
}
