using System;

namespace WebApplication.Data.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}