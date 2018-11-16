using PokerGame.GameLogic.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerGame.GameLogic
{
    public class Combination : IComparable
    {
        private List<Card> Cards { get; set; }

        public CombinationName CombinationName { get; set; }

        public Combination()
        {
            Cards = new List<Card>();
        }

        public int CombinationValue => Cards.Sum(x => (int)x.CardName);

        public void CheckCombination()
        {
            List<Card> cardsTmp = new List<Card>(Cards);

            var cardsBySuit = cardsTmp.GroupBy(x => x.Suit).ToList();

            var fiveCardsBySuitAndCardValue = cardsBySuit.Where(arr => arr.Count() == 5)
                                                         .FirstOrDefault()
                                                         ?.OrderBy(x => x.CardName)
                                                         .ToList();

            
            if(fiveCardsBySuitAndCardValue != null)
            {
                bool isRoyalFlush = true;

                bool isStraightFlush = true;

                bool isContainsKingAndAce = fiveCardsBySuitAndCardValue.Count(x => x.CardName == CardName.Ace || x.CardName == CardName.King) == 2;

                //RoyalFlush
                for (int i = 0; i < fiveCardsBySuitAndCardValue.Count - 1; i++)
                {
                    var card1 = fiveCardsBySuitAndCardValue[i];
                    var card2 = fiveCardsBySuitAndCardValue[i + 1];
                    if (Math.Abs(card1.CardName - card2.CardName) > 1)
                    {                        
                        isRoyalFlush = false;
                        if(!(card1.CardName == CardName.Ace && card2.CardName == CardName.Five)
                            && !(card1.CardName == CardName.Five && card2.CardName == CardName.Ace))
                        {
                            isStraightFlush = false;
                            break;
                        }                        
                    }
                }

                if (isRoyalFlush && isContainsKingAndAce)
                {
                    CombinationName = CombinationName.RoyalFlush;
                    Cards = fiveCardsBySuitAndCardValue;

                    return;
                }

                //StraightFlush
                if (isStraightFlush)
                {
                    CombinationName = CombinationName.StraightFlush;
                    Cards = fiveCardsBySuitAndCardValue;

                    return;
                }
            }

            var cardsFourOfKind = cardsTmp.GroupBy(x => x.CardName)
                                          .Where(card => card.Count() == 4)
                                          .FirstOrDefault()
                                          ?.ToList();

            if(cardsFourOfKind != null)
            {
                CombinationName = CombinationName.FourOfKind;
                Cards = cardsFourOfKind;
                Cards.Add(cardsTmp.Except(cardsFourOfKind).OrderByDescending(x => x.CardName).First());

                return;
            }

            var cardsFullHouse = cardsTmp.GroupBy(x => x.CardName)
                                         .Where(card => card.Count() >= 2)
                                         .ToList();

            var threeCards = cardsFullHouse.Where(arr => arr.Count() == 3).OrderByDescending(x => x.Key).FirstOrDefault();

            cardsFullHouse.Remove(threeCards);

            var smallerPairOrThreeCards = cardsFullHouse.Where(arr => arr.Count() >= 2).OrderByDescending(x => x.Key).FirstOrDefault();

            if(threeCards != null && smallerPairOrThreeCards != null)
            {
                List<Card> fullHouseCards = new List<Card>(threeCards);

                fullHouseCards.AddRange(smallerPairOrThreeCards.Take(2));

                CombinationName = CombinationName.FullHouse;
                Cards = fullHouseCards;

                return;
            }

            if(fiveCardsBySuitAndCardValue != null)
            {
                CombinationName = CombinationName.Flush;
                Cards = fiveCardsBySuitAndCardValue;

                return;
            }

            var fiveCardsByValue = cardsTmp.GroupBy(x => x.CardName)
                                           .Where(arr => arr.Count() == 5)
                                           .FirstOrDefault();

            if(fiveCardsByValue != null)
            {
                bool isStraight = true;

                var fiveCardsStraight = fiveCardsByValue.ToList();

                for (int i = 0; i < fiveCardsStraight.Count - 1; i++)
                {
                    var card1 = fiveCardsStraight[i];
                    var card2 = fiveCardsStraight[i + 1];
                    if (Math.Abs(card1.CardName - card2.CardName) > 1)
                    {
                        if (!(card1.CardName == CardName.Ace && card2.CardName == CardName.Five)
                               && !(card1.CardName == CardName.Five && card2.CardName == CardName.Ace))
                        {
                            isStraight = false;
                            break;
                        }
                    }
                }

                if(isStraight)
                {
                    CombinationName = CombinationName.Straight;
                    Cards = fiveCardsByValue.ToList();

                    return;
                }
            }

            if(threeCards != null)
            {
                List<Card> cardsWithHighestValue = new List<Card>();

                cardsWithHighestValue = threeCards.ToList();

                cardsWithHighestValue.AddRange(cardsTmp.Except(cardsWithHighestValue).OrderByDescending(x => x.CardName).Take(2));

                CombinationName = CombinationName.ThreeOfKind;
                Cards = cardsWithHighestValue;

                return;
            }

            var cardsByPairs = cardsFullHouse.OrderByDescending(x => x.Key).Take(2);

            if(cardsByPairs.Count() >= 2)
            {
                List<Card> cardsWithHighestValue = new List<Card>();

                cardsWithHighestValue.AddRange(cardsByPairs.ToList()[0]);
                cardsWithHighestValue.AddRange(cardsByPairs.ToList()[1]);

                cardsWithHighestValue.Add(cardsTmp.Except(cardsWithHighestValue).OrderByDescending(x => x.CardName).FirstOrDefault());

                CombinationName = CombinationName.TwoPairs;
                Cards = cardsWithHighestValue;

                return;
            }

            var cardsByPair = cardsFullHouse.OrderByDescending(x => x.Key).FirstOrDefault();

            if(cardsByPair != null)
            {
                List<Card> cardsWithHighestValue = new List<Card>();

                cardsWithHighestValue.AddRange(cardsByPair);

                cardsWithHighestValue.AddRange(cardsTmp.Except(cardsWithHighestValue).OrderByDescending(x => x.CardName).Take(3));

                CombinationName = CombinationName.Pair;
                Cards = cardsWithHighestValue;

                return;
            }

            CombinationName = CombinationName.HighCard;
            Cards = cardsTmp.OrderByDescending(x => x.CardName).Take(5).ToList();
        }

        public void AddCard(Card card)
        {
            this.Cards.Add(card);
        }

        public void AddCardsRange(List<Card> cards)
        {
            Cards.AddRange(cards);
        }

        public int CompareTo(object obj)
        {
            var anotherCombination = (Combination)obj;

            if(CombinationName > anotherCombination.CombinationName)
            {
                return 1;
            }
            else if (CombinationName < anotherCombination.CombinationName)
            {
                return -1;
            }
            else if(CombinationName == CombinationName.FullHouse && anotherCombination.CombinationName == CombinationName.FullHouse)
            {
                var thisCombinationThreeCardsSum = Cards.GroupBy(x => x.CardName).Where(x => x.Count() == 3).First().Sum(x => (int)x.CardName);
                var anotherCombinationThreeCardsSum = anotherCombination.Cards.GroupBy(x => x.CardName).Where(x => x.Count() == 3).First().Sum(x => (int)x.CardName);

                if(thisCombinationThreeCardsSum > anotherCombinationThreeCardsSum)
                {
                    return 1;
                }
                else if(thisCombinationThreeCardsSum < anotherCombinationThreeCardsSum)
                {
                    return -1;
                }

                var thisCombinationTwoCardsSum = Cards.GroupBy(x => x.CardName).Where(x => x.Count() == 2).First().Sum(x => (int)x.CardName);
                var anotherCombinationTwoCardsSum = anotherCombination.Cards.GroupBy(x => x.CardName).Where(x => x.Count() == 2).First().Sum(x => (int)x.CardName);

                if (thisCombinationTwoCardsSum > anotherCombinationTwoCardsSum)
                {
                    return 1;
                }
                else if (thisCombinationTwoCardsSum < anotherCombinationTwoCardsSum)
                {
                    return -1;
                }
            }
            else if(CombinationName == anotherCombination.CombinationName && CombinationValue == anotherCombination.CombinationValue)
            {
                return 0;
            }            
            else if(CombinationValue > anotherCombination.CombinationValue)
            {
                return 1;
            }

            return -1;
        }
    }
}
