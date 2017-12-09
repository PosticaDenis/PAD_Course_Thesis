using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Entities;

namespace WebApplication.Data
{
    public class DatabaseApplicationContext : DbContext
    {
        protected DatabaseApplicationContext()
        {
        }

        public DatabaseApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>();
            modelBuilder.Entity<Actor>();
            modelBuilder.Entity<ActorMovie>()
                .HasKey(am => new
                {
                    am.ActorId,
                    am.MovieId
                });

        }
    }
}