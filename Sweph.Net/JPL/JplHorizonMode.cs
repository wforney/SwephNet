namespace Sweph.Net.JPL;

/// <summary>
/// JPL Horizons calculation modes
/// </summary>
[Flags]
public enum JplHorizonMode
{
    /// <summary>
    /// None
    /// </summary>
    None = 0,

    /// <summary>
    /// Reproduce JPL Horizons 1962 - today to 0.002 arcsec
    /// </summary>
    JplHorizons = 256 * 1024,

    /// <summary>
    /// Approximate JPL Horizons 1962 - today
    /// </summary>
    JplApproximate = 512 * 1024
}
