﻿using UnityEngine;
using System.Collections;
namespace Blackjack_Game
{
    public enum GameState
    {
        OnIdle,
        OnDealing,
        OnPlay,
        OnRewards
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager _Instance;

        public Player player;
        public Dealer dealer;
        public UIStates _ui;
        public ResultManager results;
        public TableResetManager table;

        [Space] public int deckCount = 6;
        private CardDeck Deck;

        public GameState State = GameState.OnIdle;
        public static bool GameActive = false;

        private bool Trigger_DoubleDownCheck = false;
        private bool Trigger_CheckInsurance = true;
        private bool Trigger_CheckDoubleAce = false;

        private bool EvenMoneyGame = false;
        private bool EvenMoneyAccepted = false;

        private readonly float DEAL_WAIT_TIME = .5f;

        private void Awake()
        {
            if (_Instance == null)
                _Instance = this;

            Deck = new CardDeck(deckCount);
        }

        private void OnEnable()
        {
            DealQueue.OnFinishedDealing += CheckStatus;
        }

        private void OnDisable()
        {
            DealQueue.OnFinishedDealing -= CheckStatus;
        }

        public void OnClickDeal()
        {
            GameActive = true;
            if (State == GameState.OnIdle)
            {
                StartCoroutine(InitialDeal());
            }
            else
            {
                StartCoroutine(PlayerDeal());
            }
        }

        public void OnClickStand()
        {
            StandPlayerHand();
        }

        public void OnClickDouble()
        {
            Trigger_DoubleDownCheck = true;
            player.DoubleDown();
            StartCoroutine(PlayerDeal());
        }

        public void OnClickSplit()
        {
            if (Trigger_CheckDoubleAce && !player.IsAceSplitGame)
            {
                StartCoroutine(SplitAcetStance());
            }
            else
            {
                player.Split();
                StartCoroutine(PlayerDeal());
            }
        }

        private void StandPlayerHand()
        {
            if (player.Stand()) // Was Split Stand ?
            {
                StartCoroutine(PlayerDeal());
                return;
            }

            CheckStatus();
        }

        private IEnumerator SplitAcetStance()
        {
            ChangeGameState(GameState.OnDealing);
            player.IsAceSplitGame = true;
            player.Split();
            yield return new WaitForSeconds(DEAL_WAIT_TIME / 1.6f);
            DealQueue.DealCard(player.AceSplitDeal(Deck.GetCard()));
            ChangeGameState(GameState.OnPlay);
        }

        private IEnumerator InitialDeal()
        {
            ChangeGameState(GameState.OnDealing);
            for (int i = 0; i < 2; i++)
            {
                DealQueue.DealCard(player.DealCard(Deck.GetCard()));
                yield return new WaitForSeconds(DEAL_WAIT_TIME);
                DealQueue.DealCard(dealer.DealCard(Deck.GetCard(), (FlipType)i));
                yield return new WaitForSeconds(DEAL_WAIT_TIME);
            }

            ChangeGameState(GameState.OnPlay);
        }

        private IEnumerator PlayerDeal()
        {
            ChangeGameState(GameState.OnDealing);
            yield return new WaitForSeconds(DEAL_WAIT_TIME);
            DealQueue.DealCard(player.DealCard(Deck.GetCard()));
            ChangeGameState(GameState.OnPlay);
            if (Trigger_DoubleDownCheck)
            {
                Trigger_DoubleDownCheck = false;
                StandPlayerHand();
            }
        }

        private IEnumerator DealereDeal()
        {
            yield return new WaitForSeconds(DEAL_WAIT_TIME);
            DealQueue.DealCard(dealer.DealCard(Deck.GetCard(), FlipType.FlipUp));
        }

        public static void SetDealer(Dealer dealer)
        {
            _Instance.dealer = dealer;
        }

        public static void SetPlayer(Player player)
        {
            _Instance.player = player;
            _Instance.ChangeGameState(GameState.OnIdle);
        }

