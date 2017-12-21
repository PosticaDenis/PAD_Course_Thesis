using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LoadBalancer.LoadDistribution;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LoadBalancer
{
    public class LoadBalancerListener
    {
        private readonly HttpListener _httpListener;
        private readonly ILoadDistribution _loadDistribution;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public LoadBalancerListener(HttpListener httpListener, ILoadDistribution loadDistribution, IConnectionMultiplexer connectionMultiplexer)
        {
            _httpListener = httpListener;
            _loadDistribution = loadDistribution;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public void Listen()
        {
            Console.WriteLine("Listening to following profiles:");
            foreach (var prefix in _httpListener.Prefixes)
            {
                Console.WriteLine($"\t{prefix}");
            }
            RemoveCacheByPattern("*");

            _httpListener.Start();
            _httpListener.BeginGetContext(OnContext, null);
            Thread.Sleep(TimeSpan.FromDays(7));
            _httpListener.Stop();
        }

        private void OnContext(IAsyncResult ar)
        {
            var context = _httpListener.EndGetContext(ar);
            _httpListener.BeginGetContext(OnContext, null);
            var request = context.Request;

            var requestUrl = new Uri(context.Request.Url.ToString().ToLower());
            var list = requestUrl.LocalPath.Split("/").Where(s => !string.IsNullOrEmpty(s))
                .Take(2)
                .ToList();
            
            var readingResource = request.HttpMethod == "GET";

            if (list.Count == 2)
            {
                if (!readingResource)
                {
                    var resource = list[0] + "/" + list[1];
                    RemoveCacheByPattern("*"+resource+"*");
                    Console.WriteLine("Invalidating cache for "+resource + " because of method : "+request.HttpMethod);
                }
            }

            var redisKey = requestUrl.PathAndQuery;
            if (readingResource &&
                _connectionMultiplexer.GetDatabase(0).KeyExists(redisKey))
            {
                Console.WriteLine("Returning cached request.");
                var content = _connectionMultiplexer.GetDatabase(0).StringGet(redisKey);
                var cachedResponse = JsonConvert.DeserializeObject<CachedResponse>(content);

                CopyHelper.CopyHeaders(CopyHelper.ToNameValueCollection(cachedResponse.Headers), context.Response.Headers);
                CopyHelper.CopyResponse(context.Response, cachedResponse.Body, cachedResponse.Body.Length);
            }
            else
            {
                var uri = GetRedirectUri(request);

                Console.WriteLine($"Received a request. Redirecting to {uri}");
                var webRequest = WebRequest.Create(uri);
                CopyHelper.CopyRequestDetails(webRequest, request);
                CopyHelper.CopyHeaders(request.Headers, webRequest.Headers);
                CopyHelper.CopyInputStream(webRequest, request);

                var webResponse = webRequest.GetResponse();

                
                byte[] buffer = new byte[CopyHelper.BufferSize];
                var read = webResponse.GetResponseStream().Read(buffer, 0, buffer.Length);
                
                CopyHelper.CopyHeaders(webResponse.Headers, context.Response.Headers);
                CopyHelper.CopyResponse(context.Response, buffer, read);

                if (readingResource)
                {
                    var serializeObject = JsonConvert.SerializeObject(new CachedResponse
                    {
                        Body = buffer.Take(read).ToArray(),
                        Headers = CopyHelper.ToDictionary(webResponse.Headers)
                    });
                    _connectionMultiplexer.GetDatabase(0).StringSet(redisKey, serializeObject);
                }
            }
        }

        private Uri GetRedirectUri(HttpListenerRequest request)
        {
            var redirectUriBuilder = new UriBuilder(request.Url);
            var redirectUri = _loadDistribution.Next();
            redirectUriBuilder.Port = redirectUri.Port;
            redirectUriBuilder.Host = redirectUri.Host;

            return redirectUriBuilder.Uri;
        }

        private void RemoveCacheByPattern(string pattern)
        {
            foreach (var ep in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(ep);
                var redisKeys = server.Keys(pattern:pattern);
                foreach (var redisKey in redisKeys)
                {
                    _connectionMultiplexer.GetDatabase(0).KeyDelete(redisKey);
                }
            }
        }
    }
}