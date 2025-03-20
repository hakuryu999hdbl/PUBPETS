using UnityEngine;
namespace Blackjack_Game
{
    public class Dealer : MonoBehaviour
    {
        public int Score { get { return hand.currentScore; } private set { } }

        public Hand hand;

        private void Start()
        {
            GameManager.SetDealer(this);
            if (hand == null)
                hand = GetComponentInChildren<Hand>();
        }

        public bool MayHave21()
        {
            return hand.MightHave21();
        }

        public bool HasBlackjack()
        {
            return hand.Has_Blackjack();
        }

        public void RevealCard()
        {
            hand.GetSecondCard().Reveal(() => { hand.CalculateScore(); hand.UpdateScoreView(); });
        }

        public void ConcealCard()
        {
            hand.GetSecondCard().Reveal(() => { Debug.Log(""); });

            Invoke("Conceal", 1f);

        }//手动修改，将卡翻回背面

        void Conceal() 
        {
            hand.GetSecondCard().Conceal(() => { Debug.Log(""); });
        }

        public GameObject DealCard(CardData card, FlipType flip)
        {
            return hand.DealCard(card, flip);
        }

        public bool HasTurnDownCard()
        {
            return hand.GetSecondCard().side == FlipType.FlipDown;
        }

        public bool IsEnded(bool playerBusted)
        {
            return hand.currentScore >= 17 || playerBusted;
        }

        public bool IsBusted()
        {
            return hand.IsBust();
        }

        public void ResetTable()
        {
            hand.ResetHand();
        }

    }
}
