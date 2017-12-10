using System;

namespace WebApplication.Data.Events
{
    public class ActorEventEntity : IEventEntity
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid[] Movies { get; set; }
    }
}