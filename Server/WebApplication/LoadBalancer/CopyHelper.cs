using System.Collections.Specialized;
using System.Net;

namespace LoadBalancer
{
    public static class CopyHelper
    {
        private static readonly int BufferSize = 1024 * 1024 * 5; // 5mb buffer;
        
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

        public static void CopyInputStream(WebRequest webRequest, HttpListenerRequest request)
        {
            byte[] buffer = new byte[BufferSize];
            if (request.InputStream.CanRead && request.InputStream.Length > 0)
            {
                var i = request.InputStream.Read(buffer, 0, BufferSize);
                webRequest.GetRequestStream().Write(buffer, 0, i);
            }
        }

        public static void CopyResponse(WebResponse webResponse, HttpListenerResponse httpListenerResponse)
        {
            byte[] buffer = new byte[BufferSize];
            var read = webResponse.GetResponseStream().Read(buffer, 0, buffer.Length);
            httpListenerResponse.OutputStream.Write(buffer, 0, read);
            httpListenerResponse.OutputStream.Close();
            httpListenerResponse.Close();
        }

        public static void CopyRequestDetails(WebRequest webRequest, HttpListenerRequest request)
        {
            webRequest.Method = request.HttpMethod;
            webRequest.ContentLength = request.ContentLength64;
            webRequest.ContentType = request.ContentType;
        }
    }
}