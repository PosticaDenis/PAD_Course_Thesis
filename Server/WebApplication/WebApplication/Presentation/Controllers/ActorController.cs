using System;
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
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;
        private readonly ILinksService _linksService;

        public ActorController(IActorService actorService, ILinksService linksService)
        {
            _actorService = actorService;
            _linksService = linksService;
        }

        [HttpGet(Name = "GetActor")]
        public async Task<IEnumerable<Actor>> Get()
        {
            return await _actorService.Get()
                .Select(entity => ActorMapper.Map(entity))
                .AddLinks(_linksService);
        }

        [HttpGet("{id}", Name = "GetActorById")]
        public async Task<Actor> Get(Guid id)
        {
            return await ActorMapper.Map(_actorService.Get(id))
                .AddLinks(_linksService);
        }

        [HttpPost(Name = "PostActor")]
        public async Task<Actor> Post([FromBody] Actor actor)
        {
            var entity = ActorMapper.Map(actor);
            entity = _actorService.Insert(entity);


            return await ActorMapper.Map(entity)
                .AddLinks(_linksService);
        }

        [HttpPut("{id}", Name = "PutActor")]
        public async Task<Actor> Put(Guid id, [FromBody] Actor actor)
        {
            var entity = _actorService.Get(id);

            ActorMapper.Map(actor, entity);
            entity = _actorService.Update(entity);

            return await ActorMapper.Map(entity)
                .AddLinks(_linksService);
        }

        [HttpDelete("{id}", Name = "DeleteActor")]
        public async Task Delete(Guid id)
        {
            _actorService.Delete(id);
        }
    }
}