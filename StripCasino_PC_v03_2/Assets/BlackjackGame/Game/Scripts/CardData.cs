using UnityEngine;
namespace Blackjack_Game
{
    [System.Serializable]
    public class CardData
    {
        // Local
        public enum Suit
        {
            Hearts,
            Diamonds,
            Spades,
            Clubs
        }

        public enum Rank
        {
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        }

        public Suit suit;
        public Rank rank;

        public CardData(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        public CardData(int suit, int rank)
        {
            this.suit = (Suit)suit;
            this.rank = (Rank)rank;
        }

        public string GetName()
        {
            return $"{rank} of {suit}";
        }

        public int GetValue()
        {
            if ((int)rank < 10)
                return (int)rank + 1;

            return 10;
        }

        public string GetAssetName()
        {
            return rank.ToString().ToLower() + suit.ToString().ToLower();
        }

        public Mesh GetMesh()
        {
            string meshName = GetAssetName();
            return Resources.Load<Mesh>(("Cards/" + meshName));
        }
    }
}