using System;

namespace WebApplication.Data.Events
{
    public class EntityUpdatedEvent<T>
    {
        public Guid EmmitedServerId { get; set; }
        public T Entity { get; set; }
    }
}