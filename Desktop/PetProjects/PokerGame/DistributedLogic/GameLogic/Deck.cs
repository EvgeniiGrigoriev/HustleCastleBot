using PokerGame.GameLogic.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerGame.GameLogic
{
    public class Deck
    {
        private List<Card> Cards = new List<Card>();

        public int Length
        {
            get
            {
                return Cards.Count;
            }
        }

        public Deck()
        {

        }

        public void Create()
        {            
            foreach (var cardName in Enum.GetValues(typeof(CardName)))
            {
                foreach (var suit in Enum.GetValues(typeof(Suit)))
                {
                    var card = new Card((Suit)suit, (CardName)cardName);
                    Cards.Add(card);
                }
            }
        }

        public void Shuffle()
        {
            if(Cards.Count == 0)
            {
                Create();
            }

            List<Card> shuffledCards = new List<Card>();

            Random r = new Random();

            while (Cards.Count > 0)
            {
                var card = Cards[r.Next(0, Cards.Count)];
                shuffledCards.Add(card);
                Cards.Remove(card);
            }

            Cards = shuffledCards;
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public Card PopCard()
        {
            if(Cards.Count != 0)
            {
                Card card = Cards[0];

                Cards.Remove(card);

                return card;
            }
            else
            {
                return new Card();
            }
        }

        public Combination CheckCombination(Deck openDeck)
        {
            var combination = new Combination();

            List<Card> allCards = new List<Card>(openDeck.Cards);

            allCards.AddRange(Cards);

            combination.AddCardsRange(allCards);

            combination.CheckCombination();

            return combination;
        }

        public List<Card> OrderByCardNameDescending()
        {
            return Cards.OrderByDescending(x => x.CardName).ToList();
        }

        public void PrintCards()
        {
            foreach (var card in Cards)
            {
                Console.WriteLine(string.Format("{0} - {1}", card.CardName, card.Suit));
            }
        }
    }
}
