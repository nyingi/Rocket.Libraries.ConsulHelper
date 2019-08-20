namespace Rocket.Libraries.ConsulHelper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class contains methods that help format url to ensure that they are valid.
    /// </summary>
    internal static class UrlHelper
    {
        /// <summary>
        /// This method ensures that url has a trailing slash.
        /// </summary>
        /// <param name="url">The url to be checked.</param>
        /// <returns><paramref name="url"/> that ends with a '/'.</returns>
        public static string AppendTrailingSlashIfRequired(string url)
        {
            if (HasTrailingSlash(url))
            {
                return url;
            }
            else
            {
                return $"{url}/";
            }
        }

        /// <summary>
        /// Removes a leading slash on a url. This is typically used with relative urls.
        /// </summary>
        /// <param name="url">The url to trim leading slash from.</param>
        /// <returns>A url without a leading slash.</returns>
        public static string RemoveLeadingSlashIfRequired(string url)
        {
            if (HasLeadingSlash(url))
            {
                return url.Substring(1);
            }
            else
            {
                return url;
            }
        }

        /// <summary>
        /// Removes slash at the end of url if one is found.
        /// </summary>
        /// <param name="url">The url to trim slash from.</param>
        /// <returns>Url without a trailing slash.</returns>
        public static string RemoveTrailingSlashIfRequired(string url)
        {
            if (HasTrailingSlash(url))
            {
                return url.Substring(0, url.Length - 1);
            }
            else
            {
                return url;
            }
        }

        private static bool HasLeadingSlash(string url)
        {
            var urlIsBlank = string.IsNullOrEmpty(url);
            if (urlIsBlank)
            {
                return false;
            }
            else
            {
                return url.StartsWith("/", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private static bool HasTrailingSlash(string url)
        {
            var urlIsBlank = string.IsNullOrEmpty(url);
            if (urlIsBlank)
            {
                return false;
            }
            else
            {
                return url.EndsWith("/", StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}