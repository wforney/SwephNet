namespace Sweph.Net.Houses;

/// <summary>
/// House extensions
/// </summary>
public static class HouseSystemExtensions
{
    /// <summary>
    /// Convert a house system to a chararacter representation.
    /// </summary>
    public static char ToChar(this HouseSystem hs) => HouseContext.HouseSystemToChar(hs);
}
