using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imigration.Application.Extensions
{
    public static class DateExtensions
    {
        public static string ToShamsi(this DateTime date)
        {
            var persianCalender = new PersianCalendar();

            return $"{persianCalender.GetYear(date)}/{persianCalender.GetMonth(date).ToString("00")}/{persianCalender.GetDayOfMonth(date).ToString("00")}";
        }
        public static DateTime ToMiladi(this string date)
        {
            var splitedDate = date.Split("/");

            var year  = Convert.ToInt32(splitedDate[0]);
            var month = Convert.ToInt32(splitedDate[1]);
            var day = Convert.ToInt32(splitedDate[2]);
            //var hour = splitedDate[3];
            //var minute = splitedDate[4];
            //var second = splitedDate[5];
            return new DateTime(year, month, day, new PersianCalendar());

        }
    }
}
