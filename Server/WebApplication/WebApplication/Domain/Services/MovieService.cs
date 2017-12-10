using System;
using System.Collections.Generic;
using WebApplication.Data.Entities;
using WebApplication.Data.Repository;

namespace WebApplication.Domain.Services
{
    public class MovieService : AbstractService<Movie>,IMovieService
    {
        public MovieService(IMovieRepository movieRepository) : base(movieRepository)
        {
            
        }

    }
}