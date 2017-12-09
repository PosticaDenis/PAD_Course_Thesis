using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Domain.Services;
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
            return new Movie[]
            {
                new Movie
                {
                    Rating = new decimal(4.5),
                    ReleasedYear = 2010,
                    Sales = new decimal(40000),
                    Title = "Clone"
                }
            };
        }
        
        [HttpGet("{id}")]
        public Movie Get(int id)
        {
            return Mapper.Map<Movie>(_movieService.Get(id));
        }
        
        [HttpPost]
        public Movie Post([FromBody]Movie movie)
        {
            // todo 
            return new Movie
            {
                Rating = new decimal(4.5),
                ReleasedYear = 2010,
                Sales = new decimal(40000),
                Title = "Clone"
            };
        }
        
        [HttpPut("{id}")]
        public Movie Put(int id, [FromBody]Movie movie)
        {
            // todo 
            return new Movie
            {
                Rating = new decimal(4.5),
                ReleasedYear = 2010,
                Sales = new decimal(40000),
                Title = "Clone"
            };
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // todo
        }
        
        
    }
}