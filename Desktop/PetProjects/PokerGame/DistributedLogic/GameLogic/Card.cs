using PokerGame.GameLogic.Constants;

namespace PokerGame.GameLogic
{
    public class Card
    {
        public Suit Suit { get; set; }

        public CardName CardName { get; set; }

        public Card()
        {

        }

        public Card(Suit suit, CardName cardName)
        {
            Suit = suit;
            CardName = cardName;
        }
    }
}
