namespace WebApplication.Data.Entities
{
    public class Movie : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int ReleasedYear { get; set; }
        
        public decimal Sales { get; set; }
        
        public decimal Rating { get; set; }
    }
}