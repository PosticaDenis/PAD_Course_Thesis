using System;
using RiskFirst.Hateoas.Models;

namespace WebApplication.Presentation.Models
{
    public class Actor : LinkContainer
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}