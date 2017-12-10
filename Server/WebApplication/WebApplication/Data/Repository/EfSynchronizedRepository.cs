using System;
using System.Collections.Generic;
using System.Linq;
using MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Data.Entities;
using WebApplication.Data.Events;

namespace WebApplication.Data.Repository
{
    public abstract class EfSynchronizedRepository<T, TEvent> : IRepository<T>, IEventSynchronizer<T, TEvent>
        where T : class, IEntity where TEvent : IEventEntity
    {
        private readonly DatabaseApplicationContext _databaseContext;
        private readonly MessageBroker _messageBroker;
        private readonly ILogger<IEventSynchronizer<T, TEvent>> _logger;
        protected virtual IQueryable<T> DbSet => _databaseContext.Set<T>();

        public EfSynchronizedRepository(DatabaseApplicationContext databaseContext, MessageBroker messageBroker, ILogger<IEventSynchronizer<T, TEvent>> logger)
        {
            _databaseContext = databaseContext;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public string InsertQueue => typeof(T).Name + "-insert";

        public string UpdateQueue => typeof(T).Name + "-update";

        public string DeleteQueue => typeof(T).Name + "-delete";

        public T Get(Guid id)
        {
            return DbSet.First(entity => entity.Id == id);
        }

        public IEnumerable<T> Get()
        {
            return DbSet.ToList();
        }

        public T Insert(T entity, bool createEvent = true)
        {
            _databaseContext.Add(entity);
            _databaseContext.SaveChanges();
            if (createEvent)
            {
                _logger.LogInformation("Publish insert event for "+entity.Id);
                _messageBroker.Publish(InsertQueue, new EntityInsertEvent<TEvent>()
                {
                    Entity = CreateEventModel(entity)
                });
            }

            return entity;
        }

        public T Update(T entity, bool createEvent = true)
        {
            _databaseContext.Update(entity);
            _databaseContext.SaveChanges();

            if (createEvent)
            {
                _logger.LogInformation("Publish update event for "+entity.Id);
                _messageBroker.Publish(UpdateQueue, new EntityUpdatedEvent<TEvent>()
                {
                    Entity = CreateEventModel(entity)
                });
            }

            return entity;
        }

        public void Delete(T entity, bool createEvent = true)
        {
            _databaseContext.Remove(entity);
            _databaseContext.SaveChanges();

            if (createEvent)
            {
                _logger.LogInformation("Publish delete event for "+entity.Id);
                _messageBroker.Publish(DeleteQueue, new EntityDeletedEvent
                {
                    Id = entity.Id
                });
            }
        }

        public void OnInsertEvent(EntityInsertEvent<TEvent> @event)
        {
            _logger.LogInformation("Received insert event for "+@event.Entity.Id);
            var entity = CreateEntity();
            UpdateEntity(@event.Entity, entity, true);
            Insert(entity, false);
        }

        public void OnUpdateEvent(EntityUpdatedEvent<TEvent> @event)
        {
            _logger.LogInformation("Received update event for "+@event.Entity.Id);
            var eventEntity = @event.Entity;
            var entity = Get(eventEntity.Id);
            UpdateEntity(eventEntity, entity, false);
            Update(entity, false);
        }

        public void OnDeleteEvent(EntityDeletedEvent @event)
        {
            _logger.LogInformation("Received delete event for "+@event.Id);
            var entity = Get(@event.Id);

            Delete(entity, false);
        }

        public abstract TEvent CreateEventModel(T entity);

        public abstract void UpdateEntity(TEvent @event, T entity, bool copyId);

        public abstract T CreateEntity();
    }
}