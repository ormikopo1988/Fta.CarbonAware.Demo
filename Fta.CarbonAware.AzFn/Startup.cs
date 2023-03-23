using Fta.CarbonAware.Library;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(Fta.CarbonAware.AzFn.Startup))]
namespace Fta.CarbonAware.AzFn
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

            builder.Services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClient(configuration.GetValue<string>("ServiceBusConnectionSend"));
            });

            builder.Services.AddCarbonAwareApiLibrary(configuration.GetValue<string>("CarbonAwareApiBaseUrl"));
        }
    }
}

