// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Cli;

internal class CommandRegistration
{
    internal CommandRegistration(Type commandType, Type optionsType, Type interfaceType)
    {
        CommandType = commandType;
        OptionsType = optionsType;
        InterfaceType = interfaceType;
    }

    internal Type CommandType { get; }

    internal Type OptionsType { get; }

    internal Type InterfaceType { get; }
}
