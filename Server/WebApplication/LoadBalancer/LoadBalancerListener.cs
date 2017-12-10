using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LoadBalancer.LoadDistribution;

namespace LoadBalancer
{
    public class LoadBalancerListener
    {
        private readonly HttpListener _httpListener;
        private readonly ILoadDistribution _loadDistribution;

        public LoadBalancerListener(HttpListener httpListener, ILoadDistribution loadDistribution)
        {
            _httpListener = httpListener;
            _loadDistribution = loadDistribution;
        }

        public void Listen()
        {
            Console.WriteLine("Listening to following profiles:");
            foreach (var prefix in _httpListener.Prefixes)
            {
                Console.WriteLine($"\t{prefix}");
            }

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
            var uri = GetRedirectUri(request);

            Console.WriteLine($"Received a request. Redirecting to {uri}");
            var webRequest = WebRequest.Create(uri);
            CopyHelper.CopyRequestDetails(webRequest, request);
            CopyHelper.CopyHeaders(request.Headers, webRequest.Headers);
            CopyHelper.CopyInputStream(webRequest, request);

            var webResponse = webRequest.GetResponse();

            CopyHelper.CopyHeaders(webResponse.Headers, context.Response.Headers);
            CopyHelper.CopyResponse(webResponse, context.Response);
        }

        private Uri GetRedirectUri(HttpListenerRequest request)
        {
            var redirectUriBuilder = new UriBuilder(request.Url);
            var redirectUri = _loadDistribution.Next();
            redirectUriBuilder.Port = redirectUri.Port;
            redirectUriBuilder.Host = redirectUri.Host;

            return redirectUriBuilder.Uri;
        }
    }
}