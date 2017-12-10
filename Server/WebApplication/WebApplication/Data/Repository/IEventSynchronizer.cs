using WebApplication.Data.Entities;
using WebApplication.Data.Events;

namespace WebApplication.Data.Repository
{
    public interface IEventSynchronizer<T, TEvent> where T : IEntity where TEvent : IEventEntity
    {
        string InsertQueue { get; }
        string UpdateQueue { get; }
        string DeleteQueue { get; }
        void OnInsertEvent(EntityInsertEvent<TEvent> @event);
        void OnUpdateEvent(EntityUpdatedEvent<TEvent> @event);
        void OnDeleteEvent(EntityDeletedEvent @event);

        TEvent CreateEventModel(T entity);
        void UpdateEntity(TEvent @event, T entity, bool copyId);
        T CreateEntity();
    }
}