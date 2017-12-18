using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebApplication.Data.Entities;

namespace WebApplication.Data.Repository
{
    public abstract class EfRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly DatabaseApplicationContext _databaseContext;
        private readonly ILogger<IRepository<T>> _logger;
        protected virtual IQueryable<T> DbSet => _databaseContext.Set<T>();

        public EfRepository(DatabaseApplicationContext databaseContext, ILogger<IRepository<T>> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public T Get(Guid id)
        {
            return DbSet.First(entity => entity.Id == id);
        }

        public IEnumerable<T> Get()
        {
            return DbSet.ToList();
        }

        public T Insert(T entity)
        {
            try
            {
                _databaseContext.Add(entity);
                _databaseContext.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while inserting entity.", ex);
                throw;
            }
        }

        public T Update(T entity)
        {
            try
            {
                _databaseContext.Update(entity);
                _databaseContext.SaveChanges();


                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while updating entity.", ex);
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _databaseContext.Remove(entity);
                _databaseContext.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error while removing entity.", ex);
                throw;
            }
        }
    }
}