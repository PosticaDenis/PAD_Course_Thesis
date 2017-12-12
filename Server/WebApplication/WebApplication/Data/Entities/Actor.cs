using System;
using System.Collections.Generic;

namespace WebApplication.Data.Entities
{
    public class Actor : IEntity
    {
        public Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
    }
}