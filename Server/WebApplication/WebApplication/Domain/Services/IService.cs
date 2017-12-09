using System.Collections.Generic;
using WebApplication.Data.Entities;

namespace WebApplication.Domain.Services
{
    public interface IService<T> where T : class,IEntity
    {
        T Get(int id);
        T Insert(T entity);
        IEnumerable<T> Get();
        T Update(T entity);
        void Delete(int id);
    }
}