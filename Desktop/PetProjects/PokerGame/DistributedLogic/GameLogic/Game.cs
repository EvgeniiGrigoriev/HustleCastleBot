using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerGame.GameLogic
{
    public class Game
    {
        private List<Player> Players { get; set; }

        private Deck MainDeck { get; set; }

        private Deck OpenDeck { get; set; }

        public Game()
        {
            MainDeck = new Deck();
            Players = new List<Player>();
            OpenDeck = new Deck();
        }

        public void AddPlayer(Player player)
        {
            if(Players.Count < 9)
            {
                Players.Add(player);
            }
        }

        public void ShuffleDeck()
        {
            MainDeck.Shuffle();
        }

        public void DealCards()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var player in Players)
                {
                    player.AddCard(MainDeck.PopCard());
                }
            }

            AddShowedCards();
        }

        public void AddShowedCards()
        {
            MainDeck.PopCard();

            if(OpenDeck.Length < 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    OpenDeck.AddCard(MainDeck.PopCard());
                }
            }
            else
            {
                OpenDeck.AddCard(MainDeck.PopCard());
            }
        }

        public void PrintAllCards()
        {
            var orderedByCardName = OpenDeck.OrderByCardNameDescending();

            for (int i = 0; i < orderedByCardName.Count; i++)
            {
                var card = orderedByCardName[i];
                Console.WriteLine("Card: {0} - {1}", card.Suit, card.CardName);
            }

            Players.ForEach(x => x.Combination = x.Deck.CheckCombination(OpenDeck));

            Players = Players.OrderByDescending(player => player.Combination).ToList();

            foreach (var player in Players)
            {
                player.PrintCards(OpenDeck);
            }
        }
    }
}
