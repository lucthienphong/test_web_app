using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core
{
    public class DateTimeHelper
    {
        public static DateTime MinDateTimeValue = new DateTime(1945, 1, 1);
        public static DateTime MaxDateTimeValue = DateTime.MaxValue;

        /// <summary>
        /// Add the last time of day. Time value in after change is 23:59:59
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime AddLastTimeOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        /// <summary>
        /// Add the first time of day. Time value in after change is 00:00:00
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime AddFirstTimeOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        /// <summary>
        /// Add the first day of month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime AddFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }

        public static bool IsBetween(DateTime fromDate, DateTime toDate, DateTime date)
        {
            if (fromDate < date && date < toDate)
                return true;
            return false;
        }

        public static string GetMonthWithCodeHax()
        {
            string result = string.Empty;
            DateTime date = DateTime.Now;
            switch (date.Month)
            {
                case 10:
                    result = "0";
                    break;
                case 11:
                    result = "A";
                    break;
                case 12:
                    result = "B";
                    break;
                default:
                    result = date.Month.ToString();
                    break;
            }
            return result;
        }
    }
}
