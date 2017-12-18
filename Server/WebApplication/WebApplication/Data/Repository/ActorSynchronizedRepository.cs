using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Data.Entities;
using WebApplication.Data.Events;
using WebApplication.Presentation.Models;
using Actor = WebApplication.Data.Entities.Actor;

namespace WebApplication.Data.Repository
{
    public class ActorSynchronizedRepository : EfSynchronizedRepository<Actor, ActorEventEntity>, IActorRepository
    {
        public ActorSynchronizedRepository(DatabaseApplicationContext databaseContext,
            MessageBus.MessageBroker messageBroker,
            ILogger<IEventSynchronizer<Actor, ActorEventEntity>> logger,
            ServerDescriptor descriptor) : base(
            databaseContext, messageBroker, logger, descriptor)
        {
        }

        public override ActorEventEntity CreateEventModel(Actor entity)
        {
            return new ActorEventEntity
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
            };
        }

        public override void UpdateEntity(ActorEventEntity @event, Actor entity, bool copyId)
        {
            if (copyId)
            {
                entity.Id = @event.Id;
            }

            entity.FirstName = @event.FirstName;
            entity.LastName = @event.LastName;
        }

        public override Actor CreateEntity()
        {
            return new Actor();
        }
    }
}