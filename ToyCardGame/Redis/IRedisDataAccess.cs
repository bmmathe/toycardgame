using System;
using System.Collections.Generic;
using ToyCardGameLibrary;

namespace ToyCardGame.Redis
{
    public interface IRedisDataAccess
    {
        Card GetCard(Guid id);
        List<Card> GetCards();
    }
}
