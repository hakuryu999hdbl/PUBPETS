using UnityEngine;
using DG.Tweening;
namespace Baccarat_Game
{
    public class PlayerHand : Hand
    {
        public bool stand = false;

        override public void ResetHand()
        {
            base.ResetHand();

            stand = false;
        }

        protected override void UpdateNextPosition()
        {
            float offset = (tableCards.Count == 1) ? 0.10f : 0;
            nextTransformPosition = nextTransformPosition - new Vector3(cardXShift + offset, -0.0001f, offset);
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
            return tableCards.Count >= 3 || stand;
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
                (tableCards[0].GetComponentInChildren<Card>().cardData.GetValue() + tableCards[1].GetComponentInChildren<Card>().cardData.GetValue()) == 8;
        }

        public bool IsNatural9()
        {
            return
                tableCards.Count == 2 &&
                (tableCards[0].GetComponentInChildren<Card>().cardData.GetValue() + tableCards[1].GetComponentInChildren<Card>().cardData.GetValue()) == 9;
        }

        public int TakeValueFromCard(int index)
        {
            return tableCards[index].GetComponentInChildren<Card>().cardData.GetValue();

        }

        public bool HasAnAce()
        {
            return (tableCards[0].GetComponentInChildren<Card>().cardData.rank == CardData.Rank.Ace || tableCards[1].GetComponentInChildren<Card>().cardData.rank == CardData.Rank.Ace);
        }
    }
}