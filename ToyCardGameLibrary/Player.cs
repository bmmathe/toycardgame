using System;
using System.Collections.Generic;

namespace ToyCardGameLibrary
{
    public class Player
    {
        public Guid PlayerId { get; set; }
        public string Username { get; set; }

        private List<Card> _library;
        public List<Card> Library
        {
            get { return _library ?? (_library = new List<Card>()); }
            set { _library = value; }
        }

        private List<Deck> _decks;
        public List<Deck> Decks
        {
            get { return _decks ?? (_decks = new List<Deck>()); }
            set { _decks = value; }
        }
    }
}
