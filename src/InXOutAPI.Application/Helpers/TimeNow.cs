using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InXOutAPI.Application.Helpers
{
    public class TimeNow
    {
        public static DateTime UtcPlus7()
        {
            // Mendapatkan zona waktu UTC+7 (Asia/Jakarta)
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Ambil waktu lokal (UTC+7) dan pastikan Kind-nya disetel ke Local
            DateTime localTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

            // Konversi dari UTC+7 ke UTC
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime, timeZoneInfo);

            return utcTime;
        }
    }
}
