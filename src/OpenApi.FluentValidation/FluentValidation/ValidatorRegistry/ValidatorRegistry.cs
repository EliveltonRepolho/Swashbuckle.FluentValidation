// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentValidation;
using Microsoft.Extensions.Options;

namespace OpenApi.FluentValidation;

/// <summary>
/// <see cref="IValidatorRegistry"/> that works with registered validators.
/// </summary>
public class ValidatorRegistry : IValidatorRegistry
{
    private readonly ISchemaGenerationOptions _options;
    private readonly List<IValidator> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatorRegistry"/> class.
    /// </summary>
    /// <param name="validators">Validators.</param>
    /// <param name="options">Generation options.</param>
    public ValidatorRegistry(
        IEnumerable<IValidator> validators,
        IOptions<SchemaGenerationOptions>? options = null)
    {
        _validators = validators.ToList();
        _options = options?.Value ?? new SchemaGenerationOptions();
    }

    /// <inheritdoc />
    public IValidator? GetValidator(Type type)
    {
        return GetValidators(type).FirstOrDefault();
    }

    /// <inheritdoc />
    public IEnumerable<IValidator> GetValidators(Type type)
    {
        return _validators.GetValidators(type, _options);
    }
}