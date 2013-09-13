using System;

namespace ReleaseManager.API.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime LastSecondOfDay(this DateTime date)
        {
            var tomorrow = date.AddDays(1).ToShortDateString();
            var endDate = DateTime.Parse(tomorrow).AddSeconds(-1);

            return endDate;
        }

        public static DateTime FirstSecondOfDay(this DateTime date)
        {
            var yesterday = date.AddDays(-1).ToShortDateString();
            var endDate = DateTime.Parse(yesterday).AddHours(24);

            return endDate;
        }

    }
}