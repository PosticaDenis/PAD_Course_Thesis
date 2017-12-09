namespace WebApplication.Presentation.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public int ReleasedYear { get; set; }
        
        public decimal Sales { get; set; }
        
        public decimal Rating { get; set; }
    }
}