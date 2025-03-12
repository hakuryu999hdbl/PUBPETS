using System.Collections;
using UnityEngine;

namespace Baccarat_Game
{
    public class BaccaratGame : MonoBehaviour
    {

        public Player player;

        private int[] handsOnGame = { 0, 1 };

        public CardDeck cardDeck;
        public UIStates uiStates;
        public ResultManager resultManager;
        public TableResetManager tabletReset;

        public int deckCount = 3;

        private float END_GAME_PAUSE = 1.2f;

        [HideInInspector]
        public delegate void OnGameReady();
        public static OnGameReady Done;

        public static bool OnPlay { get; private set; } = false;

        void Awake()
        {
            cardDeck = new CardDeck(deckCount);
        }

        private void OnEnable()
        {
            DealQueue.OnFinishedDealing += FinishedDealing;
            TableResetManager.Done += HardResetTable;
            //ChipManager.clearBets += ClearAllPLayerBets;
        }

        private void OnDisable()
        {
            DealQueue.OnFinishedDealing -= FinishedDealing;
            TableResetManager.Done -= HardResetTable;
        }

        void Start()
        {
            ResetTable();
        }

        public void ClearAllPLayerBets()
        {
            player.ClearBet();
        }

        public void Deal()
        {
            resultManager.winHandler.HideResult();
            if (player.IsPlacingBet())
            {
                ResultManager.betsEnabled = false;
                StartCoroutine(InitialDeal());
            }
        }

        private IEnumerator InitialDeal()
        {
            OnPlay = true;
            for (int i = 0; i < 2; i++)
            {
                DealHand(0);
                yield return new WaitForSeconds(.3f);
                DealHand(1);
                yield return new WaitForSeconds(.3f);
            }
            player.canDeal = false;
        }

        private void DealHand(int id) // 0 Is player, 1 is Banker
        {
            if (player.playerHands[id].GetNumberOfCards() < 3)
            {
                uiStates.SetEnabled(false);
                CardData card = cardDeck.GetCard();
                DealQueue.DealCard(player.DealCard(card, id));
            }
        }

        void FinishedDealing()
        {
            uiStates.SetEnabled(true);
            uiStates.OnStateChange();

            Debug.Log("DEALING FINISHED");
            CheckIfEnded();
        }

        private bool playerHas3 = false;

        public void CheckIfEnded()
        {
            if (player.IsPlayerEnded())
            {
                player.NextHand();
            }
            else if (player.IsBankerEnded())
            {
                PlayerIsFinished();
            }

            // PLAYER RULES
            if (player.hand.GetCurrentScore() >= 0 && player.hand.GetCurrentScore() < 6 && player.hand.GetNumberOfCards() == 2)
            {
                DealHand(0);
                playerHas3 = true;
                player.hand.Stand();
            }
            else if (player.hand.GetCurrentScore() >= 6 || player.hand.GetNumberOfCards() >= 3)
                player.hand.Stand();

            // BANKER RULES
            if ((player.hand.GetCurrentScore() == 6 || player.hand.GetCurrentScore() == 7) && player.bankerHand.GetCurrentScore() < 6 && player.bankerHand.GetNumberOfCards() == 2)
            {
                DealHand(1);
                player.bankerHand.Stand();
            }

            else if (playerHas3 && player.bankerHand.GetNumberOfCards() == 2 && player.hand.GetNumberOfCards() == 3 && player.bankerHand.GetCurrentScore() < 7)
            {
                int playerThirdCard = player.hand.TakeValueFromCard(2);
                int bankerScore = player.bankerHand.GetCurrentScore();

                if (bankerScore < 3)
                {
                    DealHand(1);
                    player.bankerHand.Stand();
                }
                else if (bankerScore == 3 && playerThirdCard != 8)
                {
                    DealHand(1);
                    player.bankerHand.Stand();
                }
                else if (bankerScore == 4 && playerThirdCard > 1 && playerThirdCard < 8)
                {
                    DealHand(1);
                    player.bankerHand.Stand();
                }
                else if (bankerScore == 5 && playerThirdCard > 3 && playerThirdCard < 8)
                {
                    DealHand(1);
                    player.bankerHand.Stand();
                }
                else if (bankerScore == 6 && playerThirdCard == 6 || playerThirdCard == 7)
                {
                    DealHand(1);
                    player.bankerHand.Stand();
                }
                else
                {
                    player.bankerHand.Stand();
                }
            }
            else
            {
                player.bankerHand.Stand();
            }

            PlayerIsFinished();
        }

        private void PlayerIsFinished()
        {
            if (player.BothHandsEnded())
            {
                StartCoroutine(EndGame());
            }
        }

        private IEnumerator EndGame()
        {
            while (DealQueue.processing)
            {
                yield return new WaitForSeconds(.5f);
            }

            if (!player.endingGame)
            {
                player.endingGame = true;
                Debug.Log(player.name + " Is getting their awards...");
                resultManager.SetResult(player.hand.GetCurrentScore(), player.bankerHand.GetCurrentScore());
                uiStates.gameEnded = true;
                yield return new WaitForSeconds(2);
                ResetTable();
            }
        }

        private void ResetTable()
        {
            player.ResetScore();
            tabletReset.Cleanup();
            BetHistoryManager._Instance.ResetHistory();
            Done();
            ResultManager.betsEnabled = !MenuManager.IsActive;
            Player.totalBet = 0;
        }

        public void Rebet()
        {
            resultManager.winHandler.HideResult();
            uiStates.OnStateChange();
            player.canDeal = true;
        }

        private void HardResetTable()
        {
            OnPlay = false;
            player.ResetTable();
            player.canDeal = true;
        }

    }
}
