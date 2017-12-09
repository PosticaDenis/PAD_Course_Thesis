using WebApplication.Data.Entities;
using WebApplication.Data.Repository;

namespace WebApplication.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _repository;

        public MovieService(IRepository<Movie> repository)
        {
            _repository = repository;
        }

        public Movie Get(int id)
        {
            return _repository.Get(id);
        }
    }
}