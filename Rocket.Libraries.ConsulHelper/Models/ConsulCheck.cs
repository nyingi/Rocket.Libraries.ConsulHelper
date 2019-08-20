namespace Rocket.Libraries.ConsulHelper.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class is used as a container for information to help Consul perform health checks on the registered service.
    /// </summary>
    public class ConsulCheck
    {
        /// <summary>
        /// Gets or sets a value that specifies that checks associated with a service should deregister after this time. This is specified as a time duration with suffix like "10m". If a check is in the critical state for more than this configured value, then its associated service (and all of its associated checks) will automatically be deregistered. The minimum timeout is 1 minute.
        /// </summary>
        public string DeregisterCriticalServiceAfter { get; set; }

        /// <summary>
        /// Gets or sets relative url to the endpoint to be called for health checks while using the HTTP protocol on your service. MUST NOT include the base path of your service.
        /// </summary>
        public string HttpHealth { get; set; }

        /// <summary>
        /// Gets full url to the endpoint to be called for health checks while using the HTTP protocol on your service.
        /// You usually won't set this value yourself, but is built for you by the library based on other settings you've entered.
        /// </summary>
        public string HTTP { get; internal set; }

        /// <summary>
        /// Gets or sets the the frequency at which to run health check.
        /// </summary>
        public string Interval { get; set; }
    }
}