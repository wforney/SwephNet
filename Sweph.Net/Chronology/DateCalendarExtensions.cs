namespace Sweph.Net.Chronology;

/// <summary>
/// Extension methods for <see cref="DateCalendar"/> to provide a friendly string representation.
/// </summary>
public static class DateCalendarExtensions
{
    /// <summary>
    /// Get default calendar from a date
    /// </summary>
    /// <remarks>
    /// Gregorian calendar start at October 15, 1582
    /// </remarks>
    public static DateCalendar GetCalendar(int year, int month, int day)
    {
        int date = year * 10000 + month * 100 + day;
        return date >= 15821115 ? DateCalendar.Gregorian : DateCalendar.Julian;
    }

    /// <summary>
    /// Converts a <see cref="DateCalendar"/> to its string representation.
    /// </summary>
    /// <param name="calendar">The calendar to convert.</param>
    /// <returns>A string representing the calendar.</returns>
    public static string ToFriendlyString(this DateCalendar calendar)
    {
        return calendar switch
        {
            DateCalendar.Gregorian => "Gregorian",
            DateCalendar.Julian => "Julian",
            _ => throw new ArgumentOutOfRangeException(nameof(calendar), calendar, null)
        };
    }
}
