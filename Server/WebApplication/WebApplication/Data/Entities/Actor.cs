using System;
using System.Collections.Generic;

namespace WebApplication.Data.Entities
{
    public class Actor : IEntity
    {
        public Actor()
        {
            Movies = new List<ActorMovie>();
        }

        public Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual IEnumerable<ActorMovie> Movies { get; set; }
    }
}