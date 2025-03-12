using UnityEngine;

namespace Baccarat_Game
{
    public class BankerHand : Hand
    {
        public bool stand = false;

        override public void ResetHand()
        {
            base.ResetHand();

            stand = false;
        }

        public void Stand()
        {
            stand = true;
        }

        public bool CanHit()
        {
            return currentScore > 0 && !IsEnded();
        }

        public bool IsEnded()
        {
            return currentScore > 20 || stand;
        }

        public bool HasPush(int anotherScore)
        {
            return IsEnded() && currentScore == anotherScore;
        }

        public bool HasLost(int dealerScore, bool dealerBlackjack)
        {
            return (dealerScore > currentScore && dealerScore < 22);
        }

        public bool IsNatural8()
        {
            return
                tableCards.Count == 2 &&
                (tableCards[0].GetComponent<Card>().cardData.GetValue() + tableCards[1].GetComponent<Card>().cardData.GetValue()) == 8;
        }

        public bool IsNatural9()
        {
            return
                tableCards.Count == 2 &&
                (tableCards[0].GetComponent<Card>().cardData.GetValue() + tableCards[1].GetComponent<Card>().cardData.GetValue()) == 9;
        }

        public bool HasAnAce()
        {
            return (tableCards[0].GetComponent<Card>().cardData.rank == CardData.Rank.Ace || tableCards[1].GetComponent<Card>().cardData.rank == CardData.Rank.Ace);
        }
    }
}
