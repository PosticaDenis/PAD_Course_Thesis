using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace LoadBalancer
{
    public static class CopyHelper
    {
        public static readonly int BufferSize = 1024 * 1024 * 5; // 5mb buffer;

        public static void CopyHeaders(NameValueCollection nameValueCollection, NameValueCollection webHeaderCollection)
        {
            foreach (string requestHeader in nameValueCollection)
            {
                webHeaderCollection.Add(requestHeader, nameValueCollection[requestHeader]);
            }
        }

        public static void CopyInputStream(WebRequest webRequest, HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return;
            }
            using (System.IO.Stream body = request.InputStream) // here we have data
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
                {
                    var readToEnd = reader.ReadToEnd();

                    using (var outStream = webRequest.GetRequestStream())
                    {
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(outStream, request.ContentEncoding))
                        {
                            writer.Write(readToEnd);
                        }
                    }
                }
            }
        }

        public static void CopyResponse(HttpListenerResponse httpListenerResponse, byte[] buffer, int size)
        {
            httpListenerResponse.OutputStream.Write(buffer, 0, size);
            httpListenerResponse.OutputStream.Close();
            httpListenerResponse.Close();
        }

        public static void CopyRequestDetails(WebRequest webRequest, HttpListenerRequest request)
        {
            webRequest.Method = request.HttpMethod;
            webRequest.ContentLength = request.ContentLength64;
            webRequest.ContentType = request.ContentType;
        }

        public static Dictionary<string, string> ToDictionary(this NameValueCollection col)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var k in col.AllKeys)
            {
                dict.Add(k, col[k]);
            }
            return dict;
        }

        public static NameValueCollection ToNameValueCollection(this Dictionary<string, string> dictionary)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (var keyValuePair in dictionary)
            {
                nameValueCollection.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return nameValueCollection;
        }
    }
}