using System;

namespace ToyCardGameLibrary
{
    public class Card
    {
        public Guid CardId { get; set; }
        public CardType CardType { get; set; }
        public int Cost { get; set; }
        public string Name { get; set; }        
    }
}
