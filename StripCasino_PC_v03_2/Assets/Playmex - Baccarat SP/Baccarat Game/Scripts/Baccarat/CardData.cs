using UnityEngine;

namespace Baccarat_Game
{
    [System.Serializable]
    public class CardData
    {

        public enum Suit { Hearts, Diamonds, Spades, Clubs }
        public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

        public Suit suit;
        public Rank rank;

        public CardData(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;

        }

        public string DebugName()
        {
            return rank.ToString() + " of " + suit.ToString() + ", " + GetValue();
        }

        public int GetValue()
        {
            if ((int)rank < 9)
                return (int)rank + 1;

            return 0;
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
