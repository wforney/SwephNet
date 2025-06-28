using System.Globalization;

namespace Sweph.Net.Chronology;

/// <summary>
/// Represents a Julian Day in Universal Time.
/// </summary>
public partial record struct JulianDay
{
    /// <summary>
    /// 2000 January 1.5
    /// </summary>
    public const double J2000 = 2451545.0;

    /// <summary>
    /// First Julian Day of the Gregorian calendar : October 15, 1582
    /// </summary>
    public const double GregorianFirstJD = 2299160.5;

    /// <summary>
    /// Gets the calendar.
    /// </summary>
    /// <value>The calendar.</value>
    public DateCalendar Calendar { get; private init; }

    /// <summary>
    /// Gets the absolute Julian Day value.
    /// </summary>
    /// <value>The absolute Julian Day value.</value>
    public double Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDay"/> struct.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="day">The day.</param>
    /// <param name="hour">The hour.</param>
    /// <param name="calendar">The calendar.</param>
    public JulianDay(int year, int month, int day, double hour, DateCalendar? calendar = null)
        : this(new UniversalTime(year, month, day, hour), calendar)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDay"/> struct.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="day">The day.</param>
    /// <param name="hour">The hour.</param>
    /// <param name="minute">The minute.</param>
    /// <param name="second">The second.</param>
    /// <param name="calendar">The calendar.</param>
    public JulianDay(int year, int month, int day, int hour, int minute, int second, DateCalendar? calendar = null)
        : this(new UniversalTime(year, month, day, hour, minute, second), calendar)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDay"/> struct.
    /// </summary>
    /// <param name="value">The Julian Day value to initialize the instance with.</param>
    /// <param name="calendar">The calendar to use for the Julian Day.</param>
    public JulianDay(double value, DateCalendar? calendar = null)
        : this()
    {
        Calendar = calendar ?? GetCalendar(value);
        Value = value;
    }

    /// <summary>
    /// Create a new Julian Day from a DateUT
    /// </summary>
    /// <param name="date">Date source</param>
    /// <param name="calendar">Calendar source</param>
    public JulianDay(UniversalTime date, DateCalendar? calendar = null)
        : this()
    {
        Calendar = calendar ?? GetCalendar(date.Year, date.Month, date.Day);
        Value = FromDate(date, Calendar);
    }

