using UnityEngine;
using System.Collections.Generic;
namespace Blackjack_Game
{
    public class BetHistoryManager : MonoBehaviour
    {

        public static BetHistoryManager _Instance;

        public UIStates _ui;

        private Player _Player;
        private Stack<float> BetHistory;

        private void Awake()
        {
            _Instance = this;
            BetHistory = new Stack<float>();
        }

        void Start()
        {
            _ui.clearButton.onClick.AddListener(ClearHistory);
            _ui.undoButton.onClick.AddListener(Undo);
        }

        public void Add(float value)
        {
            BetHistory.Push(value);
            _ui.BetState();
        }

        public void Undo()
        {
            AudioManager.SoundPlay(0);
            if (BetHistory.Count > 0)
            {
                RemoveLastBet();
            }
            else
            {
                _ui.splitButton.interactable = false;
                _ui.clearButton.interactable = false;
            }
        }

        private void RemoveLastBet()
        {
            float bet = BetHistory.Pop();
            Player.bet -= bet;
            BalanceManager.ChangeBalance(bet);
            ChipManager.RemoveFromStack(StackType.Standard, bet);

            if (BetHistory.Count == 0 || Player.bet <= 0)
            {
                _ui.DisableButtons();
            }
        }

        public void ClearHistory()
        {
            AudioManager.SoundPlay(0);
            _ui.DisableButtons();
            BetHistory.Clear();

            BalanceManager.ChangeBalance(Player.bet);
            ChipManager.ClearStack(StackType.Standard);
            Player.bet = 0;
        }

        float playerBet = 0;

        public void ResetHistory()
        {
            playerBet = 0;
            if (BetHistory == null)
                return;

        }

        public static void SetPlayer(Player player)
        {
            _Instance._Player = player;
        }

    }
}