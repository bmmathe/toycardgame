using System;
using System.Collections.Generic;
using StackExchange.Redis;
using ToyCardGameLibrary;

namespace ToyCardGame.Redis
{
    public class RedisDataAccess : IRedisDataAccess
    {
        private ConnectionMultiplexer Redis { get;  }= ConnectionMultiplexer.Connect("localhost");
        public Card GetCard(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetCards()
        {
            throw new NotImplementedException();
        }
    }
}