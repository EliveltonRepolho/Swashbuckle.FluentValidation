﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// ReSharper disable UnusedMember.Global
#pragma warning disable SA1402

namespace MicroElements.OpenApi.Core;

/// <summary>
/// Provides common condition cases.
/// </summary>
public static class Condition
{
    /// <summary>
    /// Gets an Empty condition instance.
    /// </summary>
    /// <typeparam name="T">Condition type.</typeparam>
    /// <returns>Condition instance.</returns>
    public static ICondition<T> Empty<T>() => EmptyCondition<T>.Instance;

    /// <summary>
    /// Gets the same condition if it is not null and Empty condition if null.
    /// </summary>
    /// <typeparam name="T">Condition type.</typeparam>
    /// <param name="condition">The condition.</param>
    /// <returns>Not null condition instance.</returns>
    public static ICondition<T> NotNull<T>(this ICondition<T>? condition) => condition ?? Empty<T>();

    /// <summary>
    /// Gets condition that combines other conditions with logical AND.
    /// </summary>
    /// <typeparam name="T">Condition type.</typeparam>
    /// <param name="condition1">The first condition.</param>
    /// <param name="condition2">The second condition.</param>
    /// <returns>Condition that is true when both conditions are true.</returns>
    public static ICondition<T> And<T>(this ICondition<T> condition1, ICondition<T> condition2) =>
        new AndCondition<T>(condition1, condition2);

    /// <summary>
    /// Gets condition that combines other conditions with logical OR.
    /// </summary>
    /// <typeparam name="T">Condition type.</typeparam>
    /// <param name="condition1">The first condition.</param>
    /// <param name="condition2">The second condition.</param>
    /// <returns>Condition that is true when at least one condition is true.</returns>
    public static ICondition<T> Or<T>(this ICondition<T> condition1, ICondition<T> condition2) =>
        new OrCondition<T>(condition1, condition2);
}

/// <summary>
/// The simple condition that uses provided predicate.
/// </summary>
/// <typeparam name="T">Type for matching.</typeparam>
public class Condition<T> : ICondition<T>
{
    private readonly Func<T, bool> _matches;

    /// <summary>
    /// Initializes a new instance of the <see cref="Condition{T}"/> class.
    /// </summary>
    /// <param name="matches">Func that implements matching behaviour.</param>
    public Condition(Func<T, bool> matches)
    {
        _matches = matches.AssertArgumentNotNull(nameof(matches));
    }

    /// <inheritdoc />
    public bool Matches(T value)
    {
        return _matches(value);
    }
}

/// <summary>
/// Empty condition that always returns true.
/// </summary>
/// <typeparam name="T">Type for matching.</typeparam>
public sealed class EmptyCondition<T> : ICondition<T>
{
    /// <summary>
    /// Gets global static instance of empty condition.
    /// </summary>
    public static EmptyCondition<T> Instance { get; } = new();

    /// <inheritdoc />
    public bool Matches(T value) => true;
}

/// <summary>
/// Condition that combines one or more conditions using logical AND.
/// </summary>
/// <typeparam name="T">Condition context type.</typeparam>
public class AndCondition<T> : ICondition<T>
{
    private readonly IReadOnlyCollection<ICondition<T>>? _conditions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AndCondition{T}"/> class.
    /// </summary>
    /// <param name="conditions">Conditions to combine.</param>
    public AndCondition(params ICondition<T>[] conditions) => _conditions = conditions.ToArray();

    /// <inheritdoc />
    public bool Matches(T value)
    {
        if (_conditions is null || _conditions.Count == 0)
            return true;
        return _conditions.All(condition => condition.Matches(value));
    }
}

/// <summary>
/// Condition that combines one or more conditions using logical OR.
/// </summary>
/// <typeparam name="T">Condition context type.</typeparam>
public class OrCondition<T> : ICondition<T>
{
    private readonly IReadOnlyCollection<ICondition<T>>? _conditions;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrCondition{T}"/> class.
    /// </summary>
    /// <param name="conditions">Conditions to combine.</param>
    public OrCondition(params ICondition<T>[] conditions) => _conditions = conditions.ToArray();

    /// <inheritdoc />
    public bool Matches(T value)
    {
        if (_conditions is null || _conditions.Count == 0)
            return true;
        return _conditions.Any(condition => condition.Matches(value));
    }
}