using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Entities;

namespace WebApplication.Data.Repository
{
    public class EfRepository<T> : IRepository<T> where T : class,IEntity
    {
        private readonly DatabaseApplicationContext _databaseContext;
        protected virtual IQueryable<T> DbSet => _databaseContext.Set<T>();

        public EfRepository(DatabaseApplicationContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public T Get(int id)
        {
            return DbSet.First(entity => entity.Id == id);
        }

        public IEnumerable<T> Get()
        {
            return DbSet.ToList();
        }

        public T Insert(T entity)
        {
            _databaseContext.Add(entity);
            _databaseContext.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            _databaseContext.Update(entity);
            _databaseContext.SaveChanges();

            return entity;
        }

        public void Delete(T entity)
        {
            _databaseContext.Remove(entity);
            _databaseContext.SaveChanges();
        }
    }
}