        public void CheckStatus()
        {
            _ui.PlayingState(player, State == GameState.OnDealing);

            if (DealQueue.processing) return;

            if (!Trigger_CheckDoubleAce && DealQueue.CardCount == 4 && player.HasDoubleAce())
            {
                Trigger_CheckDoubleAce = true;
            }

            if (Trigger_CheckInsurance && DealQueue.CardCount == 4 && dealer.MayHave21())
            {
                Trigger_CheckInsurance = false;

                bool enoughCredit = Player.bet <= BalanceManager.GetBalance();
                if (player.CheckBlackjack())
                {
                    EvenMoneyGame = true;
                    _ui.insurancePanel.SetActive(true);
                    _ui.insuranceText.text = "Would you like to even money?";
                    return;
                }

                if (enoughCredit)
                {
                    _ui.insurancePanel.SetActive(true);
                    _ui.insuranceText.text = "Do you want to insurance?";
                    return;
                }
            }

            if (player.IsEnded() || EvenMoneyGame)
            {
                print("Player Ended");
                if (dealer.HasTurnDownCard())
                {
                    print("Has TurnDown Card");
                    dealer.RevealCard();
                    return;
                }

                if (!dealer.IsEnded(player.IsBusted()) && !player.CheckBlackjack() && !EvenMoneyGame)
                {
                    print("Dealer Turn");
                    StartCoroutine(DealereDeal());
                }
                else
                    EndGame();
            }
            else if (player.HaveCurrentHandEded())
                StandPlayerHand();
        }

        public void EndGame()
        {
            print("Game Ended");
            ChangeGameState(GameState.OnRewards);

            if (EvenMoneyGame)
            {
                if (EvenMoneyAccepted)
                {
                    ChipManager.SetWinningStack(StackType.Standard, true);
                    player.hand.ShowOutCome(Outcome.Win);
                }
                else
                {
                    if (dealer.HasBlackjack())
                        player.hand.IsPush(dealer.Score);
                    else
                        EvenMoneyAccepted = false;
                }
            }

            if (!EvenMoneyAccepted)
            {
                bool dealerBlackjack = dealer.HasBlackjack();
                bool playerBlackjack = player.HasBlackjack();
                bool won = player.Score > dealer.Score || dealer.IsBusted() || playerBlackjack && !dealerBlackjack;
                bool push = player.Score == dealer.Score || dealerBlackjack && playerBlackjack;
                won &= !player.hand.IsBust();

                ChipManager.SetWinningStack(StackType.Standard, won);
                ChipManager.SetPushStack(StackType.Standard, push);
                ChipManager.SetWinningStack(StackType.Double, won);
                ChipManager.SetPushStack(StackType.Double, push);
                ChipManager.SetWinningStack(StackType.Insurance, dealerBlackjack);

                if (won)
                {
                    if (playerBlackjack && !push)
                        ChipManager.SetBlackjackStack();
                    else
                        player.hand.ShowOutCome(Outcome.Win);
                }
                else if (!player.hand.IsPush(dealer.Score, dealerBlackjack))
                    player.hand.ShowOutCome(Outcome.NoWin);

                if (player.IsSplitGame)
                {
                    won = player.SplitScore > dealer.Score || dealer.Score > 21;
                    won &= !player.splitHand.IsBust();

                    ChipManager.SetWinningStack(StackType.Split, won);
                    ChipManager.SetWinningStack(StackType.DoubleSplit, won);

                    if (won)
                        player.splitHand.ShowOutCome(Outcome.Win);
                    else if (!player.splitHand.IsPush(dealer.Score, dealerBlackjack))
                        player.splitHand.ShowOutCome(Outcome.NoWin);
                }
            }

            StartCoroutine(ResetTable());
        }

        public void AcceptInsurance()
        {
            if (player.CheckBlackjack())
                EvenMoneyAccepted = true;
            else
                player.Insurance();

            CheckStatus();
        }

        public void DenyInsurance()
        {
            if (player.HasBlackjack())
                CheckStatus();
        }

        public IEnumerator ResetTable()
        {
            DealQueue.CardCount = 0;
            results.SetResult();
            Trigger_CheckInsurance = true;
            Trigger_CheckDoubleAce = false;
            EvenMoneyGame = false;
            EvenMoneyAccepted = false;
            _ui.ShowChips(false);
            yield return new WaitForSeconds(3);
            player.ResetTable();
            results.HideResult();
            dealer.ResetTable();
            table.Cleanup();
            ChangeGameState(GameState.OnIdle);
            GameActive = false;
        }

        private void ChangeGameState(GameState newState)
        {
            State = newState;
            _ui.ChangeByGameState(State);
        }
    }
}