﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.OpenApi.Core;

/// <summary>
/// Extensions for some swagger specific work.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Is supported swagger numeric type.
    /// </summary>
    internal static bool IsNumeric(this object value) => value is int || value is long || value is float || value is double || value is decimal;

    /// <summary>
    /// Convert numeric to double.
    /// </summary>
    internal static decimal NumericToDecimal(this object value) => Convert.ToDecimal(value);

    /// <summary>
    /// Returns not null enumeration.
    /// </summary>
    internal static IEnumerable<TValue> NotNull<TValue>(this IEnumerable<TValue>? collection) =>
        collection ?? Array.Empty<TValue>();

    /// <summary>
    /// Skip simple types from schema generation.
    /// </summary>
    internal static bool IsPrimitiveType(this Type type) => type.IsPrimitive || type == typeof(string) || type == typeof(decimal);

    /// <summary>
    /// Returns array in debug mode and the same collection in release.
    /// </summary>
    internal static IEnumerable<TValue> ToArrayDebug<TValue>(this IEnumerable<TValue>? collection)
    {
#if DEBUG
        return collection?.ToArray() ?? Array.Empty<TValue>();
#else
        return collection;
#endif
    }
}