using System;

namespace WebApplication.Data
{
    public class ServerDescriptor
    {
        public string Url { get; set; }
        public Guid Id { get; }

        public ServerDescriptor()
        {
            Id = Guid.NewGuid();
        }
    }
}