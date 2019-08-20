# Rocket.Libraries.ConsulHelper
Dead simple .Net library to register services to Consul and to query Consul for registered services.

## Quick Start
### Installation
Grab the nuget package from https://www.nuget.org/packages/Rocket.Libraries.ConsulHelper/

### 1. Config File
```json
{
  "ConsulSettings": {
    "Name": "ConsulHelper.Example.App1",
    "Port": 5113,
    "ConsulUrl": "http://localhost:8500",
    "Address": "http://localhost",
    "Check": {
      "DeregisterCriticalServiceAfter": "1m",
      "HttpHealth": "api/v1/health",
      "Interval": "10s"
    }
  }
}
```
In your appsettings.json, create a section with the above information, where:
- **Name** - Name of your service as it will be registered on Consul. Incidentally, this will also serve as the serviceId.
- **Port** - Port your service will be listening on.
- **ConsulUrl** - Full url (including port) to Consul.
- **Address** - Base url of your service.
- **Check** - Optional settings that control health checks on your service.
  - **DeregisterCriticalServiceAfter** - A value that specifies that checks associated with a service should deregister after this time.
  - **HttpHealth** - Url to the endpoint to be called for health checks while using the HTTP protocol on your service. **MUST NOT** include the base path of your service.
  - **Interval** - Frequency at which to run health check.

### 2. ASP.Net Core Web App
1. In the **ConfigureServices** method of your **Startup.cs** file, add below.
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // The library will require the HttpClientFactory to enable API calls to Consul.
    services.AddHttpClient();
    
    // Use IOptions Pattern to ensure settings entered in appsettings.json are available to us.
    services.Configure<ConsulRegistrationSettings>(Configuration.GetSection("ConsulSettings"));
    
    // Register a singleton to assist in querying Consul for information on registered services.
    services.AddSingleton<IConsulRegistryReader, ConsulRegistryReader>();
    
    // We take advantage of Hosted Services functionality to allow us to automatically register to Consul once our service is up.
    services.AddHostedService<ConsulRegistryWriter>();
}
```

2. Then in the **Configure** method
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
  loggerFactory.AddConsole(Configuration.GetSection("Logging"));
}
```

### 3. Console App
In your **Program.cs** file
```csharp
internal class Program
{
    public static IServiceProvider ServiceProvider;
    private static void Main(string[] args)
    {
      // Set up dependancy injection
      ServiceProvider = new ServiceCollection()
        .AddLogging()
        .AddHttpClient()
        .Configure<ConsulRegistrationSettings>(configuration.GetSection("ConsulSettings"))
        .AddSingleton<IConsulRegistryReader, ConsulRegistryReader>()
        .AddSingleton<IConsulRegistryWriter, ConsulRegistryWriter>()
        .BuildServiceProvider();
        
        // Get instance of the registration writer.
        var registrar = ServiceProvider.GetService<IConsulRegistryWriter>();
        
        //Manually call the RegisterAsync method
        registrar.RegisterAsync().GetAwaiter().GetResult();
    }
}    
```

### Usage
- Registering to Consul
  - Manually - by obtaining an instance of *IConsulRegistryWriter* we can call it's *RegisterAsync* method and if all settings are valid, then our service gets registered on Consul.
  - Automatically - by registering *ConsulRegistryWriter* as a HostedService (like in the web app example), the *RegisterAsync* gets run **exactly once** when the service is up and running.

- Querying Consul
    - We can obtain an instance of *IConsulRegistryReader* via [dependancy injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2), then call ``` GetServiceBaseAddressAsync(string serviceId)``` to obtain the url of a service identified by **serviceId**. 
    
    For richer information about a given service, then you can call the method ```GetServiceRawSettingsAsync(string serviceId)```
    
### Examples
Currently available example is a Console app. Find it in the 'Examples' directory.
    

