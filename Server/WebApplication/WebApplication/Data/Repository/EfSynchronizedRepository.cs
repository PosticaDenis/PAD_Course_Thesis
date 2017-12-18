using System;
using System.Collections.Generic;
using System.Linq;
using MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Data.Entities;
using WebApplication.Data.Events;
using WebApplication.Presentation.Models;

namespace WebApplication.Data.Repository
{
    public abstract class EfSynchronizedRepository<T, TEvent> : IRepository<T>, IEventSynchronizer<T, TEvent>
        where T : class, IEntity where TEvent : IEventEntity
    {
        private readonly DatabaseApplicationContext _databaseContext;
        private readonly MessageBroker _messageBroker;
        private readonly ILogger<IEventSynchronizer<T, TEvent>> _logger;
        private readonly ServerDescriptor _serverDescriptor;
        protected virtual IQueryable<T> DbSet => _databaseContext.Set<T>();

        public EfSynchronizedRepository(DatabaseApplicationContext databaseContext, MessageBroker messageBroker,
            ILogger<IEventSynchronizer<T, TEvent>> logger, ServerDescriptor serverDescriptor)
        {
            _databaseContext = databaseContext;
            _messageBroker = messageBroker;
            _logger = logger;
            _serverDescriptor = serverDescriptor;
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
            try
            {
                _databaseContext.Add(entity);
                _databaseContext.SaveChanges();
                if (createEvent)
                {
                    _logger.LogDebug("Publish insert event for " + entity.Id);
                    _messageBroker.Publish(InsertQueue, new EntityInsertEvent<TEvent>()
                    {
                        EmmitedServerId = _serverDescriptor.Id,
                        Entity = CreateEventModel(entity)
                    });
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while inserting entity.", ex);
                throw;
            }
        }

        public T Update(T entity, bool createEvent = true)
        {
            try
            {
                _databaseContext.Update(entity);
                _databaseContext.SaveChanges();

                if (createEvent)
                {
                    _logger.LogDebug("Publish update event for " + entity.Id);
                    _messageBroker.Publish(UpdateQueue, new EntityUpdatedEvent<TEvent>()
                    {
                        EmmitedServerId = _serverDescriptor.Id,
                        Entity = CreateEventModel(entity)
                    });
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while updating entity.", ex);
                throw;
            }
        }

        public void Delete(T entity, bool createEvent = true)
        {
            try
            {
                _databaseContext.Remove(entity);
                _databaseContext.SaveChanges();

                if (createEvent)
                {
                    _logger.LogDebug("Publish delete event for " + entity.Id);
                    _messageBroker.Publish(DeleteQueue, new EntityDeletedEvent
                    {
                        EmmitedServerId = _serverDescriptor.Id,
                        Id = entity.Id
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while removing entity.", ex);
                throw;
            }
        }

        public void OnInsertEvent(EntityInsertEvent<TEvent> @event)
        {
            _logger.LogDebug("Received insert event for " + @event.Entity.Id);
            if (@event.EmmitedServerId == _serverDescriptor.Id)
            {
                _logger.LogDebug("Skipping because emitter server id matches current");
            }
            else
            {
                var entity = CreateEntity();
                UpdateEntity(@event.Entity, entity, true);
                Insert(entity, false);
            }
        }

        public void OnUpdateEvent(EntityUpdatedEvent<TEvent> @event)
        {
            _logger.LogDebug("Received update event for " + @event.Entity.Id);
            if (@event.EmmitedServerId == _serverDescriptor.Id)
            {
                _logger.LogDebug("Skipping because emitter server id matches current");
            }
            else
            {
                var eventEntity = @event.Entity;
                var entity = Get(eventEntity.Id);
                UpdateEntity(eventEntity, entity, false);
                Update(entity, false);
            }
        }

        public void OnDeleteEvent(EntityDeletedEvent @event)
        {
            _logger.LogDebug("Received delete event for " + @event.Id);
            if (@event.EmmitedServerId == _serverDescriptor.Id)
            {
                _logger.LogDebug("Skipping because emitter server id matches current");
            }
            else
            {
                var entity = Get(@event.Id);
                Delete(entity, false);
            }
        }

        public abstract TEvent CreateEventModel(T entity);

        public abstract void UpdateEntity(TEvent @event, T entity, bool copyId);

        public abstract T CreateEntity();
    }
}