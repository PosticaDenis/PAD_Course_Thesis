using System;
using System.Collections.Concurrent;

namespace LoadBalancer.LoadDistribution
{
    public class RoundRobinLoadDistribution : ILoadDistribution
    {
        private readonly ConcurrentQueue<Uri> _uris = new ConcurrentQueue<Uri>();

        public Uri Next()
        {
            if (_uris.TryDequeue(out var res))
            {
                _uris.Enqueue(res);
                return res;
            }

            return null;
        }

        public void Add(Uri uri)
        {
            _uris.Enqueue(uri);
        }
    }
}