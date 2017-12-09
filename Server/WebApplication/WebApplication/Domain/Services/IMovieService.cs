using System.Collections.Generic;
using WebApplication.Data.Entities;

namespace WebApplication.Domain.Services
{
    public interface IMovieService
    {
        Movie Get(int id);
        Movie Insert(Movie movie);
        IEnumerable<Movie> Get();
        Movie Update(Movie movie);
        void Delete(int id);
    }
}