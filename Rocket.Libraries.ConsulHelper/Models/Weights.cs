namespace Rocket.Libraries.ConsulHelper.Models
{
    /// <summary>
    /// Information about the weights assigned to services to allow load balancing and determining health of services.
    /// </summary>
    public class Weights
    {
        /// <summary>
        /// Gets or sets count of passing checks for a given service.
        /// </summary>
        public int Passing { get; set; }

        /// <summary>
        /// Gets or sets count of warnings for a given service.
        /// </summary>
        public int Warning { get; set; }
    }
}