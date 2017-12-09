using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Entities;

namespace WebApplication.Data.Repository
{
    public class ActorRepository : EfRepository<Actor>, IRepository<Actor>
    {
        public ActorRepository(DatabaseApplicationContext databaseContext) : base(databaseContext)
        {
        }

        protected override IQueryable<Actor> DbSet => base.DbSet.Include(p => p.Movies);
    }
}