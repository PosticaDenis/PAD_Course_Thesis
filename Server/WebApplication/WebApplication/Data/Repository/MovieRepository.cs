using Microsoft.Extensions.Logging;
using WebApplication.Data.Entities;

namespace WebApplication.Data.Repository
{
    public class MovieRepository : EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(DatabaseApplicationContext databaseContext, ILogger<IRepository<Movie>> logger) : base(databaseContext, logger)
        {
        }
    }
}