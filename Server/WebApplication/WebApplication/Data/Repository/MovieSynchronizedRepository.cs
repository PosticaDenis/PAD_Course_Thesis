using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Data.Entities;
using WebApplication.Data.Events;
using WebApplication.Presentation.Models;
using Movie = WebApplication.Data.Entities.Movie;

namespace WebApplication.Data.Repository
{
    public class MovieSynchronizedRepository : EfSynchronizedRepository<Movie, MovieEventEntity>, IMovieRepository
    {
        public MovieSynchronizedRepository(DatabaseApplicationContext databaseContext,
            MessageBus.MessageBroker messageBroker, ILogger<IEventSynchronizer<Movie, MovieEventEntity>> logger,
            ServerDescriptor descriptor)
            : base(databaseContext, messageBroker, logger, descriptor)
        {
        }

        protected override IQueryable<Movie> DbSet => base.DbSet.Include(p => p.Actors);

        public override MovieEventEntity CreateEventModel(Movie entity)
        {
            return new MovieEventEntity()
            {
                Id = entity.Id,
                Actors = entity.Actors?.Select(ma => ma.ActorId).ToArray(),
                Rating = entity.Rating,
                ReleasedYear = entity.ReleasedYear,
                Sales = entity.Sales,
                Title = entity.Title
            };
        }

        public override void UpdateEntity(MovieEventEntity @event, Movie entity, bool copyId)
        {
            if (copyId)
            {
                entity.Id = @event.Id;
            }
            entity.Title = @event.Title;
            entity.Rating = @event.Rating;
            entity.ReleasedYear = @event.ReleasedYear;
            entity.Sales = @event.Sales;
            entity.Actors = @event.Actors?.Select(a => new ActorMovie
            {
                ActorId = a
            }).ToList();
        }

        public override Movie CreateEntity()
        {
            return new Movie();
        }
    }
}