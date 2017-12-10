using System;

namespace WebApplication.Data.Events
{
    public class EntityDeletedEvent
    {
        public Guid Id { get; set; }
    }
}