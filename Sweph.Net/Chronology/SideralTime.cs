using Sweph.Net.Utilities;

namespace Sweph.Net.Chronology;

/// <summary>
/// Represents a sideral time value, which is the angle of the Earth in its rotation relative to the stars.
/// </summary>
public readonly record struct SideralTime(double Value)
{
    /// <inheritdoc/>
    public override string ToString() => DoubleFormatter.FormatAsTime(Value);
}
