using System;

namespace LoadBalancer.LoadDistribution
{
    public interface ILoadDistribution
    {
        Uri Next();

        void Add(Uri uri);
    }
}