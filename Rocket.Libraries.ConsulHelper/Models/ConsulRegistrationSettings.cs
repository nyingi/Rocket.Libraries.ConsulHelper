using Rocket.Libraries.ConsulHelper.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.ConsulHelper.Models
{
    public class ConsulRegistrationSettings
    {
        private ConsulCheck _check;

        public string Name { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }

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

        public Weights Weights { get; set; }

        public string ConsulUrl { get; set; }

        internal string AddressWithoutTailingSlash => UrlHelper.RemoveTrailingSlashIfRequired(Address);
    }
}