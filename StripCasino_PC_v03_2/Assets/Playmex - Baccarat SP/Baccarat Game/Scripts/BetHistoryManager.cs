using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Baccarat_Game
{
    public class BetHistoryManager : MonoBehaviour
    {

        public static BetHistoryManager _Instance;

        public Button dealButton;
        public Button clearButton;
        public Button rebetButton;
        public Button undoButton;

        [Space]
        public BetSpace[] betSpaces;

        Stack<BetFootPrint> betHistory;

        public bool onGame = false;

        private void Awake()
        {
            _Instance = this;
        }

        void Start()
        {

            betHistory = new Stack<BetFootPrint>();

            undoButton.onClick.AddListener(Undo);
            clearButton.onClick.AddListener(ClearHistory);
            rebetButton.onClick.AddListener(ApplyRebet);
            ResultManager.betsEnabled = true;
        }

        public void Add(int type, float value)
        {
            dealButton.interactable = true;
            undoButton.interactable = true;
            clearButton.interactable = true;
            betHistory.Push(new BetFootPrint(type, value));
        }

        public void Undo()
        {
            AudioManager.SoundPlay(0);
            if (betHistory.Count > 0)
            {
                RemoveLastBet();
            }
            else
            {
                undoButton.interactable = false;
                clearButton.interactable = false;
            }
        }

        private void RemoveLastBet()
        {

            if (betHistory.Count > 0)
            {
                BetFootPrint bet = betHistory.Pop();
                betSpaces[bet.betType].Remove(bet.value);

                if (Player.totalBet <= 0)
                {
                    dealButton.interactable = false;
                }
                if (betHistory.Count == 0)
                {
                    undoButton.interactable = false;
                    clearButton.interactable = false;
                }
            }
        }

        public void ClearHistory()
        {
            dealButton.interactable = false;
            undoButton.interactable = false;
            clearButton.interactable = false;
            AudioManager.SoundPlay(0);
            while (betHistory.Count > 0)
            {
                RemoveLastBet();
            }
            Player.totalBet = 0;
            onGame = false;
        }

        public void HideUI(bool hide)
        {
            dealButton.gameObject.SetActive(!hide);
            undoButton.gameObject.SetActive(!hide);
            clearButton.gameObject.SetActive(!hide);
        }


        float playerBet = 0;
        float bankerBet = 0;
        float tieBet = 0;
        public void ResetHistory()
        {
            playerBet = 0;
            bankerBet = 0;
            tieBet = 0;

            if (betHistory == null)
                return;

            while (betHistory.Count > 0)
            {
                BetFootPrint bet = betHistory.Pop();

                if (bet.betType == 0)
                    playerBet += bet.value;
                else if (bet.betType == 1)
                    bankerBet += bet.value;
                else
                    tieBet += bet.value;
            }
        }

        public void ApplyRebet()
        {
            betSpaces[0].AddBet(playerBet);
            betSpaces[1].AddBet(bankerBet);
            betSpaces[2].AddBet(tieBet);
        }
    }


    public class BetFootPrint
    {
        public int betType;
        public float value;

        public BetFootPrint(int type, float selectedValue)
        {
            betType = type;
            value = selectedValue;
        }
    }
}
