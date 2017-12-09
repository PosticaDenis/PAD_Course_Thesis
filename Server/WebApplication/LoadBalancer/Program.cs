using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using LoadBalancer.LoadDistribution;

namespace LoadBalancer
{
    class Program
    {

        static void Main(string[] args)
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(Environment.GetEnvironmentVariable("LoadBalancerListenUrl"));
            
            var loadBalancerListener = new LoadBalancerListener(httpListener, GetLoadDistribution());
            loadBalancerListener.Listen();
        }

        private static RoundRobinLoadDistribution GetLoadDistribution()
        {
            var roundRobinLoadDistribution = new RoundRobinLoadDistribution();
            roundRobinLoadDistribution.Add(new Uri("http://localhost:5000"));
            
            return roundRobinLoadDistribution;
        }
    }
}