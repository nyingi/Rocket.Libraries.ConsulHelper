namespace Rocket.Libraries.ConsulHelper.Models
{
    using Rocket.Libraries.ConsulHelper.Services;

    /// <summary>
    /// Information required by the library to allow it to interact successfully with Consul.
    /// </summary>
    public class ConsulRegistrationSettings
    {
        private ConsulCheck _check;

        /// <summary>
        /// Gets or sets the name of your service.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the base url of your service.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the url your service is listening on.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets settings that control health checks on your service.
        /// </summary>
        public ConsulCheck Check
        {
            get
            {
                if (_check == null)
                {
                    return null;
                }
                else
                {
                    _check.HTTP = $"{AddressWithoutTailingSlash}:{Port}/{_check.HttpHealth}";
                    return _check;
                }
            }

            set => _check = value;
        }

        /// <summary>
        /// Gets or sets information about the weights assigned to services to allow load balancing and determining health of services.
        /// </summary>
        public Weights Weights { get; set; }

        /// <summary>
        /// Gets or sets the full url (including port) to Consul.
        /// </summary>
        public string ConsulUrl { get; set; }

        /// <summary>
        /// Gets the address to your services but ensures that it has no trailing slash.
        /// </summary>
        internal string AddressWithoutTailingSlash => UrlHelper.RemoveTrailingSlashIfRequired(Address);
    }
}