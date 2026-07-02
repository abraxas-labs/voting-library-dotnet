// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Voting.Lib.Common.Extensions;

/// <summary>
/// Provides extension methods for enumeration types.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the value specified in the <see cref="EnumMemberAttribute"/> of an enum value.
    /// If the attribute is missing, it falls back to the name of the enum member.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="value">The enumeration value for which to retrieve the member value.</param>
    /// <returns>The string value defined in the <see cref="EnumMemberAttribute"/>, or the enum name if the attribute is not set.</returns>
    public static string GetEnumMemberValue<T>(this T value)
        where T : Enum
    {
        return value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<EnumMemberAttribute>()?
            .Value ?? value.ToString(); // Fallback to the name if attribute is missing
    }
}
