using System.Linq;
using WebApplication.Data.Entities;
using Movie = WebApplication.Presentation.Models.Movie;

namespace WebApplication.Presentation.Mapper
{
    public static class MovieMapper
    {
        public static Movie Map(Data.Entities.Movie source, Movie destination)
        {
            destination.Rating = source.Rating;
            destination.ReleasedYear = source.ReleasedYear;
            destination.Sales = source.Sales;
            destination.Title = source.Title;
            destination.Id = source.Id;

            return destination;
        }

        public static Movie Map(Data.Entities.Movie source)
        {
            return Map(source, new Movie());
        }

        public static Data.Entities.Movie Map(Movie source, Data.Entities.Movie destination)
        {
            destination.Rating = source.Rating;
            destination.ReleasedYear = source.ReleasedYear;
            destination.Sales = source.Sales;
            destination.Title = source.Title;

            return destination;
        }

        public static Data.Entities.Movie Map(Movie source)
        {
            return Map(source, new Data.Entities.Movie());
        }
    }
}