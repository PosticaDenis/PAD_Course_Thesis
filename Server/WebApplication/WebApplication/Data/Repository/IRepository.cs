using System;
using System.Collections.Generic;
using WebApplication.Data.Entities;

namespace WebApplication.Data.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        T Get(Guid id);
        IEnumerable<T> Get();
        T Insert(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}