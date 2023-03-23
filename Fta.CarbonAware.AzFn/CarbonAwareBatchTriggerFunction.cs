using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Fta.CarbonAware.Library.Interfaces;
using Fta.CarbonAware.Library.Models;
using Microsoft.Azure.WebJobs;

namespace Fta.CarbonAware.AzFn
{
    public class CarbonAwareBatchTriggerFunction
    {
        private readonly ICarbonAwareApiClient _carbonAwareApiClient;
        private readonly IDateTimeProvider _dateTimeService;
        private readonly ServiceBusClient _serviceBusClient;

        public CarbonAwareBatchTriggerFunction(ICarbonAwareApiClient carbonAwareApiClient,
            IDateTimeProvider dateTimeService,
            ServiceBusClient serviceBusClient)
        {
            _carbonAwareApiClient = carbonAwareApiClient;
            _dateTimeService = dateTimeService;
            _serviceBusClient = serviceBusClient;
        }

        [FunctionName("CarbonAwareBatchTrigger")]
        public async Task CarbonAwareBatchTrigger([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer)
        {
            var currentForecastDataResult = await _carbonAwareApiClient.GetCurrentForecastDataAsync(new EmissionsForecastCurrentOptions
            {
                Location = "westus",
                Duration = 5,
                End = new DateTimeOffset(_dateTimeService.UtcNow.Year, _dateTimeService.UtcNow.Month, _dateTimeService.UtcNow.Day, 23, 59, 59, TimeSpan.Zero)
            });

            var currentForecastData = currentForecastDataResult.Data;

            var optimalDataPoints = currentForecastData.OptimalDataPoints;

            if (optimalDataPoints is not null)
            {
                var optimalDataPoint = optimalDataPoints.SingleOrDefault();

                if (optimalDataPoint is not null)
                {
                    var sender = _serviceBusClient.CreateSender("carbon-aware-batch-queue");

                    var message = new ServiceBusMessage($"C# Timer trigger function executed at: {_dateTimeService.UtcNow}")
                    {
                        ScheduledEnqueueTime = optimalDataPoint.Timestamp
                    };

                    await sender.SendMessageAsync(message);
                }
            }
        }
    }
}
