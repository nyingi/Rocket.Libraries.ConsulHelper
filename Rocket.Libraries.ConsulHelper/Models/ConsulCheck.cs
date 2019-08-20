using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.ConsulHelper.Models
{
    public class ConsulCheck
    {
        public string DeregisterCriticalServiceAfter { get; set; }

        public string HttpHealth { get; set; }

        public string HTTP { get; internal set; }

        public string Interval { get; set; }
    }
}