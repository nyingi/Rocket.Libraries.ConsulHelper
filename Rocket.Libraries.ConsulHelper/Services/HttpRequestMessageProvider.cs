namespace Rocket.Libraries.ConsulHelper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;

    /// <summary>
    /// This class returns a HttpRequestMessage with a well formatted url.
    /// </summary>
    internal static class HttpRequestMessageProvider
    {
        /// <summary>
        /// Returns a HttpRequestMessage with a well formatted url.
        /// </summary>
        /// <param name="method">The HttpMethod intended for the HttpRequestMessage.</param>
        /// <param name="baseUrl">The base part of the url.</param>
        /// <param name="relativeUrlPart">The relative part of the url.</param>
        /// <returns>A HttpRequestMessage with as well formatted url.</returns>
        public static HttpRequestMessage Get(HttpMethod method, string baseUrl, string relativeUrlPart)
        {
            var baseUrlWithTrailingSlash = UrlHelper.AppendTrailingSlashIfRequired(baseUrl);
            var relativeUrlWithoutLeadingSlash = UrlHelper.RemoveLeadingSlashIfRequired(relativeUrlPart);
            var fullUrl = $"{baseUrlWithTrailingSlash}{relativeUrlWithoutLeadingSlash}";
            return new HttpRequestMessage(
                      method,
                      fullUrl);
        }
    }
}