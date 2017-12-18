using Microsoft.Extensions.Logging;
using Actor = WebApplication.Data.Entities.Actor;

namespace WebApplication.Data.Repository
{
    public class ActorRepository : EfRepository<Actor>, IActorRepository
    {
        public ActorRepository(DatabaseApplicationContext databaseContext, ILogger<IRepository<Actor>> logger) : base(databaseContext, logger)
        {
        }
    }
}