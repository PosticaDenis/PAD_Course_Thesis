using System;

namespace WebApplication.Data.Entities
{
    public class ActorMovie
    {
        public Guid ActorId { get; set; }
        public Actor Actor { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}