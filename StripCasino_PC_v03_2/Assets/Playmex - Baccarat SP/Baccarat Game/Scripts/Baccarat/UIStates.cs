using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Baccarat_Game
{
    public class UIStates : MonoBehaviour
    {

        public Button dealButton;
        public Button clearButton;
        public Button rebetButton;
        public Button rebetX2Button;

        public GameObject gameOverPanel;
        public GameObject gameOverText;

        public Slider volumeSlider;
        public Toggle musicToggle;

        public Player player;
        //public ChipToggle chipToggle;

        bool buttonsEnabled;
        public bool gameEnded = false;

        public static int language = 0;

        public void Awake()
        {
            buttonsEnabled = true;
            ChangeDealText(Translations.DEAL[language]);
        }

        public void Reset()
        {
            OnStateChange();
        }

        public void SetEnabled(bool enabled)
        {
            buttonsEnabled = enabled;

            if (!enabled)
            {
                EnableButton(dealButton, false);
                EnableButton(clearButton, false);
                EnableButton(rebetButton, false);
                EnableButton(rebetX2Button, false);
                //chipToggle.ToggleChips(false);
            }
        }

        private void CheckResets()
        {
            EnableButton(rebetButton, gameEnded);
            EnableButton(rebetX2Button, gameEnded);
        }

        public void OnStateChange()
        {
            CheckResets();
            CheckDeal();
            CheckClear();
        }

        public void ChangeDealText(string text)
        {
            dealButton.GetComponentInChildren<TMP_Text>().text = text;
        }

        private void CheckDeal()
        {

            if (player.IsIdle() || player.bankerHand.IsEnded() || player.hand.IsEnded())
            {
                EnableButton(dealButton, false);

            }
            else if (player.canDeal)
            {
                EnableButton(dealButton, true);

            }
        }

        private void CheckClear()
        {
            if (player.IsPlacingBet() && player.canDeal)
            {
                clearButton.interactable = true;
            }
            else
            {
                clearButton.interactable = false;
            }
        }


        public void EnableButton(Button button, bool enabled)
        {
            button.interactable = enabled;

        }
    }
}