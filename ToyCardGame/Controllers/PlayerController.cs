using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToyCardGame.Redis;
using ToyCardGameLibrary;

namespace ToyCardGame.Controllers
{
    [Route("[controller]")]
    public class PlayerController : Controller
    {
        private IRedisDataAccess _redis;

        public PlayerController(IRedisDataAccess redis)
        {
            _redis = redis;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public Card Get(Guid id)
        {
            return _redis.GetCard(id);
        }

        [HttpGet]
        public List<Card> Get()
        {
            return _redis.GetCards();
        }
    }
}
