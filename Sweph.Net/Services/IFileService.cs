using Sweph.Net.Chronology;
using Sweph.Net.Planets;

namespace Sweph.Net.Services;

internal interface IFileService
{
    /// <summary>
    /// Finds the name of the asteroid with the specified identifier as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// A Task&lt;string?&gt; representing the asynchronous operation, containing the name of the
    /// asteroid if found, or null if not found.
    /// </returns>
    Task<string?> FindAsteroidNameAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find element as an asynchronous operation.
    /// </summary>
    /// <param name="idPlanet">The identifier planet.</param>
    /// <param name="julianDay">The julian day.</param>
    /// <param name="fict_ifl">Fictitious ifl value.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// A Task&lt;ValueTuple&lt;OsculatingElement?, int?&gt;&gt; representing the asynchronous
    /// operation, containing the osculating element and fictitious ifl value.
    /// </returns>
    /// <exception cref="SwephNetException"></exception>
    Task<(OsculatingElement? OsculatingElement, int? fict_ifl)> FindElementAsync(int idPlanet, double julianDay, int fict_ifl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get <see cref="DeltaT"/> records as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A Task&lt;IAsyncEnumerable`1&gt; representing the asynchronous operation.</returns>
    IAsyncEnumerable<DeltaT> GetDeltaTRecordsAsync(CancellationToken cancellationToken = default);
}