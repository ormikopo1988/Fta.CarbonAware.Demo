using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fta.CarbonAware.Web.Interfaces;
using Fta.CarbonAware.Web.Services;
using Fta.CarbonAware.Web.Settings;

namespace Fta.CarbonAware.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIpInfoNamedHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<ILocationApiClient, IpInfoApiClient>();

            return services;
        }

        public static IServiceCollection AddCarbonAwareApiSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the settings for "CarbonAwareApi" section as a service for injection from DI container
            var carbonAwareApiSettings = new CarbonAwareApiSettings();
            configuration.Bind(CarbonAwareApiSettings.CarbonAwareApiSectionKey, carbonAwareApiSettings);
            services.AddSingleton(carbonAwareApiSettings);

            return services;
        }

        public static IServiceCollection AddIpInfoApiSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the settings for "IpInfoApi" section as a service for injection from DI container
            var ipInfoApiSettings = new IpInfoApiSettings();
            configuration.Bind(IpInfoApiSettings.IpInfoApiSectionKey, ipInfoApiSettings);
            services.AddSingleton(ipInfoApiSettings);

            return services;
        }
    }
}
