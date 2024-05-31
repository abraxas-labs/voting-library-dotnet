// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snapper;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// Extensions for snapper.
/// </summary>
public static class SnapperExtensions
{
    /// <summary>
    /// Use this settings to work with the snapshot files.
    /// Additionally to snapper, this ignores reference loops, which is handy to work with ef core.
    /// Use Newtonsoft since Snapper uses Newtonsoft as well.
    /// </summary>
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        DateParseHandling = DateParseHandling.None,
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    };

    /// <summary>
    /// Compares elements against a snapshot or updates the snapshot according to snapper rules.
    /// </summary>
    /// <param name="snapshot">Items to snapshot-compare.</param>
    /// <param name="ignoredProps">Properties to be ignored.</param>
    /// <typeparam name="T">Type of the elements.</typeparam>
    [AssertionMethod]
    public static void MatchSnapshot<T>(
        this IEnumerable<T> snapshot,
        params Expression<Func<T, dynamic>>[] ignoredProps)
        where T : notnull
        => snapshot.MatchSnapshot(string.Empty, ignoredProps);

    /// <summary>
    /// Compares an element against a snapshot or updates the snapshot according to snapper rules.
    /// </summary>
    /// <param name="snapshot">Items to snapshot-compare.</param>
    /// <param name="ignoredProps">Properties to be ignored.</param>
    /// <typeparam name="T">Type of the elements.</typeparam>
    [AssertionMethod]
    public static void MatchSnapshot<T>(
        this T snapshot,
        params Expression<Func<T, dynamic>>[] ignoredProps)
        => snapshot.MatchSnapshot(string.Empty, ignoredProps);

    /// <summary>
    /// Compares elements against a snapshot or updates the snapshot according to snapper rules.
    /// </summary>
    /// <param name="snapshot">Items to snapshot-compare.</param>
    /// <param name="snapshotName">Name of the snapshot.</param>
    /// <param name="ignoredProps">Properties to be ignored.</param>
    /// <typeparam name="T">Type of the elements.</typeparam>
    [AssertionMethod]
    public static void MatchSnapshot<T>(
        this IEnumerable<T> snapshot,
        string snapshotName,
        params Expression<Func<T, dynamic>>[] ignoredProps)
        where T : notnull
    {
        var snapshotList = snapshot.ToList();
        if (ignoredProps.Length > 0)
        {
            // clone, since this is not performance critical code at all and only testing code
            // json should be fine... Anyway it gets serialized to json during snapshotting
            snapshotList = snapshotList
                .Select(x => JObject.FromObject(x, JsonSerializer.Create(JsonSettings)).ToObject<T>())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();

            foreach (var value in snapshotList)
            {
                SetDefaultValues(value, ignoredProps);
            }
        }

        snapshotList.MatchSnapshot(snapshotName);
    }

    /// <summary>
    /// Compares an element against a snapshot or updates the snapshot according to snapper rules.
    /// </summary>
    /// <param name="snapshot">Items to snapshot-compare.</param>
    /// <param name="snapshotName">Name of the snapshot.</param>
    /// <param name="ignoredProps">Properties to be ignored.</param>
    /// <typeparam name="T">Type of the elements.</typeparam>
    [AssertionMethod]
    public static void MatchSnapshot<T>(
        this T snapshot,
        string snapshotName,
        params Expression<Func<T, dynamic>>[] ignoredProps)
    {
        T? nullableSnapshot = snapshot;

        // clone, since this is not performance critical code at all and only testing code
        // json should be fine... Anyway it gets serialized to json during snapshotting
        if (ignoredProps.Length > 0 && nullableSnapshot != null)
        {
            nullableSnapshot = JObject.FromObject(nullableSnapshot, JsonSerializer.Create(JsonSettings)).ToObject<T>();
            if (nullableSnapshot != null)
            {
                SetDefaultValues(nullableSnapshot, ignoredProps);
            }
        }

        var obj = ToJObject(nullableSnapshot);
        if (string.IsNullOrEmpty(snapshotName))
        {
            obj.ShouldMatchSnapshot();
        }
        else
        {
            obj.ShouldMatchChildSnapshot(snapshotName);
        }
    }

    /// <summary>
    /// Compares XML content (as a string, not an object) against a snapshot or updates the snapshot.
    /// Formats the XML before comparing.
    /// Generates the snapshot path based on the caller information.
    /// </summary>
    /// <param name="content">The content to compare with the snapshot.</param>
    /// <param name="updateSnapshot">Whether to update the snapshot.</param>
    /// <param name="childTestCaseName">The child name of the test case (use when multiple snapshots are taken in one test).</param>
    /// <param name="memberName">The name of the source member / test method.</param>
    /// <param name="filePath">The name of the source file path.</param>
    [AssertionMethod]
    public static void MatchXmlSnapshot(
        this string content,
        bool? updateSnapshot = null,
        string childTestCaseName = "",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "")
    {
        content = XmlUtil.FormatTestXml(content);
        content.MatchCallerSnapshot("xml", updateSnapshot, childTestCaseName, memberName, filePath);
    }

    /// <summary>
    /// Compares "raw content" (as a string, not an object) against a snapshot or updates the snapshot.
    /// Generates the snapshot path based on the caller information.
    /// </summary>
    /// <param name="content">The content to compare with the snapshot.</param>
    /// <param name="snapshotExt">The file extension of the snapshot.</param>
    /// <param name="updateSnapshot">Whether to update the snapshot.</param>
    /// <param name="childTestCaseName">The child name of the test case (use when multiple snapshots are taken in one test).</param>
    /// <param name="memberName">The name of the source member / test method.</param>
    /// <param name="filePath">The name of the source file path.</param>
    [AssertionMethod]
    public static void MatchRawCallerSnapshot(
        this string content,
        string snapshotExt,
        bool? updateSnapshot = null,
        string childTestCaseName = "",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "")
        => content.MatchCallerSnapshot(snapshotExt, updateSnapshot, childTestCaseName, memberName, filePath);

    /// <summary>
    /// Compares "raw content" (as a string, not an object) against a snapshot or updates the snapshot.
    /// Since this method does not use Snapper, the path to the snapshot and whether to update the snapshot must be passed manually.
    /// </summary>
    /// <param name="content">The content to compare with the snapshot.</param>
    /// <param name="snapshotPath">Full path to the snapshot file.</param>
    /// <param name="updateSnapshot">Whether to update the snapshot.</param>
    [AssertionMethod]
    public static void MatchRawSnapshot(this string content, string snapshotPath, bool updateSnapshot)
    {
        if (updateSnapshot)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(snapshotPath)!);
            if (!File.Exists(snapshotPath) || !File.ReadAllText(snapshotPath).Equals(content, StringComparison.Ordinal))
            {
                File.WriteAllText(snapshotPath, content);
            }
        }
        else
        {
            File.ReadAllText(snapshotPath)
                .Should()
                .Be(content);
        }
    }

    /// <summary>
    /// Builds a snapshot file path.
    /// </summary>
    /// <param name="childTestCaseName">The name of the child test case.</param>
    /// <param name="memberName">The test method name.</param>
    /// <param name="sourceFilePath">The source file path.</param>
    /// <param name="snapshotExt">The extension of the snapshot file.</param>
    /// <returns>The path to the snapshot file.</returns>
    public static string GetSnapshotFilePath(string childTestCaseName, string memberName, string sourceFilePath, string snapshotExt)
    {
        if (!string.IsNullOrEmpty(childTestCaseName))
        {
            childTestCaseName = "_" + childTestCaseName;
        }

        // Use the same base file name as the test class
        // we group the snapshot files via <DependentUpon> with the test class.
        return Path.Join(
            Path.GetDirectoryName(sourceFilePath),
            "_snapshots",
            $"{Path.GetFileNameWithoutExtension(sourceFilePath)}_{memberName}{childTestCaseName}.{snapshotExt}");
    }

    private static void MatchCallerSnapshot(
        this string content,
        string snapshotExt,
        bool? updateSnapshot,
        string childTestCaseName,
        string memberName,
        string filePath)
    {
        var path = GetSnapshotFilePath(childTestCaseName, memberName, filePath, snapshotExt);

#if DEBUGUPDATESNAPSHOTS
        updateSnapshot ??= true;
#else
        updateSnapshot ??= false;
#endif

        MatchRawSnapshot(content, path, updateSnapshot.Value);
    }

    /// <summary>
    /// Convert an object to a JObject using the <see cref="JsonSettings"/>.
    /// Same code as in Snapper for snapshots to be compatible.
    /// </summary>
    /// <param name="obj">The object to be converted.</param>
    /// <returns>The converted JObject.</returns>
    private static JObject ToJObject(object? obj)
    {
        obj ??= new object();
        try
        {
            return JObject.FromObject(obj, JsonSerializer.Create(JsonSettings));
        }
        catch (ArgumentException)
        {
            return JObject.FromObject(new { AutoGenerated = obj }, JsonSerializer.Create(JsonSettings));
        }
    }

    private static void SetDefaultValues<T>(T snapshot, params Expression<Func<T, dynamic>>[] ignoredProps)
    {
        foreach (var ignoredProp in ignoredProps)
        {
            SetDefaultValue(snapshot, ignoredProp);
        }
    }

    private static void SetDefaultValue<T>(T snapshot, Expression<Func<T, dynamic>> propExpr)
    {
        if (snapshot == null)
        {
            return;
        }

        var body = propExpr.Body;

        // some DataTypes are converted to dynamic by dotnet.
        if (body is UnaryExpression { NodeType: ExpressionType.Convert } unaryExp)
        {
            body = unaryExp.Operand;
        }

        if (body is not MemberExpression mex)
        {
            throw new InvalidCastException();
        }

        if (mex.Member is not PropertyInfo pi)
        {
            throw new InvalidCastException();
        }

        var target = GetTarget(snapshot, mex.Expression);
        if (target != null)
        {
            pi.SetValue(target, GetDefaultValue(pi.PropertyType), null);
        }
    }

    private static object? GetDefaultValue(Type t)
        => t == typeof(string)
            ? string.Empty
            : t == typeof(Guid)
                ? Guid.Empty
                : t.IsValueType
                    ? null
                    : Activator.CreateInstance(t);

    private static object? GetTarget(object? obj, Expression? expr)
    {
        if (obj == null || expr == null)
        {
            return null;
        }

        switch (expr.NodeType)
        {
            case ExpressionType.Parameter:
                return obj;
            case ExpressionType.MemberAccess:
                var mex = (MemberExpression)expr;
                if (mex.Member is not PropertyInfo pi)
                {
                    throw new InvalidCastException();
                }

                var target = GetTarget(obj, mex.Expression);
                return pi.GetValue(target, null);
            default:
                throw new InvalidOperationException();
        }
    }
}
