using Sweph.Net.Chronology;
using System.Globalization;
using System.Text;

namespace Sweph.Net;

/// <summary>
/// Represents a date and time in Universal Time (UT).
/// </summary>
public readonly record struct UniversalTime : IComparable, IComparable<UniversalTime>, IEquatable<UniversalTime>, IFormattable
{
    private const string DefaultFormat = "dd/MM/yyyy HH:mm:ss";

    private readonly int _year, _month, _day, _hours, _minutes, _seconds;

    /// <summary>
    /// New date from components
    /// </summary>
    public UniversalTime(int year, int month, int day, int hours, int minutes, int seconds)
        : this()
    {
        double jd = JulianDay.FromDate(year, month, day, hours, minutes, seconds, DateCalendar.Julian);
        JulianDay.ToDate(jd, DateCalendar.Julian, out _year, out _month, out _day, out _hours, out _minutes, out _seconds);
    }

    /// <summary>
    /// New date from components
    /// </summary>
    public UniversalTime(int year, int month, int day, double hour)
        : this()
    {
        double jd = JulianDay.FromDate(year, month, day, hour, DateCalendar.Julian);
        JulianDay.ToDate(jd, DateCalendar.Julian, out _year, out _month, out _day, out _hours, out _minutes, out _seconds);
    }

    /// <summary>
    /// New date from DateTime
    /// </summary>
    public UniversalTime(DateTime date)
        : this(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second)
    {
    }

    /// <summary>
    /// New date from DateTimeOffset
    /// </summary>
    /// <param name="date"></param>
    public UniversalTime(DateTimeOffset date)
        : this(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second)
    {
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        return obj switch
        {
            UniversalTime ut => CompareTo(ut),
            DateTime dt => CompareTo(new UniversalTime(dt)),
            DateTimeOffset dto => CompareTo(new UniversalTime(dto)),
            _ => -1
        };
    }

    /// <inheritdoc/>
    public int CompareTo(UniversalTime other)
    {
        int result;
        if ((result = Year - other.Year) != 0)
        {
            return result;
        }

        if ((result = Month - other.Month) != 0)
        {
            return result;
        }

        if ((result = Day - other.Day) != 0)
        {
            return result;
        }

        if ((result = Hours - other.Hours) != 0)
        {
            return result;
        }

        return (result = Minutes - other.Minutes) != 0 ? result : (result = Seconds - other.Seconds) != 0 ? result : result;
    }

    /// <summary>
    /// Compare two UniversalTime instances.
    /// </summary>
    /// <param name="date1">The first date to compare.</param>
    /// <param name="date2">The second date to compare.</param>
    /// <returns>
    /// An integer that indicates the relative order of the dates being compared. The return value
    /// has the following meanings:
    /// - Less than zero: <paramref name="date1"/> is earlier than <paramref name="date2"/>.
    /// - Zero: <paramref name="date1"/> is the same as <paramref name="date2"/>.
    /// - Greater than zero: <paramref name="date1"/> is later than <paramref name="date2"/>.
    /// </returns>
    public static int Compare(UniversalTime date1, UniversalTime date2) => date1.CompareTo(date2);

    /// <inheritdoc/>
    public bool Equals(UniversalTime other) => CompareTo(other) == 0;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Year, Month, Day, Hours, Minutes, Seconds);

    /// <inheritdoc/>
    public override string ToString() => ToString(DefaultFormat, null);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            format = DefaultFormat;
        }

        DateTimeFormatInfo? dfi = (formatProvider ?? CultureInfo.CurrentCulture).GetFormat(typeof(DateTimeFormatInfo)) as DateTimeFormatInfo;
        dfi ??= CultureInfo.CurrentCulture.DateTimeFormat;
        StringBuilder result = new();
        int fl = format.Length;
        for (int i = 0; i < fl; i++)
        {
            char c = format[i];
            int cnt;
            switch (c)
            {
                case '\\':
                    i++;
                    _ = i < format.Length ? result.Append(format[i]) : result.Append('\\');

                    break;

                case 'd':
                    cnt = 0;
                    while (i < fl && format[i] == 'd') { cnt++; i++; }
                    i--;
                    double jd = JulianDay.FromUniversalTime(this);
                    int nd = ((int)JulianDay.DayOfWeek(jd)) + 1;
                    if (nd >= 7)
                    {
                        nd -= 7;
                    }

                    if (cnt == 1)
                    {
                        _ = result.Append(Day);
                    }
                    else if (cnt == 2)
                    {
                        _ = result.Append(Day.ToString("D2", CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        _ = cnt == 3 ? result.Append(dfi.AbbreviatedDayNames[nd]) : result.Append(dfi.DayNames[nd]);
                    }

                    break;

                case 'M':
                    cnt = 0;
                    while (i < fl && format[i] == 'M') { cnt++; i++; }
                    i--;
                    if (cnt == 1)
                    {
                        _ = result.Append(Month);
                    }
                    else if (cnt == 2)
                    {
                        _ = result.Append(Month.ToString("D2", CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        _ = cnt == 3 ? result.Append(dfi.AbbreviatedMonthNames[Month - 1]) : result.Append(dfi.MonthNames[Month - 1]);
                    }

                    break;

                case 'y':
                    cnt = 0;
                    while (i < fl && format[i] == 'y') { cnt++; i++; }
                    i--;
                    if (cnt == 1)
                    {
                        _ = result.Append(Year % 100);
                    }
                    else
                    {
                        _ = cnt == 2 ? result.Append((Year % 100).ToString("D2", CultureInfo.CurrentCulture)) : result.Append(Year);
                    }

                    break;

                case 'h':
                    cnt = 0;
                    while (i < fl && format[i] == 'h') { cnt++; i++; }
                    i--;
                    _ = cnt == 1 ? result.Append(Hours % 12) : result.Append((Hours % 12).ToString("D2", CultureInfo.CurrentCulture));

                    break;

                case 'H':
                    cnt = 0;
                    while (i < fl && format[i] == 'H') { cnt++; i++; }
                    i--;
                    _ = cnt == 1 ? result.Append(Hours) : result.Append(Hours.ToString("D2", CultureInfo.CurrentCulture));

                    break;

                case 'm':
                    cnt = 0;
                    while (i < fl && format[i] == 'm') { cnt++; i++; }
                    i--;
                    _ = cnt == 1 ? result.Append(Minutes) : result.Append(Minutes.ToString("D2", CultureInfo.CurrentCulture));

                    break;

                case 's':
                    cnt = 0;
                    while (i < fl && format[i] == 's') { cnt++; i++; }
                    i--;
                    _ = cnt == 1 ? result.Append(Seconds) : result.Append(Seconds.ToString("D2", CultureInfo.CurrentCulture));

                    break;

                case 't':
                    cnt = 0;
                    while (i < fl && format[i] == 't') { cnt++; i++; }
                    i--;
                    string des = Hours < 12 ? dfi.AMDesignator : dfi.PMDesignator;
                    _ = cnt == 1 ? result.Append(des[0]) : result.Append(des);

                    break;
                //case '/':
                //    result.Append(DateTime.MinValue.ToString("/"));
                //    break;
                default:
                    _ = result.Append(c);
                    break;
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// Convert to a DateTime
    /// </summary>
    public DateTime ToDateTime() => new(Year, Month, Day, Hours, Minutes, Seconds);

    /// <summary>
    /// Convert to a DateTimeOffset
    /// </summary>
    public DateTimeOffset ToDateTimeOffset() => new(Year, Month, Day, Hours, Minutes, Seconds, TimeSpan.Zero);

    /// <summary>
    /// Operator &lt;
    /// </summary>
    public static bool operator <(UniversalTime date1, UniversalTime date2) => Compare(date1, date2) < 0;

    /// <summary>
    /// Operator &gt;
    /// </summary>
    public static bool operator >(UniversalTime date1, UniversalTime date2) => Compare(date1, date2) > 0;

    /// <summary>
    /// Operator &lt;=
    /// </summary>
    public static bool operator <=(UniversalTime date1, UniversalTime date2) => Compare(date1, date2) <= 0;

    /// <summary>
    /// Operator &gt;=
    /// </summary>
    public static bool operator >=(UniversalTime date1, UniversalTime date2) => Compare(date1, date2) >= 0;

    /// <summary>
    /// Add offset
    /// </summary>
    public static UniversalTime operator +(UniversalTime date, TimeSpan offset)
    {
        double jd = JulianDay.FromDate(date, DateCalendar.Gregorian);
        jd += offset.TotalDays;
        return JulianDay.ToUniversalTime(jd, DateCalendar.Gregorian);
    }

    /// <summary>
    /// Subtract offset
    /// </summary>
    public static UniversalTime operator -(UniversalTime date, TimeSpan offset)
    {
        double jd = JulianDay.FromDate(date, DateCalendar.Gregorian);
        jd -= offset.TotalDays;
        return JulianDay.ToUniversalTime(jd, DateCalendar.Gregorian);
    }

    /// <summary>
    /// Gets the day of the week for this Universal Time.
    /// </summary>
    public int Day => _day;

    /// <summary>
    /// Gets the day of the week for this Universal Time.
    /// </summary>
    public int Month => _month;

    /// <summary>
    /// Gets the year component of the Universal Time.
    /// </summary>
    public int Year => _year;

    /// <summary>
    /// Gets the hours component of the Universal Time.
    /// </summary>
    public int Hours => _hours;

    /// <summary>
    /// Gets the minutes component of the Universal Time.
    /// </summary>
    public int Minutes => _minutes;

    /// <summary>
    /// Gets the seconds component of the Universal Time.
    /// </summary>
    public int Seconds => _seconds;

    /// <summary>
    /// Adds two UniversalTime instances together.
    /// </summary>
    /// <param name="left">The left UniversalTime to add.</param>
    /// <param name="right">The right UniversalTime to add.</param>
    /// <returns>
    /// A new UniversalTime instance that represents the sum of the two UniversalTime instances.
    /// </returns>
    public static UniversalTime Add(UniversalTime left, UniversalTime right)
    {
        double jdLeft = JulianDay.FromDate(left, DateCalendar.Gregorian);
        double jdRight = JulianDay.FromDate(right, DateCalendar.Gregorian);
        double jdResult = jdLeft + (jdRight - JulianDay.FromDate(0, 1, 1, 0, 0, 0, DateCalendar.Gregorian));
        return JulianDay.ToUniversalTime(jdResult, DateCalendar.Gregorian);
    }

    /// <summary>
    /// Subtracts one UniversalTime from another.
    /// </summary>
    /// <param name="left">The left UniversalTime to subtract from.</param>
    /// <param name="right">The right UniversalTime to subtract.</param>
    /// <returns>A new UniversalTime representing the result of the subtraction.</returns>
    public static UniversalTime Subtract(UniversalTime left, UniversalTime right)
    {
        double jdLeft = JulianDay.FromDate(left, DateCalendar.Gregorian);
        double jdRight = JulianDay.FromDate(right, DateCalendar.Gregorian);
        double jdResult = jdLeft - (jdRight - JulianDay.FromDate(0, 1, 1, 0, 0, 0, DateCalendar.Gregorian));
        return JulianDay.ToUniversalTime(jdResult, DateCalendar.Gregorian);
    }
}
