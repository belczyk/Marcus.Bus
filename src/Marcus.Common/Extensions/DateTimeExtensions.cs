using System;
using System.Globalization;

namespace Marcus.Common
{
    public static class DateTimeExtensions
    {
        public static long ToUnixEpochDate(this DateTime date)
        {
            return (long) Math.Round((date.ToUniversalTime() -
                                      new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }

        public static DateTime BeginingOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime StartOfWeek(this DateTime date)
        {
            var days = date.DayOfWeek - DayOfWeek.Monday;

            if (days < 0)
                days += 7;

            return date.AddDays(-1 * days).Date;
        }

        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.StartOfWeek().AddDays(6);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
        }

        public static string ToMMMM_YYYYText(this DateTime date)
        {
            return date.ToString("MMMM yyyy", new CultureInfo("en-US"));
        }

        public static bool InMonth(this DateTime date, int year, int month)
        {
            return date.Year == year && date.Month == month;
        }

        public static bool InSameMonth(this DateTime date, DateTime date2)
        {
            return date.Year == date2.Year && date.Month == date2.Month;
        }

        public static int WeekOfyear(this DateTime date)
        {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var cal = dfi.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static string ToIsoDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}