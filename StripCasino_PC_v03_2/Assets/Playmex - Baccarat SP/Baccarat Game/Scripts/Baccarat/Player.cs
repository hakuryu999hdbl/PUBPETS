using UnityEngine;
using System.Collections.Generic;
namespace Baccarat_Game
{
    public class Player : MonoBehaviour
    {
        public UIStates uiStates;

        [HideInInspector]
        public static float totalBet = 0;
        [HideInInspector]
        public float totalWinnings = 0;
        [HideInInspector]
        public bool endingGame = false;
        [HideInInspector]
        public float startingBet = 0;

        public PlayerHand hand;
        public BankerHand bankerHand;

        [HideInInspector]
        public Hand currentHand;

        public Hand[] playerHands;

        [HideInInspector]
        public List<ChipStack> betStacks;

        public bool canDeal = true;

        private void Start()
        {
            playerHands = new Hand[] { hand, bankerHand };
            betStacks = new List<ChipStack>();
        }

        public void AwardWinnings()
        {
            // Side Bets
            {
                /*
                if (hand.GetCurrentScore() == bankerHand.GetCurrentScore())
                {
                    Debug.Log("TIE");
                    totalWinnings += GetWininigs(BetType.Tie);
                }
                else
                {
                    if (hand.GetCurrentScore() > bankerHand.GetCurrentScore())
                    {
                        Debug.Log("PLAYER WON");
                        totalWinnings += GetWininigs(BetType.Player);
                    }
                    else
                    {
                        Debug.Log("BANKER WON");
                        totalWinnings += GetWininigs(BetType.Banker);
                    }
                }


                if (hand.HasPerfectPair())
                {
                    Debug.Log("PERFECT PAIR to PLAYER");
                    totalWinnings += GetWininigs(0x03);
                    totalWinnings += GetWininigs(0x06);
                    totalWinnings += GetWininigs(0x07);
                }
                else if (bankerHand.HasPerfectPair())
                {
                    Debug.Log("PERFECT PAIR to BANKER");
                    totalWinnings += GetWininigs(0x03);
                    totalWinnings += GetWininigs(0x06);
                    totalWinnings += GetWininigs(0x08);
                }
                else if (hand.HasPair())
                {
                    Debug.Log("PAIR to PLAYER");
                    totalWinnings += GetWininigs(0x06);
                    totalWinnings += GetWininigs(0x07);
                }
                else if (bankerHand.HasPair())
                {
                    Debug.Log("PAIR to BANKER");
                    totalWinnings += GetWininigs(0x06);
                    totalWinnings += GetWininigs(0x08);
                }
                if (hand.GetNumberOfCards() + bankerHand.GetNumberOfCards() < 5)
                {
                    Debug.Log("SMALL");
                    totalWinnings += GetWininigs(0x04);
                }
                else {
                    Debug.Log("BIG");
                    totalWinnings += GetWininigs(0x05);
                }*/
            }

            uiStates.OnStateChange();
        }

        public void ClearBet()
        {
            totalBet = 0;
            betStacks.Clear();
            uiStates.OnStateChange();
        }

        public GameObject DealCard(CardData card, int id)
        {
            if (IsPlacingBet())
            {
                startingBet = totalBet;
            }

            return playerHands[id].DealCard(card);
        }

        public void ResetScore()
        {
            hand.ResetScore();
            bankerHand.ResetScore();
        }

        public void ResetStartBet()
        {
            startingBet = 0;
        }

        public void ResetTable()
        {

            hand.ResetHand();
            bankerHand.ResetHand();

            currentHand = hand;

            endingGame = false;
            uiStates.Reset();
        }

        public bool IsIdle()
        {
            return hand.currentScore == 0 && bankerHand.currentScore == 0 && totalBet == 0;
        }

        public bool IsPlacingBet()
        {
            return totalBet > 0;
        }

        public bool BothHandsEnded()
        {
            return hand.IsEnded() && bankerHand.IsEnded();
        }

        public bool IsPlayerEnded()
        {
            return hand.IsEnded();
        }

        public bool IsBankerEnded()
        {
            return bankerHand.IsEnded();
        }

        public void StandOnPlayertHand()
        {
            hand.Stand();
        }

        public void StandOnBankertHand()
        {
            bankerHand.Stand();
        }

        public void NextHand()
        {
            currentHand = bankerHand;
        }

        public void ResetHand()
        {
            currentHand = hand;
        }
    }
}