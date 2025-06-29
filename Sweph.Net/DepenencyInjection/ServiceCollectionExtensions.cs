using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sweph.Net.Chronology;
using Sweph.Net.Houses;
using Sweph.Net.Planets;
using Sweph.Net.Services;

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
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    [RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
    [RequiresUnreferencedCode("SwephNetSettings dependent types may have their members trimmed. Ensure all required members are preserved.")]
    public static IServiceCollection AddSwephNet(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<SwephNetSettings>(configuration.GetSection(SwephNetSettings.SectionName));

        services.TryAddSingleton<Context>();
        services.TryAddSingleton<IFileService, FileService>();
        services.TryAddSingleton<HouseContext>();
        services.TryAddSingleton<JulianDayDeltaT>();
        services.TryAddSingleton<PlanetContext>();

        return services;
    }
}
