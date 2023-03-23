using System;
using System.Web;

namespace Fta.CarbonAware.Library.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static string UrlEncode(this DateTimeOffset dateTimeOffset)
        {
            return HttpUtility.UrlEncode(dateTimeOffset.ToString("o"));
        }
    }
}
