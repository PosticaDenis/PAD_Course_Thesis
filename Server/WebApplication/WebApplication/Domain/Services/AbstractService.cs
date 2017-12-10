using System;
using System.Collections.Generic;
using WebApplication.Data.Entities;
using WebApplication.Data.Repository;

namespace WebApplication.Domain.Services
{
    public class AbstractService<T> : IService<T> where T : class, IEntity
    {
        protected readonly IRepository<T> MovieRepository;

        public AbstractService(IRepository<T> movieRepository)
        {
            MovieRepository = movieRepository;
        }

        public T Get(Guid id)
        {
            return MovieRepository.Get(id);
        }

        public T Insert(T entity)
        {
            return MovieRepository.Insert(entity);
        }

        public IEnumerable<T> Get()
        {
            return MovieRepository.Get();
        }

        public T Update(T entity)
        {
            return MovieRepository.Update(entity);
        }

        public void Delete(Guid id)
        {
            var entity = MovieRepository.Get(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            MovieRepository.Delete(entity);
        }
    }
}