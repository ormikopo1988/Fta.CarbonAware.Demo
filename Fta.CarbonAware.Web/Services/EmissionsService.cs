using Fta.CarbonAware.Library.Interfaces;
using Fta.CarbonAware.Library.Models;
using Fta.CarbonAware.Web.Interfaces;
using Fta.CarbonAware.Web.Models;
using Fta.CarbonAware.Web.Settings;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.CarbonAware.Web.Services
{
    public class EmissionsService : IEmissionsService
    {
        private readonly ICarbonAwareApiClient _carbonAwareApiClient;
        private readonly CarbonAwareApiSettings _carbonAwareApiSettings;

        public EmissionsService(ICarbonAwareApiClient carbonAwareApiClient, CarbonAwareApiSettings carbonAwareApiSettings)
        {
            _carbonAwareApiClient = carbonAwareApiClient;
            _carbonAwareApiSettings = carbonAwareApiSettings;
        }

        public async Task<Result<EmissionsForecastDto>> GetCurrentForecastAsync(CancellationToken ct = default)
        {
            var currentForecastDataResult = await _carbonAwareApiClient.GetCurrentForecastDataAsync(new EmissionsForecastCurrentOptions
            {
                Location = _carbonAwareApiSettings.Location,
                Duration = _carbonAwareApiSettings.WindowSize
            }, ct);

            if (currentForecastDataResult.Error is not null || currentForecastDataResult.Data is null)
            {
                return new Result<EmissionsForecastDto>
                {
                    Error = currentForecastDataResult.Error ?? new Error
                    {
                        ErrorCode = ErrorCode.Unspecified,
                        Message = "Could not fetch forecast data."
                    }
                };
            }

            var currentForecastData = currentForecastDataResult.Data;

            var result = new Result<EmissionsForecastDto>
            {
                Data = new EmissionsForecastDto()
            };

            var optimalDataPoints = currentForecastData.OptimalDataPoints;

            if (optimalDataPoints is not null)
            {
                var optimalDataPoint = optimalDataPoints.SingleOrDefault();

                if (optimalDataPoint is not null)
                {
                    result.Data.OptimalDataPoint = new DataPointDto
                    {
                        Timestamp = optimalDataPoint.Timestamp,
                        Value = optimalDataPoint.Value
                    };
                }
            }

            var forecastDataPoints = currentForecastData.ForecastData;

            if (forecastDataPoints is not null)
            {
                var closestForecastDataPoint = forecastDataPoints.FirstOrDefault();

                if (closestForecastDataPoint is not null)
                {
                    result.Data.ClosestForecastDataPoint = new DataPointDto
                    {
                        Timestamp = closestForecastDataPoint.Timestamp,
                        Value = closestForecastDataPoint.Value
                    };
                }
            }

            return result;
        }
    }
}
