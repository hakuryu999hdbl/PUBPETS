using System.Collections.Generic;
namespace Blackjack_Game
{
    [System.Serializable]
    public class CardDeck
    {
        private List<CardData> cards;
        private int _deckCount;
        public CardDeck(int deckCount)
        {
            _deckCount = deckCount;
            Shuffle(_deckCount);
        }

        public void Shuffle(int deckCount)
        {
            cards = new List<CardData>(52 * deckCount);

            do
            {
                AddDeck();
                // DebugDeck();

                deckCount--;
            } while (deckCount > 0);

            RandomiseDeck(); // << Randomize Deck
        }

        public CardData GetCard()
        {
            CardData card = cards[0];
            cards.RemoveAt(0);

            if (cards.Count == 0)
            {
                Shuffle(_deckCount);
            }

            return card;
        }

        private void AddDeck()
        {
            var suits = (CardData.Suit[])System.Enum.GetValues(typeof(CardData.Suit));
            var ranks = (CardData.Rank[])System.Enum.GetValues(typeof(CardData.Rank));

            foreach (var s in suits)
            {
                foreach (var r in ranks)
                {
                    var card = new CardData(s, r);
                    cards.Add(card);
                }
            }
        }

        private void DebugDeck()
        {
            cards.Add(new CardData(0, 4));
            cards.Add(new CardData(0, 0));
            cards.Add(new CardData(0, 3));
            cards.Add(new CardData(0, 10));

            cards.Add(new CardData(0, 10));
            cards.Add(new CardData(0, 10));
            cards.Add(new CardData(0, 10));
            cards.Add(new CardData(0, 4));
        }

        private void RandomiseDeck()
        {
            FisherYatesCardDeckShuffle(cards);
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