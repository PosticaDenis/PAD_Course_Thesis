using System;

namespace WebApplication.Data.Events
{
    public class MovieEventEntity : IEventEntity
    {
        public Guid Id { get; set; }
       
        public string Title { get; set; }

        public int ReleasedYear { get; set; }

        public decimal Sales { get; set; }

        public decimal Rating { get; set; }

        public Guid[] Actors { get; set; }
    }
}