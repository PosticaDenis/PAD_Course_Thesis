using System.Collections.Specialized;
using System.Net;

namespace LoadBalancer
{
    public static class CopyHelper
    {
        public static void CopyHeaders(NameValueCollection nameValueCollection, WebHeaderCollection webHeaderCollection)
        {
            foreach (string requestHeader in nameValueCollection)
            {
                webHeaderCollection.Add(requestHeader, nameValueCollection[requestHeader]);
            }
        }

        public static void CopyHeaders(WebHeaderCollection webHeaderCollection, WebHeaderCollection headerCollection)
        {
            foreach (string webResponseHeader in webHeaderCollection)
            {
                headerCollection.Add(webResponseHeader, webHeaderCollection[webResponseHeader]);
            }
        }

        public static void CopyRequestDetails(WebRequest webRequest, HttpListenerRequest request)
        {
            webRequest.Method = request.HttpMethod;
            webRequest.ContentLength = request.ContentLength64;
            webRequest.ContentType = request.ContentType;
        }
    }
}