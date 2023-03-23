namespace Fta.CarbonAware.Web.Models
{
    public class EmissionsForecastDto
    {
        public DataPointDto OptimalDataPoint { get; set; } = default!;

        public DataPointDto ClosestForecastDataPoint { get; set; } = default!;
    }
}
