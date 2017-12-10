namespace WebApplication.Data.Events
{
    public class EntityInsertEvent<T>
    {
        public T Entity { get; set; }
    }
}