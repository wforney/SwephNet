using System.Collections.Immutable;

namespace Sweph.Net.Houses;

/// <summary>
/// Represents the result of house calculations.
/// </summary>
/// <param name="Houses">The list of houses cuspids.</param>
/// <param name="AscMc">The list of Ascendant and Midheaven positions.</param>
public readonly record struct HouseResult(
    ImmutableArray<double> Houses,
    ImmutableArray<double> AscMc);
