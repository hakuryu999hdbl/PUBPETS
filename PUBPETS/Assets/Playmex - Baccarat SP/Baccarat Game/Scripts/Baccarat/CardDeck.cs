using System.Collections.Generic;
namespace Baccarat_Game
{
    [System.Serializable]
    public class CardDeck
    {
        public List<CardData> deck;

        public CardDeck(int deckCount) { Shuffle(deckCount); }

        public void Shuffle(int deckCount)
        {
            deck = new List<CardData>(52 * deckCount);

            do
            {
                // |vvv|
                AddDeck();
                // |^^^|
                deckCount--;
            } while (deckCount > 0);

            RandomiseDeck(); // << Randomize Deck

        }

        public CardData GetCard()
        {
            CardData card = deck[0];
            deck.RemoveAt(0);

            return card;
        }

        private void AddDeck()
        {
            var suits = (CardData.Suit[])System.Enum.GetValues(typeof(CardData.Suit));
            var ranks = (CardData.Rank[])System.Enum.GetValues(typeof(CardData.Rank));

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    deck.Add(new CardData((CardData.Suit)suits[i], (CardData.Rank)ranks[j]));
                }
            }

        }

        private void RandomiseDeck()
        {
            FisherYatesCardDeckShuffle(deck);
        }

        private static List<CardData> FisherYatesCardDeckShuffle(List<CardData> aList)
        {

            System.Random _random = new System.Random();

            CardData myGO;

            int n = aList.Count;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                myGO = aList[r];
                aList[r] = aList[i];
                aList[i] = myGO;
            }

            return aList;
        }
    }
}
