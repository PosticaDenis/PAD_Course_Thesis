using WebApplication.Data.Entities;
using WebApplication.Data.Repository;

namespace WebApplication.Domain.Services
{
    public class ActorService : AbstractService<Actor> , IActorService
    {
        public ActorService(IRepository<Actor> repository) : base(repository)
        {
        }
    }
}