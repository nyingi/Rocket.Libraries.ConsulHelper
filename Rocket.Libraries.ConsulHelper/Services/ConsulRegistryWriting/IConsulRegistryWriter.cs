namespace Rocket.Libraries.ConsulHelper.Services.ConsulRegistryWriting
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes functionality to allow writing of service information to Consul. Also facilitates using of ConsulRegistryWriters in dependancy injection.
    /// </summary>
    public interface IConsulRegistryWriter
    {
        /// <summary>
        /// Writes the service's information to Consul.
        /// </summary>
        /// <returns>Nothing.</returns>
        Task RegisterAsync();
    }
}