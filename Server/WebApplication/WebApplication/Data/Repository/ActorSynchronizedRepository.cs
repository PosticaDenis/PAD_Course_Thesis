using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Data.Entities;
using WebApplication.Data.Events;

namespace WebApplication.Data.Repository
{
    public class ActorSynchronizedRepository : EfSynchronizedRepository<Actor, ActorEventEntity>, IActorRepository
    {
        public ActorSynchronizedRepository(DatabaseApplicationContext databaseContext, MessageBus.MessageBroker messageBroker, ILogger<IEventSynchronizer<Actor, ActorEventEntity>> logger) : base(
            databaseContext, messageBroker, logger)
        {
        }

        protected override IQueryable<Actor> DbSet => base.DbSet.Include(p => p.Movies);

        public override ActorEventEntity CreateEventModel(Actor entity)
        {
            return new ActorEventEntity
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Movies = entity.Movies?.Select(m => m.MovieId).ToArray()
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
            entity.Movies = @event.Movies?.Select(m => new ActorMovie
            {
                MovieId = m
            }).ToList();
        }

        public override Actor CreateEntity()
        {
            return new Actor();
        }
    }
}