using WebApplication.Data.Entities;
using WebApplication.Data.Events;

namespace WebApplication.Data.Repository
{
    public interface IMovieRepository : IRepository<Movie>, IEventSynchronizer<Movie,MovieEventEntity>
    {
        
    }
}