using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Domain.Services;
using WebApplication.Presentation.Mapper;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return _movieService.Get()
                .Select(entity => MovieMapper.Map(entity));
        }

        [HttpGet("{id}")]
        public Movie Get(int id)
        {
            return MovieMapper.Map(_movieService.Get(id));
        }

        [HttpPost]
        public Movie Post([FromBody] Movie movie)
        {
            var entity = MovieMapper.Map(movie);
            entity = _movieService.Insert(entity);


            return MovieMapper.Map(entity);
        }

        [HttpPut("{id}")]
        public Movie Put(int id, [FromBody] Movie movie)
        {
            var entity = _movieService.Get(id);

            MovieMapper.Map(movie, entity);
            entity = _movieService.Update(entity);

            return MovieMapper.Map(entity);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _movieService.Delete(id);
        }
    }
}