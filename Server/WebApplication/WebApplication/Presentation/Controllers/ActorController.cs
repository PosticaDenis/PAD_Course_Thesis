using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Domain.Services;
using WebApplication.Presentation.Mapper;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public IEnumerable<Actor> Get()
        {
            return _actorService.Get()
                .Select(entity => ActorMapper.Map(entity));
        }

        [HttpGet("{id}")]
        public Actor Get(int id)
        {
            return ActorMapper.Map(_actorService.Get(id));
        }

        [HttpPost]
        public Actor Post([FromBody] Actor actor)
        {
            var entity = ActorMapper.Map(actor);
            entity = _actorService.Insert(entity);


            return ActorMapper.Map(entity);
        }

        [HttpPut("{id}")]
        public Actor Put(int id, [FromBody] Actor actor)
        {
            var entity = _actorService.Get(id);

            ActorMapper.Map(actor, entity);
            entity = _actorService.Update(entity);

            return ActorMapper.Map(entity);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _actorService.Delete(id);
        }
    }
}