using WebApplication.Data.Entities;
using WebApplication.Data.Events;

namespace WebApplication.Data.Repository
{
    public interface IActorRepository : IRepository<Actor>, IEventSynchronizer<Actor, ActorEventEntity>
    {
        
    }
}