using System;

namespace WebApplication.Data.Events
{
    public class EntityInsertEvent<T>
    {
        public Guid EmmitedServerId { get; set; }
        public T Entity { get; set; }
    }
}