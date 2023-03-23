using Fta.CarbonAware.Library.Interfaces;
using System;

namespace Fta.CarbonAware.Library.Services
{
    public class DateTimeService : IDateTimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