    /// <summary>
    /// Get default calendar from a date
    /// </summary>
    /// <remarks>Gregorian calendar start at October 15, 1582</remarks>
    public static DateCalendar GetCalendar(int year, int month, int day)
    {
        int date = year * 10000 + month * 100 + day;
        return date >= 15821115 ? DateCalendar.Gregorian : DateCalendar.Julian;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDay"/> struct from a DateTime.
    /// </summary>
    /// <param name="date">
    /// The <see cref="DateTime"/> instance representing the date for which to create the Julian Day.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="JulianDay"/> struct representing the Julian Day of the
    /// specified date.
    /// </returns>
    public static JulianDay FromDate(DateTime date)
    {
        double julianDay = date.ToOADate() + 2415018.5;
        return new JulianDay(julianDay);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="JulianDay"/> to <see cref="double"/>.
    /// </summary>
    /// <param name="jd">The <see cref="JulianDay"/> instance.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator double(JulianDay jd) => jd.Value;

    /// <summary>
    /// Converts the Julian Day to a double value representing the Julian Day number.
    /// </summary>
    /// <returns>The Julian Day number as a double.</returns>
    public readonly double ToDouble() => Value;

    /// <inheritdoc/>
    public override readonly string ToString() => Value.ToString(CultureInfo.CurrentCulture);

    /// <summary>
    /// Converts the Julian Day to a <see cref="UniversalTime"/> in Universal Time (UT).
    /// </summary>
    /// <returns>
    /// A <see cref="UniversalTime"/> representing the Julian Day in Universal Time (UT).
    /// </returns>
    public readonly UniversalTime ToUniversalTime() => ToUniversalTime(Value, Calendar);

    /// <summary>
    /// Converts the Julian Day to a <see cref="DateTime"/> in Universal Time (UTC).
    /// </summary>
    /// <returns>A <see cref="DateTime"/> representing the Julian Day.</returns>
    public readonly DateTime ToDateTime() => ToUniversalTime().ToDateTime();

    /// <summary>
    /// Get the hour decimal value
    /// </summary>
    /// <param name="hour">Hour</param>
    /// <param name="minute">Minute</param>
    /// <param name="second">Second</param>
    /// <returns>The hour in decimal value</returns>
    public static double GetHourValue(int hour, int minute, int second) => hour + minute / 60.0 + second / 3600.0;

    /// <summary>
    /// This function returns the absolute Julian day number (JD) for a given date.
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="month">Month</param>
    /// <param name="day">Day</param>
    /// <param name="hour">Hour</param>
    /// <param name="minute">Minute</param>
    /// <param name="second">Second</param>
    /// <param name="calendar">Calendar of conversion</param>
    /// <returns>The julian day value as Universal Time</returns>
    public static double FromDate(
        int year, int month, int day,
        int hour, int minute, int second,
        DateCalendar calendar) => FromDate(year, month, day, GetHourValue(hour, minute, second), calendar);

    /// <summary>
    /// This function returns the absolute Julian day number (JD) for a given date.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="day">The day.</param>
    /// <param name="hour">The hour in decimal.</param>
    /// <param name="calendar">The calendar of conversion.</param>
    /// <returns>The Julian day value as Universal Time.</returns>
    /// <remarks>
    /// <para>Base on swe_julday()</para>
    /// <para>
    /// The Julian day number is a system of numbering all days continously within the time range of
    /// known human history. It should be familiar to every astrological or astronomical programmer.
    /// The time variable in astronomical theories is usually expressed in Julian days or Julian
    /// centuries (36525 days per century) relative to some start day; the start day is called 'the
    /// epoch'. The Julian day number is a double representing the number of days since JD = 0.0 on
    /// 1 Jan -4712, 12:00 noon (in the Julian calendar).
    /// </para>
    /// <para>
    /// Midnight has always a JD with fraction .5, because traditionally the astronomical day
    /// started at noon. This was practical because then there was no change of date during a night
    /// at the telescope. From this comes also the fact the noon ephemerides were printed before
    /// midnight ephemerides were introduced early in the 20th century.
    /// </para>
    /// <para>NOTE: The Julian day number must not be confused with the Julian calendar system.</para>
    /// <para>
    /// Be aware the we always use astronomical year numbering for the years before Christ, not the
    /// historical year numbering. Astronomical years are done with negative numbers, historical
    /// years with indicators BC or BCE (before common era). Year 0 (astronomical) = 1 BC year -1
    /// (astronomical) = 2 BC etc.
    /// </para>
    /// <para>
    /// Original author: Marc Pottenger, Los Angeles. with bug fix for year &lt; -4711 15-aug-88 by
    /// Alois Treindl (The parameter sequence m,d,y still indicates the US origin, be careful
    /// because the similar function date_conversion() uses other parameter sequence and also
    /// Astrodienst relative juldate.)
    /// </para>
    /// <para>
    /// References: Oliver Montenbruck, Grundlagen der Ephemeridenrechnung, Verlag Sterne und
    /// Weltraum (1987), p.49 ff
    /// </para>
    /// </remarks>
    public static double FromDate(
        int year, int month, int day,
        double hour, DateCalendar calendar)
    {
        double jd;
        double u, u0, u1, u2;
        u = year;
        if (month < 3)
        {
            u -= 1;
        }

        u0 = u + 4712.0;
        u1 = month + 1.0;
        if (u1 < 4)
        {
            u1 += 12.0;
        }

        jd = Math.Floor(u0 * 365.25)
           + Math.Floor(30.6 * u1 + 0.000001)
           + day + hour / 24.0 - 63.5;
        if (calendar == DateCalendar.Gregorian)
        {
            u2 = Math.Floor(Math.Abs(u) / 100) - Math.Floor(Math.Abs(u) / 400);
            if (u < 0.0)
            {
                u2 = -u2;
            }

            jd = jd - u2 + 2;
            if (u < 0.0 && u / 100 == Math.Floor(u / 100) && u / 400 != Math.Floor(u / 400))
            {
                jd -= 1;
            }
        }
        return jd;
    }

    /// <summary>
    /// Get the day of the week of a Julian Day
    /// </summary>
    public static WeekDay DayOfWeek(double jd) => (WeekDay)(((int)Math.Floor(jd - 2433282 - 1.5) % 7 + 7) % 7);

    /// <summary>
    /// Convert a Julian Day to a DateTime
    /// </summary>
    /// <param name="jd">The Julian day.</param>
    /// <param name="calendar">The calendar.</param>
    /// <param name="year">The year result.</param>
    /// <param name="month">The month result.</param>
    /// <param name="day">The day result.</param>
    /// <param name="hour">The hour result.</param>
    /// <param name="minute">The minute result.</param>
    /// <param name="second">The second result.</param>
    /// <remarks>
    /// <para>Based on swe_rev_jul()</para>
    /// <para>swe_revjul() is the inverse function to swe_julday(), see the description there.</para>
    /// <para>
    /// Be aware the we use astronomical year numbering for the years before Christ, not the
    /// historical year numbering. Astronomical years are done with negative numbers, historical
    /// years with indicators BC or BCE (before common era).
    /// </para>
    /// <para>
    /// Year 0 (astronomical) = 1 BC historical year <br/> year -1 (astronomical) = 2 BC historical
    /// year <br/> year -234 (astronomical) = 235 BC historical year etc.
    /// </para>
    /// <para>
    /// Original author Mark Pottenger, Los Angeles. with bug fix for year &lt; -4711 16-aug-88
    /// Alois Treindl
    /// </para>
    /// </remarks>
    public static void ToDate(
        double jd,
        DateCalendar calendar,
        out int year, out int month, out int day,
        out int hour, out int minute, out int second)
    {
        double u0, u1, u2, u3, u4;
        u0 = jd + 32082.5;
        if (calendar == DateCalendar.Gregorian)
        {
            u1 = u0 + Math.Floor(u0 / 36525.0) - Math.Floor(u0 / 146100.0) - 38.0;
            if (jd >= 1830691.5)
            {
                u1 += 1;
            }

            u0 = u0 + Math.Floor(u1 / 36525.0) - Math.Floor(u1 / 146100.0) - 38.0;
        }
        u2 = Math.Floor(u0 + 123.0);
        u3 = Math.Floor((u2 - 122.2) / 365.25);
        u4 = Math.Floor((u2 - Math.Floor(365.25 * u3)) / 30.6001);
        month = (int)(u4 - 1.0);
        if (month > 12)
        {
            month -= 12;
        }

        day = (int)(u2 - Math.Floor(365.25 * u3) - Math.Floor(30.6001 * u4));
        year = (int)(u3 + Math.Floor((u4 - 2.0) / 12.0) - 4800);
        double jut = (jd - Math.Floor(jd + 0.5) + 0.5) * 24.0;
        jut += 0.5 / 3600.0;
        hour = (int)jut;
        minute = (int)Math.Floor(jut * 60.0 % 60);
        second = (int)Math.Floor(jut * 3600.0 % 60);
    }

    /// <summary>
    /// Convert a DateUT to a Julian Day
    /// </summary>
    /// <param name="date">Date to convert</param>
    /// <param name="calendar">Calendar to use</param>
    /// <returns>The Julian Day</returns>
    public static double FromDate(UniversalTime date, DateCalendar calendar) =>
        FromDate(date.Year, date.Month, date.Day, date.Hours, date.Minutes, date.Seconds, calendar);

    /// <summary>
    /// Convert a DateUT to a Julian Day
    /// </summary>
    /// <param name="date">Date to convert</param>
    /// <returns>The Julian Day</returns>
    public static double FromUniversalTime(UniversalTime date)
    {
        DateCalendar calendar = DateCalendarExtensions.GetCalendar(date.Year, date.Month, date.Day);
        return FromDate(
            date.Year, date.Month, date.Day,
            date.Hours, date.Minutes, date.Seconds,
            calendar);
    }

    /// <summary>
    /// Get default calendar from a Julian Day
    /// </summary>
    /// <remarks>Gregorian calendar start at October 15, 1582</remarks>
    public static DateCalendar GetCalendar(double jd) => jd < GregorianFirstJD ? DateCalendar.Julian : DateCalendar.Gregorian;

    /// <summary>
    /// Convert a Julian Day to a DateUT in Universal Time (UT).
    /// </summary>
    /// <param name="jd">
    /// The Julian Day to convert, which is a double representing the Julian Day number.
    /// </param>
    /// <param name="calendar">
    /// The calendar to use for the conversion. If null, the default calendar based on the Julian
    /// Day will be used.
    /// </param>
    /// <returns>The DateUT representing the Julian Day in Universal Time (UT).</returns>
    public static UniversalTime ToUniversalTime(double jd, DateCalendar? calendar = null)
    {
        calendar ??= GetCalendar(jd);
        ToDate(jd, calendar.Value, out int year, out int month, out int day, out int hour, out int minute, out int second);
        return new UniversalTime(year, month, day, hour, minute, second);
    }
}
