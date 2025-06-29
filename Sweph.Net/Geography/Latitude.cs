namespace Sweph.Net.Geography;

/// <summary>
/// Represents a geographic latitude.
/// </summary>
public readonly record struct Latitude
{
    /// <summary>
    /// Create a latitude from a value
    /// </summary>
    /// <param name="value"></param>
    public Latitude(double value)
        : this()
    {
        int sig = Math.Sign(value);
        value = Math.Abs(value);
        Degrees = (int)value;
        Minutes = ((int)(value * 60.0)) % 60;
        Seconds = ((int)(value * 3600.0)) % 60;

        while (Degrees >= 180)
        {
            Degrees -= 180;
        }

        Value = Degrees + (Minutes / 60.0) + (Seconds / 3600.0);
        if (sig < 0)
        {
            Value = -Value;
        }

        Polarity = sig < 0 ? LatitudePolarity.South : LatitudePolarity.North;
    }

    /// <summary>
    /// Create a latitude from his components
    /// </summary>
    /// <param name="degrees"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    public Latitude(int degrees, int minutes, int seconds)
        : this()
    {
        if (degrees is <= (-180) or >= 180)
        {
            throw new ArgumentOutOfRangeException(nameof(degrees));
        }

        if (minutes is < 0 or >= 60)
        {
            throw new ArgumentOutOfRangeException(nameof(minutes));
        }

        if (seconds is < (int)0.0 or >= (int)60.0)
        {
            throw new ArgumentOutOfRangeException(nameof(seconds));
        }

        Degrees = Math.Abs(degrees);
        Minutes = minutes;
        Seconds = seconds;

        Value = Degrees + (Minutes / 60.0) + (Seconds / 3600.0);
        if (degrees < 0)
        {
            Value = -Value;
        }

        Polarity = degrees < 0 ? LatitudePolarity.South : LatitudePolarity.North;
    }

    /// <summary>
    /// Create a latitude from his components
    /// </summary>
    /// <param name="degrees"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <param name="polarity"></param>
    public Latitude(int degrees, int minutes, int seconds, LatitudePolarity polarity)
        : this()
    {
        if (degrees is < 0 or >= 180)
        {
            throw new ArgumentOutOfRangeException(nameof(degrees));
        }

        if (minutes is < 0 or >= 60)
        {
            throw new ArgumentOutOfRangeException(nameof(minutes));
        }

        if (seconds is < (int)0.0 or >= (int)60.0)
        {
            throw new ArgumentOutOfRangeException(nameof(seconds));
        }

        Degrees = degrees;
        Minutes = minutes;
        Seconds = seconds;

        Value = Degrees + (Minutes / 60.0) + (Seconds / 3600.0);
        if (polarity == LatitudePolarity.South)
        {
            Value = -Value;
        }

        Polarity = polarity;
    }

    /// <summary>
    /// Convert to string
    /// </summary>
    public override string ToString() => $"{Degrees}{Polarity.ToString()[0]}{Minutes:D2}'{Seconds:D2}\"";

    /// <summary>
    /// Implicit conversion of Latitude to Double
    /// </summary>
    public static implicit operator double(Latitude lat) => lat.Value;

    /// <summary>
    /// Implicit conversion of Double to Latitude
    /// </summary>
    public static implicit operator Latitude(double value) => new(value);

    /// <summary>
    /// The latitude value in degrees.
    /// </summary>
    public double Value { get; private init; }

    /// <summary>
    /// The latitude in degrees.
    /// </summary>
    public int Degrees { get; private init; }

    /// <summary>
    /// The latitude in minutes.
    /// </summary>
    public int Minutes { get; private init; }

    /// <summary>
    /// The latitude in seconds.
    /// </summary>
    public double Seconds { get; private init; }

    /// <summary>
    /// The polarity of the latitude.
    /// </summary>
    public LatitudePolarity Polarity { get; private init; }

    /// <summary>
    /// Converts to double.
    /// </summary>
    /// <returns>double.</returns>
    public double ToDouble() => Value;

    /// <summary>
    /// Creates a new Latitude from a double value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A new instance of <see cref="Latitude"/>.</returns>
    public static Latitude FromDouble(double value) => new(value);
}
