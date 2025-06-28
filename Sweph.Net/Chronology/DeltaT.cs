namespace Sweph.Net.Chronology;

/// <summary>
/// Represents a record of DeltaT in a file.
/// </summary>
/// <param name="Year">The year.</param>
/// <param name="Value">The value.</param>
public readonly record struct DeltaT(int Year, double Value)
{
    /// <summary> for delta t: intrinsic tidal acceleration in the mean motion of the moon, not
    /// given in the parameters list of the ephemeris files but computed by
    /// Chapront/Chapront-Touzé/Francou A&amp;A 387 (2002), p. 705. </summary>
    public const double Tidal26 = -26.0;

    /// <summary> for delta t: intrinsic tidal acceleration in the mean motion of the moon, not
    /// given in the parameters list of the ephemeris files but computed by
    /// Chapront/Chapront-Touzé/Francou A&amp;A 387 (2002), p. 705. </summary>
    public const double TidalDE200 = -23.8946;

    /// <summary>
    /// was (-25.8) until V. 1.76.2
    /// </summary>
    public const double TidalDE403 = -25.580;

    /// <summary>
    /// was (-25.8) until V. 1.76.2
    /// </summary>
    public const double TidalDE404 = -25.580;

    /// <summary>
    /// was (-25.7376) until V. 1.76.2
    /// </summary>
    public const double TidalDE405 = -25.826;

    /// <summary>
    /// was (-25.7376) until V. 1.76.2
    /// </summary>
    public const double TidalDE406 = -25.826;

    /// <summary>
    /// JPL Interoffice Memorandum 14-mar-2008 on DE421 Lunar Orbit
    /// </summary>
    public const double TidalDE421 = -25.85;

    /// <summary>
    /// JPL Interoffice Memorandum 9-jul-2013 on DE430 Lunar Orbit
    /// </summary>
    public const double TidalDE430 = -25.82;

    /// <summary>
    /// Waiting for information
    /// </summary>
    public const double TidalDE431 = -25.82;

    /// <summary>
    /// Default tidal
    /// </summary>
    public const double TidalDefault = TidalDE431;
}
