namespace WebApplication.Data.Entities
{
    public class Actor : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}