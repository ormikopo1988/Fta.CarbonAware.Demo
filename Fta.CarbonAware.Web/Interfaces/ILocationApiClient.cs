using System.Threading.Tasks;
using System.Threading;

namespace Fta.CarbonAware.Web.Interfaces
{
    public interface ILocationApiClient
    {
        Task<bool> IsIpAddressInWestUsRegionAsync(string? ipAddress = null, CancellationToken ct = default);
    }
}
