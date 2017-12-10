namespace WebApplication.Data.Events
{
    public class EntityUpdatedEvent<T>
    {
        public T Entity { get; set; }
    }
}