namespace Rocket.Libraries.ConsulHelper.Services.ConsulRegistryReading
{
    using System.Threading.Tasks;
    using Rocket.Libraries.ConsulHelper.Models;

    /// <summary>
    /// This interface defines methods that should be supported by the ConsulRegistryReaders. It also enables DI of ConsulRegistryReaders.
    /// </summary>
    public interface IConsulRegistryReader
    {
        /// <summary>
        /// Returns raw information about a service registered on Consul. The information is packaged in an object of type <see cref="ConsulRegistrationSettings"/>.
        /// </summary>
        /// <param name="serviceId">The id of the service to look up.</param>
        /// <returns>On success, an object of type <see cref="ConsulRegistrationSettings"/> but on failure, null.</returns>
        Task<ConsulRegistrationSettings> GetServiceRawSettingsAsync(string serviceId);

        /// <summary>
        /// This method retrieve the url of a service as registered on Consul.
        /// </summary>
        /// <param name="serviceId">The id of the service to look up.</param>
        /// <returns>On successful look up, the base url of the service is returned, but on failure, this method returns null.</returns>
        Task<string> GetServiceBaseAddressAsync(string serviceId);
    }
}