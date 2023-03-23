using Fta.CarbonAware.Library.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.CarbonAware.Library.Interfaces
{
    public interface ICarbonAwareApiClient
    {
        Task<Result<EmissionsForecastResponse>> GetCurrentForecastDataAsync(EmissionsForecastCurrentOptions emissionsForecastCurrentOptions, CancellationToken ct = default);
    }
}
