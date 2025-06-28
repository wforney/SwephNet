namespace Sweph.Net.Utilities;

/// <summary>
/// Utility class for formatting double values into human-readable strings.
/// </summary>
public static class DoubleFormatter
{
    /// <summary>
    /// Format a value to format : D ° MM' SS.0000
    /// </summary>
    /// <param name="value">
    /// The value to format, typically representing an angle in degrees.
    /// </param>
    /// <returns>
    /// A string representing the formatted value in degrees, minutes, and seconds.
    /// </returns>
    public static string FormatAsDegrees(double value)
    {
        bool minus = value < 0;
        value = Math.Abs(value);
        int deg = (int)value;
        int min = (int)(value * 60.0 % 60.0);
        double sec = value * 3600.0 % 60.0;
        return $"{(minus ? '-' : ' ')}{deg,3:##0}° {min,2:#0}' {sec,7:#0.0000}";
    }

    /// <summary>
    /// Format a value to format : 'HH' h 'mm' m 'ss' s
    /// </summary>
    /// <param name="value">
    /// The value to format, typically representing an angle in hours.
    /// </param>
    /// <returns>
    /// A string representing the formatted value in hours, minutes, and seconds.
    /// </returns>
    public static string FormatAsHour(double value)
    {
        int deg = (int)value;
        value = Math.Abs(value);
        int min = (int)(value * 60.0 % 60.0);
        int sec = (int)(value * 3600.0 % 60.0);
        return $"{deg,2:#0} h {min:00} m {sec:00} s";
    }

    /// <summary>
    /// Format a value to format : HH:mm:ss
    /// </summary>
    /// <param name="value">
    /// The value to format, typically representing a time in hours.
    /// </param>
    /// <returns>
    /// A string representing the formatted value in hours, minutes, and seconds in a compact format.
    /// </returns>
    public static string FormatAsTime(double value)
    {
        int deg = (int)value;
        value = Math.Abs(value);
        int min = (int)(value * 60.0 % 60.0);
        int sec = (int)(value * 3600.0 % 60.0);
        return $"{deg,2:00}:{min:00}:{sec:00}";
    }
}
