using Fta.CarbonAware.Web.Interfaces;
using Fta.CarbonAware.Web.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Globalization;
using Fta.CarbonAware.Web.Settings;

namespace Fta.CarbonAware.Web.Services
{
    public class IpInfoApiClient : ILocationApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IpInfoApiSettings _ipInfoApiSettings;
        private readonly ILogger<IpInfoApiClient> _logger;

        public IpInfoApiClient(HttpClient httpClient,
            IpInfoApiSettings ipInfoApiSettings,
            ILogger<IpInfoApiClient> logger)
        {
            _httpClient = httpClient;
            _ipInfoApiSettings = ipInfoApiSettings;
            _logger = logger;
        }

        public async Task<bool> IsIpAddressInWestUsRegionAsync(string? ipAddress = null, CancellationToken ct = default)
        {
            try
            {
                var url = string.IsNullOrEmpty(ipAddress) ? 
                    $"{_ipInfoApiSettings.BaseUrl}?token={_ipInfoApiSettings.AccessToken}" : 
                    $"{_ipInfoApiSettings.BaseUrl}{ipAddress}?token={_ipInfoApiSettings.AccessToken}";

                var httpResponseMessage = await _httpClient.GetAsync(url, ct).ConfigureAwait(false);
                var ipInfoResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IpInfo>(cancellationToken: ct);

                if (httpResponseMessage.IsSuccessStatusCode && ipInfoResponse != null)
                {
                    var regionInfo = new RegionInfo(ipInfoResponse.Country);

                    return regionInfo.TwoLetterISORegionName.Equals("US") && regionInfo.GeoId == 244 && ipInfoResponse.Region == "California";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in {ServiceName} -> {MethodName} method.", nameof(IpInfoApiClient), nameof(IsIpAddressInWestUsRegionAsync));
            }

            return false;
        }
    }
}
