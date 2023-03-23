using Fta.CarbonAware.Library.Interfaces;
using Fta.CarbonAware.Web.Interfaces;
using Fta.CarbonAware.Web.Models;
using Fta.CarbonAware.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.CarbonAware.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmissionsService _emissionsService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IConfiguration _configuration;
        private readonly ILocationApiClient _locationApiClient;

        public HomeController(IEmissionsService emissionsService, IDateTimeProvider dateTimeProvider, IConfiguration configuration, ILocationApiClient locationApiClient)
        {
            _emissionsService = emissionsService;
            _dateTimeProvider = dateTimeProvider;
            _configuration = configuration;
            _locationApiClient = locationApiClient;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var homeViewModel = new HomeViewModel();

            var currentForecastDataResult = await _emissionsService.GetCurrentForecastAsync(ct);

            if (currentForecastDataResult.Data is not null)
            {
                var currentForecastData = currentForecastDataResult.Data;
                var optimalDataPoint = currentForecastData.OptimalDataPoint;

                if (IsItCurrentlyAnOptimalCarbonEmissionsZone(optimalDataPoint))
                {
                    homeViewModel.ShowCreateBooking = true;
                    
                    return View(homeViewModel);
                }

                var closestForecastDataPoint = currentForecastData.ClosestForecastDataPoint;
                
                homeViewModel.ShowCreateBooking = IsClosestCarbonEmissionsValueLessThanOrEqualToTheEmissionsThresholdValue(closestForecastDataPoint);

                var isClientIpAddressInWestUsRegion = await _locationApiClient.IsIpAddressInWestUsRegionAsync(GetClientIpAddress(), ct);

                if (isClientIpAddressInWestUsRegion)
                {
                    ViewBag.BackgroundCssClass = "bg-dark text-light";
                    ViewBag.NavbarCssClass = "navbar-dark";
                    ViewBag.TextCssClass = "text-light";
                }
            }

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetClientIpAddress()
        {
            // Get the client IP address
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Check if the IP address is available
            if (ipAddress == null)
            {
                // Try to get the IP address from the X-Forwarded-For header
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"];
            }

            // For Localhost case
            if (ipAddress == "::1")
            {
                //ipAddress = "62.1.191.64"; // westeurope
                ipAddress = "40.112.243.64"; // westus
            }

            // Return the IP address
            return ipAddress!;
        }

        private bool IsItCurrentlyAnOptimalCarbonEmissionsZone(DataPointDto optimalDataPoint)
        {
            return optimalDataPoint?.Timestamp <= _dateTimeProvider.UtcNow.AddMinutes(5) && optimalDataPoint.Timestamp >= _dateTimeProvider.UtcNow;
        }

        private bool IsClosestCarbonEmissionsValueLessThanOrEqualToTheEmissionsThresholdValue(DataPointDto closestForecastDataPoint)
        {
            return closestForecastDataPoint?.Value <= _configuration.GetValue<int>("EmissionsThresholdValue");
        }
    }
}