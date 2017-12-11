using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using LoadBalancer.LoadDistribution;
using MessageBus.Events;
using StackExchange.Redis;

namespace LoadBalancer
{
    class Program
    {
        private static MessageBus.MessageBroker _messageBroker;
        private static HttpListener _httpListener;
        private static ConnectionMultiplexer _connectionMultiplexer;
        
        static void Main(string[] args)
        {
            _messageBroker = new MessageBus.MessageBroker(Environment.GetEnvironmentVariable("MessageBrokerConnectionString"));
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(Environment.GetEnvironmentVariable("LoadBalancerListenUrl"));
            _connectionMultiplexer = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("RedisHost"));
            

            var loadDistrubution = GetLoadDistribution();
            var loadBalancerListener = new LoadBalancerListener(_httpListener,loadDistrubution, _connectionMultiplexer);
            _messageBroker.Subscribe<ServerUpEvent>("server", serverUpEvent =>
            {
                Console.WriteLine($"Server up {serverUpEvent.Url}");
                loadDistrubution.Add(new Uri(serverUpEvent.Url));
            });
            loadBalancerListener.Listen();
        }

        private static RoundRobinLoadDistribution GetLoadDistribution()
        {
            var roundRobinLoadDistribution = new RoundRobinLoadDistribution();
            return roundRobinLoadDistribution;
        }
    }
}