using Fta.CarbonAware.Web.Models;
using System.Threading.Tasks;
using System.Threading;
using Fta.CarbonAware.Library.Models;

namespace Fta.CarbonAware.Web.Interfaces
{
    public interface IEmissionsService
    {
        Task<Result<EmissionsForecastDto>> GetCurrentForecastAsync(CancellationToken ct = default);
    }
}
