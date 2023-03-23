using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Fta.CarbonAware.AzFn
{
    public static class CarbonAwareBatchProcessorFunction
    {
        [FunctionName("CarbonAwareBatchProcess")]
        public static void CarbonAwareBatchProcess([ServiceBusTrigger("carbon-aware-batch-queue", Connection = "ServiceBusConnectionListen")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}. Simulating CPU intensive batch operation being processed at the optimal time of the day.");
        }
    }
}
