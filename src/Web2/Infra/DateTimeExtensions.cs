using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infra
{
    public static class DateTimeExtensions
    {
        public static DateTime GetIntervalStart(this DateTime date, string type) 
        {
            DateTime result = date;

            if (type == "day")
                result = new DateTime(date.Year, date.Month, date.Day);

            else if (type == "week")
                result = date.AddDays(-(int)date.DayOfWeek);

            else if (type == "month")
                result = new DateTime(date.Year, date.Month, 1);

            return new DateTime(result.Year, result.Month, result.Day);
        }

        public static DateTime GetIntervalEnd(this DateTime date, string type)
        {
            DateTime result = date;

            if (type == "day")
                result = new DateTime(date.Year, date.Month, date.Day);

            else if (type == "week")
                result = date.AddDays(7 - ((int)date.DayOfWeek + 1));

            else if (type == "month")
                result = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            return new DateTime(result.Year, result.Month, result.Day, 23, 59, 59);
        }

        public static long ToJavascriptTimestamp(this DateTime input)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var time = input.Subtract(new TimeSpan(epoch.Ticks));
            return (long)(time.Ticks / 10000);
        }
    }
}