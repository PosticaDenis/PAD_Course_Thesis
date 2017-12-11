using System;

namespace WebApplication.Data.Events
{
    public class EntityDeletedEvent
    {
        public Guid EmmitedServerId { get; set; }
        public Guid Id { get; set; }
    }
}