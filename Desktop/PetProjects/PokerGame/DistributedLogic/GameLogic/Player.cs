using System;

namespace PokerGame.GameLogic
{
    public class Player
    {
        public string Name { get; set; }

        public Deck Deck { get; set; }

        public Combination Combination { get; set; }

        public Player(string name = "", Deck deck = null)
        {
            Name = name;            
            Deck = deck ?? new Deck();
            Combination = new Combination();
        }

        public void AddCard(Card card)
        {
            Deck.AddCard(card);
        }        

        public void PrintCards(Deck openDeck)
        {
            Combination = Deck.CheckCombination(openDeck);
            Console.WriteLine("");
            Console.WriteLine("Player name = " + Name);
            Deck.PrintCards();
            Console.WriteLine("Player combination: {0} Combination value: {1}", Combination.CombinationName, Combination.CombinationValue);
        }
    }
}
