// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// A helper class for code quality tools (Sonar, ReSharper) to identify custom method's which do an assertion.
/// See SonarQube rule S2699.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AssertionMethodAttribute : Attribute
{
}
