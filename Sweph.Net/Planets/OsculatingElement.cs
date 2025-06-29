namespace Sweph.Net.Planets;

/// <summary>
/// Osculating element informations
/// </summary>
/// <param name="Name">Name of the celestial body.</param>
/// <param name="Epoch">Epoch of the elements.</param>
/// <param name="Equinox">Equinox of the elements.</param>
/// <param name="MeanAnomaly">Mean anomaly at the epoch (degrees).</param>
/// <param name="SemiAxis">Semi-major axis (AU).</param>
/// <param name="Eccentricity">Eccentricity of the orbit.</param>
/// <param name="Perihelion">Argument of perihelion (degrees).</param>
/// <param name="AscendingNode">Longitude of ascending node (degrees).</param>
/// <param name="Inclination">Inclination to the ecliptic (degrees).</param>
public readonly record struct OsculatingElement(
    string Name,
    double Epoch,
    double Equinox,
    double MeanAnomaly,
    double SemiAxis,
    double Eccentricity,
    double Perihelion,
    double AscendingNode,
    double Inclination);
