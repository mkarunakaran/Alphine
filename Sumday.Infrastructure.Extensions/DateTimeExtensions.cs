using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime UtcToEasternStandardTime(this DateTime date)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternDateTime = TimeZoneInfo.ConvertTime(date, easternZone);
            return easternDateTime;
        }
    }
}
