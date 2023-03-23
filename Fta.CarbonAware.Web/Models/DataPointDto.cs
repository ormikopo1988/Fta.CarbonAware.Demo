using System;

namespace Fta.CarbonAware.Web.Models
{
    public class DataPointDto
    {
        public double Value { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
