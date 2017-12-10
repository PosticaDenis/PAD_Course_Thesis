using System;

namespace WebApplication.Data.Events
{
    public interface IEventEntity
    {
        Guid Id { get; set; }
    }
}