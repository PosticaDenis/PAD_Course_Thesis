using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using LoadBalancer.LoadDistribution;
using MessageBus.Events;

namespace LoadBalancer
{
    class Program
    {
        private static MessageBus.MessageBroker _messageBroker;
        private static HttpListener _httpListener;
        
        static void Main(string[] args)
        {
            _messageBroker = new MessageBus.MessageBroker(Environment.GetEnvironmentVariable("MessageBrokerConnectionString"));
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(Environment.GetEnvironmentVariable("LoadBalancerListenUrl"));

            var loadDistrubution = GetLoadDistribution();
            var loadBalancerListener = new LoadBalancerListener(_httpListener,loadDistrubution);
            _messageBroker.Subscribe<ServerUpEvent>("server", serverUpEvent =>
            {
                Console.WriteLine($"Server up {serverUpEvent.Url}");
                loadDistrubution.Add(new Uri(serverUpEvent.Url));
            });
            loadBalancerListener.Listen();
        }

        private static RoundRobinLoadDistribution GetLoadDistribution()
        {
            return new RoundRobinLoadDistribution();
        }
    }
}