using RiskFirst.Hateoas.Models;

namespace WebApplication.Presentation.Models
{
    public class Movie : LinkContainer
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ReleasedYear { get; set; }

        public decimal Sales { get; set; }

        public decimal Rating { get; set; }

        public int[] Actors { get; set; }
    }
}