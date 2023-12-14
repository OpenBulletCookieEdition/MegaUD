using System;
using System.Collections.Generic;
using System.Text;
using MegaApiClientCore.Enums;

namespace MegaApiClientCore.Models
{
    public class Proxy
    {
        public ProxyType Type { get; set; }
        public string ProxyString { get; set; }

        public Proxy(string proxy, ProxyType type)
        {
            ProxyString = proxy;
            Type = type;  
        }

    }
}
