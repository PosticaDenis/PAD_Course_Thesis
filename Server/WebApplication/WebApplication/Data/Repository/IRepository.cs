using System;
using System.Collections.Generic;
using WebApplication.Data.Entities;
using WebApplication.Data.Events;

namespace WebApplication.Data.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        T Get(Guid id);
        IEnumerable<T> Get();
        T Insert(T entity, bool createEvent = true);
        T Update(T entity, bool createEvent = true);
        void Delete(T entity, bool createEvent = true);


    }
}