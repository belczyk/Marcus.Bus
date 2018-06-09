using System;

namespace Marcus.Common
{
    public static class Calendar
    {
        public static DateTime NowCES
            =>
                TimeZoneInfo.ConvertTime(DateTime.Now,
                    TimeZoneInfo.FromSerializedString(
                        "Central European Standard Time;60;(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb;Central European Standard Time;Central European Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];"));


        public static void ForEachMonth(int startYear, int startMonth, Action<int, int> action, int? endYear = null,
            int? endMonth = null)
        {
            var lastYear = endYear ?? DateTime.Now.Year;
            var lastMonth = endMonth ?? DateTime.Now.Month;

            for (var year = startYear; year <= lastYear; year++)
            for (var month = 1; month <= 12; month++)
            {
                if (year == startYear && month < startMonth || year == lastYear && month > lastMonth)
                    continue;

                action(year, month);
            }
        }

        public static DateTime BeginingOfMonth(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime EndOfMonth(int year, int month)
        {
            return new DateTime(year, month,
                DateTime.DaysInMonth(year, month));
        }
    }
}