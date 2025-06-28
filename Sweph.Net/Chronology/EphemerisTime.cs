using System.Globalization;

namespace Sweph.Net.Chronology;

/// <summary>
/// Represents a Julian Day as Ephemeris Time.
/// </summary>
/// <param name="JulianDay">The Julian Day associated with this Ephemeris Time.</param>
/// <param name="DeltaT">
/// The DeltaT value, which is the difference between Universal Time and Ephemeris Time.
/// </param>
public readonly record struct EphemerisTime(JulianDay JulianDay, double DeltaT)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EphemerisTime"/> struct.
    /// </summary>
    /// <param name="julianDay">The Julian day.</param>
    public EphemerisTime(JulianDay julianDay)
        : this(julianDay, new JulianDayDeltaT().DeltaTAsync(julianDay).ConfigureAwait(false).GetAwaiter().GetResult()) // TODO: use async/await properly
    {
    }

    /// <summary>
    /// Gets the value of the Ephemeris Time, which is the Julian Day value plus DeltaT.
    /// </summary>
    /// <value>The Ephemeris Time value, calculated as Julian Day value plus DeltaT.</value>
    public double Value => JulianDay.Value + DeltaT;

    /// <summary>
    /// Implicitly converts an <see cref="EphemerisTime"/> to a double.
    /// </summary>
    public static implicit operator double(EphemerisTime et) => et.Value;

    /// <inheritdoc/>
    public override string ToString() => Value.ToString("G17", CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the Ephemeris Time to a double representation.
    /// </summary>
    /// <returns>The double representation of the Ephemeris Time value.</returns>
    public double ToDouble() => Value;

    /// <summary>
    /// Converts the Ephemeris Time to a Universal Time representation.
    /// </summary>
    /// <returns>
    /// A <see cref="UniversalTime"/> instance representing the Ephemeris Time in Universal Time.
    /// </returns>
    public UniversalTime ToUniversalTime() => JulianDay.ToUniversalTime(JulianDay.Value, JulianDay.Calendar);
}
