using System.Collections.Generic;

namespace WebApplication.Data.Entities
{
    public class Movie : IEntity
    {
        public Movie()
        {
            Actors = new List<ActorMovie>();
        }

        public int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual int ReleasedYear { get; set; }

        public virtual decimal Sales { get; set; }

        public virtual decimal Rating { get; set; }

        public virtual IEnumerable<ActorMovie> Actors { get; set; }
    }
}