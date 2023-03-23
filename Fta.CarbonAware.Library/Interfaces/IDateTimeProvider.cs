using System;

namespace Fta.CarbonAware.Library.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
        DateTimeOffset UtcNow { get; }
    }
}
