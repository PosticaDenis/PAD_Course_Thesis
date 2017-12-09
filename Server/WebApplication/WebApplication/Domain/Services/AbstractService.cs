using System;
using System.Collections.Generic;
using WebApplication.Data.Entities;
using WebApplication.Data.Repository;

namespace WebApplication.Domain.Services
{
    public class AbstractService<T> : IService<T> where T : class, IEntity
    {
        protected readonly IRepository<T> _repository;

        public AbstractService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public T Get(int id)
        {
            return _repository.Get(id);
        }

        public T Insert(T entity)
        {
            return _repository.Insert(entity);
        }

        public IEnumerable<T> Get()
        {
            return _repository.Get();
        }

        public T Update(T entity)
        {
            return _repository.Update(entity);
        }

        public void Delete(int id)
        {
            var entity = _repository.Get(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            _repository.Delete(entity);
        }
    }
}