using RiskFirst.Hateoas.Models;

namespace WebApplication.Presentation.Models
{
    public class Actor : LinkContainer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int[] Movies { get; set; }
    }
}