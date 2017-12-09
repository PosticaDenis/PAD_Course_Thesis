using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiskFirst.Hateoas;
using WebApplication.Domain.Services;
using WebApplication.Presentation.Helper;
using WebApplication.Presentation.Mapper;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ILinksService _linksService;

        public MovieController(IMovieService movieService, ILinksService linksService)
        {
            _movieService = movieService;
            _linksService = linksService;
        }

        [HttpGet(Name = "GetMovie")]
        public async Task<IEnumerable<Movie>> Get()
        {
            return await _movieService.Get()
                .Select(entity => MovieMapper.Map(entity))
                .AddLinks(_linksService);
        }

        [HttpGet("{id}", Name = "GetMovieById")]
        public async Task<Movie> Get(int id)
        {
            return await MovieMapper.Map(_movieService.Get(id))
                .AddLinks(_linksService);
        }

        [HttpPost(Name = "PostMovie")]
        public async Task<Movie> Post([FromBody] Movie movie)
        {
            var entity = MovieMapper.Map(movie);
            entity = _movieService.Insert(entity);


            return await MovieMapper.Map(entity)
                .AddLinks(_linksService);
        }

        [HttpPut("{id}", Name = "PutMovie")]
        public async Task<Movie> Put(int id, [FromBody] Movie movie)
        {
            var entity = _movieService.Get(id);

            MovieMapper.Map(movie, entity);
            entity = _movieService.Update(entity);

            return await MovieMapper.Map(entity)
                .AddLinks(_linksService);
        }

        [HttpDelete("{id}", Name = "DeleteMovie")]
        public async Task Delete(int id)
        {
            _movieService.Delete(id);
        }
    }
}