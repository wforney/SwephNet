namespace Sweph.Net.Geography;

/// <summary>
/// Represents a longitude value in degrees, minutes, and seconds.
/// </summary>
public readonly record struct Longitude
{
    /// <summary>
    /// Create a longitude from a value
    /// </summary>
    /// <param name="value"></param>
    public Longitude(double value)
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

        Polarity = sig < 0 ? LongitudePolarity.West : LongitudePolarity.East;
    }

    /// <summary>
    /// Create a longitude from his components
    /// </summary>
    /// <param name="degrees"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    public Longitude(int degrees, int minutes, int seconds)
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

        Polarity = degrees < 0 ? LongitudePolarity.West : LongitudePolarity.East;
    }

    /// <summary>
    /// Create a longitude from his components
    /// </summary>
    /// <param name="degrees"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <param name="polarity"></param>
    public Longitude(int degrees, int minutes, int seconds, LongitudePolarity polarity)
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
        if (polarity == LongitudePolarity.West)
        {
            Value = -Value;
        }

        Polarity = polarity;
    }
    /// <summary>
    /// Gets the degrees.
    /// </summary>
    /// <value>The degrees.</value>
    public int Degrees { get; private init; }

    /// <summary>
    /// Gets the minutes.
    /// </summary>
    /// <value>The minutes.</value>
    public int Minutes { get; private init; }

    /// <summary>
    /// Gets the seconds.
    /// </summary>
    /// <value>The seconds.</value>
    public int Seconds { get; private init; }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public double Value { get; private init; }

    /// <summary>
    /// Gets the polarity.
    /// </summary>
    /// <value>The polarity.</value>
    public LongitudePolarity Polarity { get; private init; }

    /// <inheritdoc/>
    public override string ToString() =>
        $"{Degrees}{Polarity.ToString()[0]}{Minutes:D2}'{Seconds:D2}\"";

    /// <summary>
    /// Performs an implicit conversion from <see cref="Longitude"/> to <see cref="double"/>.
    /// </summary>
    /// <param name="longitude">The longitude.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator double(Longitude longitude) => longitude.Value;

    /// <summary>
    /// Performs an implicit conversion from <see cref="double"/> to <see cref="Longitude"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Longitude(double value) => new(value);

    /// <summary>
    /// Converts the longitude to a double value.
    /// </summary>
    /// <returns>The longitude value as a double.</returns>
    public double ToDouble() => Value;

    /// <summary>
    /// Creates a <see cref="Longitude"/> from its components.
    /// </summary>
    /// <param name="degrees">The degrees.</param>
    /// <param name="minutes">The minutes.</param>
    /// <param name="seconds">The seconds.</param>
    /// <param name="polarity">The polarity.</param>
    /// <returns>A new instance of <see cref="Longitude"/> representing the specified components.</returns>
    public static Longitude FromComponents(int degrees, int minutes, int seconds, LongitudePolarity polarity) => new(degrees, minutes, seconds, polarity);

    /// <summary>
    /// Creates a <see cref="Longitude"/> from a double value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A new instance of <see cref="Longitude"/> representing the specified value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// nameof(value), Value must be between -180 and 180 degrees.
    /// </exception>
    public static Longitude FromDouble(double value)
    {
        return value is < (-180.0) or > 180.0
            ? throw new ArgumentOutOfRangeException(nameof(value), "Value must be between -180 and 180 degrees.")
            : new Longitude(value);
    }
}
