using System.Collections.Immutable;

namespace Sweph.Net.Houses;

/// <summary>
/// HousePoint class represents various significant points in the astrological houses system.
/// </summary>
/// <param name="Id">The identifier of the house point.</param>
/// <param name="Name">The name of the house point.</param>
public readonly record struct HousePoint(int Id, string Name)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HousePoint"/> struct.
    /// </summary>
    public HousePoint() : this(-1, string.Empty)
    {
    }

    /// <summary>
    /// Ascendant
    /// </summary>
    public static HousePoint Ascendant => Points[0];

    /// <summary>
    /// MC
    /// </summary>
    public static HousePoint MC => Points[1];

    /// <summary>
    /// ARMC
    /// </summary>
    public static HousePoint ARMC => Points[2];

    /// <summary>
    /// Vertex
    /// </summary>
    public static HousePoint Vertex => Points[3];

    /// <summary>
    /// Equatorial ascendant
    /// </summary>
    public static HousePoint EquatorialAscendant => Points[4];

    /// <summary>
    /// "co-ascendant" (W. Koch)
    /// </summary>
    public static HousePoint CoAscendantKoch => Points[5];

    /// <summary>
    /// "co-ascendant" (M. Munkasey)
    /// </summary>
    public static HousePoint CoAscendantMunkasey => Points[6];

    /// <summary>
    /// "polar ascendant" (M. Munkasey)
    /// </summary>
    public static HousePoint PolarAcsendant => Points[7];

    /// <summary>
    /// Liste of points
    /// </summary>
    public static ImmutableArray<HousePoint> Points { get; } =
        [
        new HousePoint(0, "Ascendant"),
        new HousePoint(1, "MC"),
        new HousePoint(2, "ARMC"),
        new HousePoint(3, "Vertex"),
        new HousePoint(4, "Equatorial ascendant"),
        new HousePoint(5, "Co-Ascendant Koch (W. Koch)"),
        new HousePoint(6, "Co-Ascendant (M. Munkasey)"),
        new HousePoint(7, "Polar ascendant (M. Munkasey)")
        ];

    /// <summary>
    /// Convert to int
    /// </summary>
    public static implicit operator int(HousePoint point) => point.Id;

    /// <summary>
    /// Get the housepoint of an id
    /// </summary>
    public static implicit operator HousePoint?(int id) => id < 0 || id >= Points.Length ? null : Points[id];

    /// <summary>
    /// Converts the HousePoint to Int32.
    /// </summary>
    /// <returns>The integer identifier of the house point.</returns>
    public int ToInt32() => Id;

    /// <summary>
    /// Converts to nullable.
    /// </summary>
    /// <returns>Sweph.Net.Houses.HousePoint?.</returns>
    public HousePoint? ToNullable() => Id < 0 || Id >= Points.Length ? null : Points[Id];
}
