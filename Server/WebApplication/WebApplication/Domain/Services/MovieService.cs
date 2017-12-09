using System;
using System.Collections.Generic;
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

        public Movie Insert(Movie movie)
        {
            return _repository.Insert(movie);
        }

        public IEnumerable<Movie> Get()
        {
            return _repository.Get();
        }

        public Movie Update(Movie movie)
        {
            return _repository.Update(movie);
        }

        public void Delete(int id)
        {
            var movie = _repository.Get(id);

            if (movie == null)
            {
                throw new Exception("Entity not found");
            }
            
            _repository.Delete(movie);
        }
    }
}