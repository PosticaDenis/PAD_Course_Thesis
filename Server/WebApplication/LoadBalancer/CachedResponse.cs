using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace LoadBalancer
{
    public class CachedResponse
    {
        public Dictionary<string,string> Headers { get; set; }
        public byte[] Body { get; set; }
    }
}