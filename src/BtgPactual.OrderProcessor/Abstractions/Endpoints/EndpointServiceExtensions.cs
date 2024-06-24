using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BtgPactual.OrderProcessor.Abstractions.Endpoints;

public static class EndpointServiceExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly location)
    {
        var descriptors = location.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false }
                           && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(descriptors);

        return services;
    }
}