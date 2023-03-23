using Microsoft.Extensions.DependencyInjection;
using System;
using Fta.CarbonAware.Library.Interfaces;
using Fta.CarbonAware.Library.Services;

namespace Fta.CarbonAware.Library
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCarbonAwareApiLibrary(this IServiceCollection services, string carbonAwareApiBaseUrl) 
        {
            services.AddTransient<IDateTimeProvider, DateTimeService>();

            services
                .AddHttpClient<ICarbonAwareApiClient, CarbonAwareApiClient>(client =>
                {
                    client.BaseAddress = new Uri(carbonAwareApiBaseUrl);
                });

            return services;
        }
    }
}
