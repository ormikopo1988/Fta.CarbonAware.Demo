# Carbon Aware SDK Demos

Measuring the carbon emissions of your application is the first step to plan your cloud efficiency and optimize its emissions.

There is a slight problem, though: the carbon intensity of the electricity grid varies over time, so ideally, 
if we want to react to how the energy is produced and even enforce a better production of energy over time, 
we should aim at having emissions data in real time. 
Even if I have moved my application to the best, greenest datacenter, 
the ecosystem of the application has a lot more items to account for: the edge, the network transfers, the data transfers, the user devices, etc. 
All this infrastructure is outside the datacenter and will use energy from the local grid, which has different values of carbon intensity across time.

We want to be able to track in real-time how “green” is the energy that powers our application, and, more importantly, 
we want to be able to decide what to do when such intensity is higher (which means the region is using fossil fuel to power the infrastructure, so I might want to provide a limited number of features) 
or lower (this means using renewable sources, and might allow for a richer set of features in my app).

For that, we will examine what is called the [Carbon Aware SDK](https://github.com/Green-Software-Foundation/carbon-aware-sdk).

When software does more when the electricity is clean and do less when the electricity is dirty, or runs in a location where the energy is cleaner, we call this **carbon aware software**.

The Carbon Aware SDK helps you build the carbon aware software solutions with the intelligence to use the greenest energy sources.

There are several ways to consume Carbon Aware data for your use case. Each approach surfaces the same data for the same call:
- You can run the application using the CLI.
- You can run it as a separate WebAPI and connect via REST requests, which we are going to follow in the use-case demos contained in the current repo.
- You can reference the Carbon Aware C# Library in your projects and make use of its functionalities and features.


## Prerequisites

### Data Sources

The Carbon Aware SDK [includes access to various data sources](https://github.com/Green-Software-Foundation/carbon-aware-sdk/blob/dev/docs/selecting-a-data-source.md) of carbon aware data, including WattTime, ElectricityMaps, and a custom JSON source. 

In the CarbonAware SDK configuration, you can set which data source to use as the EmissionsDataSource and which one as the ForecastDataSource.

If set to WattTime or ElectricityMaps, the configuration specific to that data provider must also be supplied.

### Setup the carbon-aware WebApi

The Carbon Aware SDK [provides an API](https://github.com/Green-Software-Foundation/carbon-aware-sdk/blob/dev/docs/carbon-aware-webapi.md) to get the marginal carbon intensity for a given location and time period. 
This user interface is best for when you can change the code, and deploy separately. 
This also allows you to manage the Carbon Aware logic independently of the system using it.

The WebApi replicates the CLI and SDK functionality, leveraging the same configuration and providing a REST end point with Swagger/OpenAPI definition for client generation.

### WattTime Configuration

In our demos, we are going to be using WattTime as our data source both for emissions and for forecast data.
For that reason, [WattTime-specific configuration](https://github.com/Green-Software-Foundation/carbon-aware-sdk/blob/dev/docs/configuration.md#watttime-configuration) 
is required to be set in the appsettings.json file of the carbon-aware sdk WebApi.

```

{
    "username": "",
    "password": "",
    "Type": "WattTime"
}

```

To get those values, we need to sign up for a test account and get a username/password pair, following the respective steps from the [WattTime documentation](https://www.watttime.org/api-documentation/#best-practices-for-api-usage).

To sum up, the high level steps to setup everything are:

- Go to the [Carbon Aware SDK](https://github.com/Green-Software-Foundation/carbon-aware-sdk) and clone the repo in your PC.
- Open the solution with Visual Studio or Visual Studio code.
- If you have a WattTime account registered (or other data source), 
you will need to configure the application to use them. 
By default the SDK will use a pre-generated JSON file with random data. 
To configure the application, you will need to set up specific environment variables or modify appsettings.json inside of src/CarbonAware.WebApi/src directory.
In our case we will be using WattTime as mentioned above, so go ahead and change your appsettings.json file in the following format:

```

"DataSources": {
    "EmissionsDataSource": "WattTime",
    "ForecastDataSource": "WattTime",
    "Configurations": {
        "test-json": {
            "Type": "JSON",
            "DataFileLocation": "test-data-azure-emissions.json"
        },
        "WattTime": {
            "Type": "WattTime",
            "username": "<your-watt-time-username>",
            "password": "<your-watt-time-password>"
        }
    }
}

```

- Deploy the CarbonAware.WebApi project to an App Service in Azure.
- Take the URL of the published Carbon-Aware SDK API, as you will need it for the following demos.

## Demo No1: Carbon-aware web application

### Architecture

![Here, we are seeing an architecture for a carbon aware web application.](/Carbon-Aware-Web-Application-Architecture.png)

On the right hand side of this image you can see the different data sources presented above, which are WattTime API, ElectricityMaps API and other potential data sources.

In the middle we have deployed the Carbon Aware SDK as a separate Web API inside an Azure App Service. In our case, we have configured that to be able to talk to WattTime API, both for fetching emissions and forecast data.

Finally, on the left hand side, we have a very simple MVC NET 6 Web Application, which will play the role of the client that consumes the Carbon Aware SDK API, that exposes the carbon data. 
With this type of integration, we will be in a position of making our application carbon-aware.

In the context of the demo, there are multiple scenarios into play. During all of them, our client will send a request to the Carbon Aware SDK API to ask it whether it is running in the context of an optimal zone 
in regards to carbon emissions. Based on the response, the app will decide whether it will enable a CPU intensive feature or not ("Book Flight").

Also, for the background color of the app, it will be always rendered as white, with one exception. 
If we currently are not in an optimal carbon emissions zone, then our web application is going to also make a call to the [ipinfo.io API](https://ipinfo.io/) 
to check the origin of the client IP of the end user’s browser and will decide which color to render based on the response.

For your case, go into the appsettings.json file of the `Fta.CarbonAware.Web` project, and make the relevant changes:

```

"CarbonAwareApi": {
    "BaseUrl": "<base-url-where-you-published-the-carbon-aware-sdk-api>",
    "Location": "westus",
    "WindowSize": 10
},
"IpInfoApi": {
    "BaseUrl": "https://ipinfo.io/",
    "AccessToken": "<access-token-from-ip-info-io-in-dev-set-it-in-secret-manager>"
}

```

For the "Location" part of the "CarbonAwareApi" section use "westus", as this region, is the only one allowed by the WattTime API, in its free account. 
This would mean, that in our case our client (carbon-aware web application) would also be deployed in "West US" region, 
so it would request the forecast emissions data for this particular region from the Carbon Aware SDK API.

For the "AccessToken" part of the "IpInfoApi" section, you should paste here your own, once you have created an account there. 
You can set this using the Secret Manager, when you are running this from your local machine. In production scenarios, you can use Azure Key Vault.
Be careful to never commit this secret directly in source control.

[Video containing the use case demo](https://clipchamp.com/watch/JdWSIGwP3zi)

## Demo No2: Heavy CPU batch job running in optimal time of day

### Architecture

![Here we can see an architecture for a heavy CPU intensive batch job, which we want to run on the optimal time of the day](/Heavy-CPU-Batch-Job-Architecture.png)

Let’s move to our second demo. 

There are two different functions. The first one, which is called `CarbonAwareBatchTriggerFunction`, will be a timer trigger function. 
Imagine that this will run every day at 12:01 AM. This function when triggered, will call the Carbon aware SDK API to request which time inside the next 24-hour period will be the optimal time point, 
in regards to carbon emissions, for running this batch job. When having received the response back, it will then create a scheduled message to send to a service bus queue. 
For example, if the API responds to the function that the optimal data point is at 5:00 PM in the afternoon, the function will create a scheduled message and will instruct service bus to enqueue it at 5:00 PM.

There is also another function, which is called `CarbonAwareBatchProcessorFunction`, which is a service bus trigger one. 
This will be triggered, whenever the message appears inside the queue of the service bus, it will receive the message and then it would start processing the CPU intensive batch operation, 
at the optimal time of the day. Following our previous example, this would be at 5:00 PM in the afternoon.

For you to run this demo locally, you will need to do the following:
- Create a Service Bus named `carbonawaredemo` in the Azure Portal (for the purpose of the demo, you can put it inside the same resource group as the one you used to publish the Carbon Aware SDK API):
  - Create a queue named `carbon-aware-batch-queue`.
  - Go into the queue and click on `Shared access policies`
  - Create two different shared access policies:
    - The first one called `sas-send` will have only a `Send` claim and will be used by the `CarbonAwareBatchTriggerFunction` function to send the message to the service bus queue.
    - The second one called `sas-listen` will have only a `Listen` claim and will be used by the `CarbonAwareBatchProcessorFunction` function to listen for messages from the service bus queue.
- Create a local.settings.json file inside the `Fta.CarbonAware.AzFn` project and put inside it the following values:
  - **ServiceBusConnectionSend**: Your CfS API Primary Key from the MCfS API Management Portal.
  - **ServiceBusConnectionListen**: Your Azure AD tenant id, which you signed up to enable the MCfS Preview API.
  - **CarbonAwareApiBaseUrl**: The base url where you published the carbon aware sdk api.

```

{
    "IsEncrypted": false,
    "Values": {
      "AzureWebJobsStorage": "UseDevelopmentStorage=true",
      "FUNCTIONS_WORKER_RUNTIME": "dotnet",
      "ServiceBusConnectionSend": "<sas-send-primary-connection-string>",
      "ServiceBusConnectionListen": "<sas-listen-primary-connection-string>",
      "CarbonAwareApiBaseUrl": "<base-url-where-you-published-the-carbon-aware-sdk-api>"
    }
}

```

[Video containing the use case demo](https://clipchamp.com/watch/8cl0lmA3OuD)