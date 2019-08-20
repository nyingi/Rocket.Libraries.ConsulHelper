namespace Rocket.Libraries.ConsulHelper.Services.ConsulRegistryWriting
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Rocket.Libraries.ConsulHelper.Models;

    /// <summary>
    /// This class contains functionality to allow writing of service information to Consul.
    /// </summary>
    public class ConsulRegistryWriter : IHostedService, IConsulRegistryWriter
    {
        private ConsulRegistrationSettings _serviceSettings;

        private IHttpClientFactory _httpClientFactory;

        private ILoggerFactory _loggerFactory;

        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsulRegistryWriter"/> class.
        /// </summary>
        /// <param name="serviceSettingsOpts">Information about the service to be registered.</param>
        /// <param name="httpClientFactory">An instance of IHttpClientFactory that's inject and is used to obtain a HttpClient object to facilitate communication with Consul.</param>
        /// <param name="loggerFactory">An instance of ILoggerFactory fed in via Dependancy Injection that's used to generate a logger.</param>
        public ConsulRegistryWriter(IOptions<ConsulRegistrationSettings> serviceSettingsOpts, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            _serviceSettings = serviceSettingsOpts.Value;
            _httpClientFactory = httpClientFactory;
            _loggerFactory = loggerFactory;
        }

        private ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = _loggerFactory.CreateLogger<ConsulRegistryWriter>();
                }

                return _logger;
            }
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RegisterAsync();
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task RegisterAsync()
        {
            try
            {
                Logger.LogNoisyInformation($"Attempting Registration of {_serviceSettings.Name} to Consul at {_serviceSettings.ConsulUrl}");
                Logger.LogNoisyInformation($"Service Address: {_serviceSettings.Address}");
                Logger.LogNoisyInformation($"Service Port: {_serviceSettings.Port}");
                LogAboutHealthCheck();
                var httpClient = _httpClientFactory.CreateClient();
                var request = HttpRequestMessageProvider.Get(HttpMethod.Put, _serviceSettings.ConsulUrl, "v1/agent/service/register");
                request.Content = new StringContent(JsonConvert.SerializeObject(_serviceSettings), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                LogResponse(response);
            }
            catch (Exception e)
            {
                Logger.LogNoisyError(e, $"Error occured registering service ${_serviceSettings.Name} with Consul");
                Logger.LogNoisyWarning($"Service {_serviceSettings.Name} failed to register with Consul. This may impact performance of other services as they won't be able to locate it");
            }
        }

        private void LogAboutHealthCheck()
        {
            var healthCheckMissing = string.IsNullOrEmpty(_serviceSettings.Check.HttpHealth);
            if (healthCheckMissing)
            {
                Logger.LogNoisyWarning($"No health-check specified. This isn't necessarily catastrophic but because of this, Consul will continue serving up this service's url even when the service is not available");
            }
            else
            {
                Logger.LogNoisyInformation($"Health check for service is {_serviceSettings.Check.HTTP}");
            }
        }

        private void LogResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                Logger.LogNoisyInformation($"Service Registration With Consul Succeeded");
            }
            else
            {
                Logger.LogNoisyError(null, $"Service Registration With Consul Failed With Status Code {response.StatusCode}");
            }
        }
    }
}