namespace Rocket.Libraries.ConsulHelper.Services.ConsulRegistryReading
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Rocket.Libraries.ConsulHelper.Models;
    using Rocket.Libraries.ConsulHelper.Services.ConsulRegistryWriting;

    /// <summary>
    /// This class contains functionality to retrieve information about a service registered on Consul.
    /// </summary>
    public class ConsulRegistryReader : IConsulRegistryReader
    {
        private ConsulRegistrationSettings _serviceSettings;

        private IHttpClientFactory _httpClientFactory;

        private ILoggerFactory _loggerFactory;

        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsulRegistryReader"/> class.
        /// </summary>
        /// <param name="serviceSettingsOpts">Settings of the service containing Consul's url among other information.</param>
        /// <param name="httpClientFactory">An instance of IHttpClientFactory that's inject and is used to obtain a HttpClient object to facilitate communication with Consul.</param>
        /// <param name="loggerFactory">An instance of ILoggerFactory fed in via Dependancy Injection that's used to generate a logger.</param>
        public ConsulRegistryReader(IOptions<ConsulRegistrationSettings> serviceSettingsOpts, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
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
        public async Task<string> GetServiceBaseAddressAsync(string serviceId)
        {
            var serviceSettings = await GetServiceRawSettingsAsync(serviceId);
            var serviceSettingsMissing = serviceSettings == null;
            if (serviceSettingsMissing)
            {
                return string.Empty;
            }
            else
            {
                var baseAddress = $"{serviceSettings.AddressWithoutTailingSlash}:{serviceSettings.Port}/";
                Logger.LogNoisyInformation($"Base address of {serviceId} is {baseAddress}");
                return baseAddress;
            }
        }

        /// <inheritdoc/>
        public async Task<ConsulRegistrationSettings> GetServiceRawSettingsAsync(string serviceId)
        {
            Logger.LogNoisyInformation($"Fetching information about service '{serviceId}'");
            var httpClient = _httpClientFactory.CreateClient();
            var request = HttpRequestMessageProvider.Get(
                    HttpMethod.Get,
                    _serviceSettings.ConsulUrl,
                    $"v1/agent/service/{serviceId}");
            var response = await httpClient.SendAsync(request);
            LogResponse(response);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ConsulRegistrationSettings>(responseString);
            }
            else
            {
                return null;
            }
        }

        private void LogResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                Logger.LogNoisyInformation($"Service information reading succeeded");
            }
            else
            {
                Logger.LogNoisyError(null, $"Service information reading failed with status code {response.StatusCode}");
            }
        }
    }
}