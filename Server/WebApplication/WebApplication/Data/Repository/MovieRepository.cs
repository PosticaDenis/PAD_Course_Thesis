using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Entities;

namespace WebApplication.Data.Repository
{
    public class MovieRepository : EfRepository<Movie>, IRepository<Movie>
    {
        public MovieRepository(DatabaseApplicationContext databaseContext) : base(databaseContext)
        {
        }

        protected override IQueryable<Movie> DbSet => base.DbSet.Include(p => p.Actors);
    }
}