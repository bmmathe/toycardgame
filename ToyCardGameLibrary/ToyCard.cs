namespace ToyCardGameLibrary
{
    public class ToyCard : Card
    {
        public ToyCard()
        {
            CardType = CardType.Toy;            
        }
        public int Attack { get; set; }
        public int Health { get; set; }
        public StaticCardProperty[] Properties { get; set; }
    }
}
