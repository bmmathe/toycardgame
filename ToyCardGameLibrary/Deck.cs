using System.Collections.Generic;

namespace ToyCardGameLibrary
{
    public class Deck
    {
        public Deck(string name)
        {
            Name = name;
            Cards = new List<Card>(30);
        }
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
    }
}
