using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Entities;

namespace WebApplication.Data
{
    public class DatabaseApplicationContext : DbContext
    {
        protected DatabaseApplicationContext()
        {
            Database.Migrate();
        }

        public DatabaseApplicationContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>();
            modelBuilder.Entity<Actor>();

        }
    }
}