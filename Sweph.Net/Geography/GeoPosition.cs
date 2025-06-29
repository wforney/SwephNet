using Generator.Equals;

namespace Sweph.Net.Geography;

/// <summary>
/// Represents a geographic position defined by longitude, latitude, and altitude.
/// </summary>
/// <param name="longitude">The longitude.</param>
/// <param name="latitude">The latitude.</param>
/// <param name="altitude">The altitude.</param>
[Equatable]
public partial class GeoPosition(Longitude longitude, Latitude latitude, double altitude)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeoPosition"/> class.
    /// </summary>
    public GeoPosition()
        : this(new Longitude(0.0), new Latitude(0.0), 0.0)
    {
    }

    /// <summary>
    /// Gets or sets the altitude.
    /// </summary>
    /// <value>The altitude.</value>
    [DefaultEquality]
    public double Altitude { get; set; } = altitude;

    /// <summary>
    /// Gets or sets the latitude.
    /// </summary>
    /// <value>The latitude.</value>
    [DefaultEquality]
    public Latitude Latitude { get; set; } = latitude;

    /// <summary>
    /// Gets or sets the longitude.
    /// </summary>
    /// <value>The longitude.</value>
    [DefaultEquality]
    public Longitude Longitude { get; set; } = longitude;

    /// <inheritdoc/>
    public override string ToString() => $"{Longitude}, {Latitude}, {Altitude} m";
}
