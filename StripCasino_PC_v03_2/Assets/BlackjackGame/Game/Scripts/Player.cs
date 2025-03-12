using UnityEngine;
using System.Collections.Generic;
namespace Blackjack_Game
{
    public class Player : MonoBehaviour
    {
        public UIStates uiStates;

        public int Score
        {
            get { return hand.currentScore; }
            private set { }
        }

        public int SplitScore
        {
            get { return splitHand.currentScore; }
            private set { }
        }

        [HideInInspector] public static float bet = 0;
        [HideInInspector] public PlayerHand hand;
        public PlayerHand splitHand;

        [HideInInspector] public PlayerHand currentHand;

        public bool IsSplitGame { get; private set; } = false;
        public bool IsAceSplitGame = false;

        public static Player _Temp_Instance;

        private void Start()
        {
            _Temp_Instance = this;
            currentHand = hand;
            GameManager.SetPlayer(this);
        }

        public GameObject DealCard(CardData card)
        {
            return currentHand.DealCard(card, FlipType.FlipUp);
        }

        public GameObject AceSplitDeal(CardData card)
        {
            splitHand.splitAceHand = true;
            return splitHand.DealCard(card, FlipType.FlipUp);
        }

        public void ResetScore()
        {
            hand.ResetScore();
            splitHand.ResetScore();
        }

        public void ResetTable()
        {
            hand.ResetHand();
            splitHand.ResetHand();
            IsAceSplitGame = false;
            IsSplitGame = false;
            bet = 0;

            currentHand = hand;
        }

        public bool IsIdle()
        {
            return hand.currentScore == 0 && splitHand.currentScore == 0 && bet == 0;
        }

        public bool IsPlacingBet()
        {
            return bet > 0;
        }

        public bool BothHandsEnded()
        {
            return hand.IsEnded() && splitHand.IsEnded();
        }

        public bool IsEnded()
        {
            if (IsSplitGame)
                return hand.IsEnded() && splitHand.IsEnded();

            return hand.IsEnded();
        }

        public bool HaveCurrentHandEded()
        {
            return currentHand.IsEnded();
        }

        public bool HasDoubleAce()
        {
            return hand.HasSplitAcesPair() && !IsSplitGame;
        }

        public bool IsBusted()
        {
            if (IsSplitGame)
                return hand.IsBust() && splitHand.IsBust();
            else
                return hand.IsBust();
        }

        public bool HasBlackjack()
        {
            return hand.Has_Blackjack() && !IsSplitGame;
        }

        public bool CheckBlackjack()
        {
            return hand.Check_Blackjack() && !IsSplitGame;
        }

        public void ShowWin()
        {
            if (IsSplitGame)
            {
                hand.ShowOutCome(Outcome.Win);
                splitHand.ShowOutCome(Outcome.Win);
            }
            else
                hand.ShowOutCome(Outcome.Win);
        }

        public void ShowNoWin()
        {
            if (IsSplitGame)
            {
                hand.ShowOutCome(Outcome.NoWin);
                splitHand.ShowOutCome(Outcome.NoWin);
            }
            else
                hand.ShowOutCome(Outcome.NoWin);
        }

        public bool Stand()
        {
            bool wasSplitStand = currentHand == splitHand;
            currentHand.Stand();
            currentHand = hand;
            hand.splitAceHand = IsAceSplitGame;
            if (wasSplitStand)
                currentHand.selectionArrow.SetActive(true);

            return wasSplitStand;
        }

        public void Split()
        {
            IsSplitGame = true;
            currentHand = splitHand;

            splitHand.Split(hand.RemoveSecondCard(), FlipType.FlipUp);

            BalanceManager.ChangeBalance(-bet);
            ChipManager.AddToStack(StackType.Split, bet);
        }

        public void DoubleDown()
        {
            if (currentHand == splitHand)
            {
                BalanceManager.ChangeBalance(-bet);
                ChipManager.AddToStack(StackType.DoubleSplit, bet);
            }
            else
            {
                BalanceManager.ChangeBalance(-bet);
                ChipManager.AddToStack(StackType.Double, bet);
            }
        }

        public void Insurance()
        {
            var insuranceBet = bet / 2;
            BalanceManager.ChangeBalance(-insuranceBet);
            ChipManager.AddToStack(StackType.Insurance, insuranceBet);
        }

        public bool CanSplit()
        {
            return hand.HasSplitPair() && !IsSplitGame && bet <= BalanceManager.GetBalance();
        }

        public bool CanDouble()
        {
            return currentHand.currentScore > 7 && currentHand.currentScore < 15 && currentHand.NumberOfCards() == 2 &&
                   bet <= BalanceManager.GetBalance();
        }
    }
}