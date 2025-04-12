// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentValidation;
using Microsoft.Extensions.Options;

namespace OpenApi.FluentValidation;

/// <summary>
/// Validator registry that gets validators from <see cref="IServiceProvider"/>.
/// </summary>
public class ServiceProviderValidatorRegistry : IValidatorRegistry
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISchemaGenerationOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceProviderValidatorRegistry"/> class.
    /// </summary>
    /// <param name="serviceProvider">The source service provider.</param>
    /// <param name="options">Schema generation options.</param>
    public ServiceProviderValidatorRegistry(
        IServiceProvider serviceProvider,
        IOptions<SchemaGenerationOptions>? options = null)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
        _options = options?.Value ?? new SchemaGenerationOptions();
    }

    /// <inheritdoc />
    public IValidator? GetValidator(Type type) => _serviceProvider.GetValidator(type);

    /// <inheritdoc />
    public IEnumerable<IValidator> GetValidators(Type type) => _serviceProvider.GetValidators(type, _options);
}