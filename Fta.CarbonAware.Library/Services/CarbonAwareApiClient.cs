using Fta.CarbonAware.Library.Interfaces;
using Fta.CarbonAware.Library.Models;
using System.Net.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text;
using Fta.CarbonAware.Library.Extensions;
using System.Collections.Generic;

namespace Fta.CarbonAware.Library.Services
{
    public class CarbonAwareApiClient : ICarbonAwareApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CarbonAwareApiClient(HttpClient httpClient, IDateTimeProvider dateTimeProvider)
        {
            _httpClient = httpClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<EmissionsForecastResponse>> GetCurrentForecastDataAsync(EmissionsForecastCurrentOptions emissionsForecastCurrentOptions, CancellationToken ct = default)
        {
            try
            {
                var url = BuildUrl(emissionsForecastCurrentOptions);
                var httpResponseMessage = await _httpClient.GetAsync(url, ct).ConfigureAwait(false);
                
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var emissionsForecastsResponse = await httpResponseMessage.Content.ReadFromJsonAsync<List<EmissionsForecastResponse>>(cancellationToken: ct);

                    if (emissionsForecastsResponse is not null)
                    {
                        return new Result<EmissionsForecastResponse>
                        {
                            Data = emissionsForecastsResponse[0]
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<EmissionsForecastResponse>
                {
                    Error = new Error
                    {
                        ErrorCode = ErrorCode.Unspecified,
                        Message = ex.Message
                    }
                };
            }

            return new Result<EmissionsForecastResponse>
            {
                Error = new Error
                {
                    ErrorCode = ErrorCode.Unspecified,
                    Message = "Unable to get current forecast data."
                }
            };
        }

        private string BuildUrl(EmissionsForecastCurrentOptions emissionsForecastCurrentOptions)
        {
            var result = new StringBuilder($"emissions/forecasts/current");

            result.Append('?');

            result.Append($"location={emissionsForecastCurrentOptions.Location}&");

            if (emissionsForecastCurrentOptions.Start.HasValue)
            {
                result.Append($"dataStartAt={emissionsForecastCurrentOptions.Start.Value.UrlEncode()}&");
            }
            else
            {
                result.Append($"dataStartAt={_dateTimeProvider.UtcNow.AddMinutes(5).UrlEncode()}&");
            }

            if (emissionsForecastCurrentOptions.End.HasValue)
            {
                result.Append($"dataEndAt={emissionsForecastCurrentOptions.End.Value.UrlEncode()}&");
            }
            else
            {
                result.Append($"dataEndAt={_dateTimeProvider.UtcNow.AddHours(1).UrlEncode()}&");
            }

            if (emissionsForecastCurrentOptions.Duration != default)
            {
                result.Append($"windowSize={emissionsForecastCurrentOptions.Duration}");
            }

            return result.ToString().TrimEnd('&');
        }
    }
}
