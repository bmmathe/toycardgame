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
    public class CardController
    {
        private IRedisDataAccess _redis;
        public CardController(IRedisDataAccess redis)
        {
            _redis = redis;
        }
        [HttpGet]
        public List<Card> Get()
        {
            return _redis.GetCards();
        }
    }
}
