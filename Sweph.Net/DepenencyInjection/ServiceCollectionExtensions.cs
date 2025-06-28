using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sweph.Net.Chronology;

namespace Sweph.Net.DepenencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to add Sweph.Net services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the Sweph.Net services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSwephNet(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // SweDate
        // SwePlanet
        // SwpHouse

        services.TryAddSingleton<Context>();
        services.TryAddSingleton<JulianDayDeltaT>();

        return services;
    }
}
