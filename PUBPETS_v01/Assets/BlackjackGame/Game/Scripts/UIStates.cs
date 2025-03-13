using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace Blackjack_Game
{
    public class UIStates : MonoBehaviour
    {

        [Header("Buttons")]
        public Button dealButton;
        public Button clearButton;
        public Button standButton;
        public Button undoButton;
        public Button doubleButton;
        public Button splitButton;


        [Space]
        [Header("Panels")]
        public CanvasGroup _MainUI;
        public CanvasGroup chipsPanel;
        public CanvasGroup buttonsPanel;
        public GameObject insurancePanel;
        public TMP_Text insuranceText;

        [Space]
        [Header("Audio UI")]
        public Slider volumeSlider;
        public Toggle musicToggle;
        [Space]
        public Image checkMark;
        public Sprite selected;
        public Sprite noVolume;

        public void SetEnableUI(bool enabled)
        {
            _MainUI.interactable = enabled;
        }

        public void DisableButtons()
        {
            buttonsPanel.interactable = false;

            clearButton.gameObject.SetActive(false);
            undoButton.gameObject.SetActive(false);
        }

        public void BetState()
        {
            buttonsPanel.interactable = true;
            clearButton.gameObject.SetActive(true);
            undoButton.gameObject.SetActive(true);

            dealButton.interactable = true;
            clearButton.interactable = true;
            undoButton.interactable = true;

            standButton.interactable = false;
            doubleButton.interactable = false;
            splitButton.interactable = false;
        }

        public void PlayingState(Player player, bool onInitialDeal)
        {
            if (player.IsEnded() || onInitialDeal)
            {
                DisableButtons();
                return;
            }
            buttonsPanel.interactable = true;

            dealButton.interactable = true;
            standButton.interactable = true;

            doubleButton.interactable = player.CanDouble();
            splitButton.interactable = player.CanSplit();
        }

        public void ShowChips(bool show)
        {
            chipsPanel.interactable = show;
            ResultManager.betsEnabled = show;
        }

        public void ChangeByGameState(GameState state)
        {
            if (state == GameState.OnIdle)
            {
                UseHitButtonState(false);
                DisableButtons();
                ShowChips(true);
            }
            else if (state == GameState.OnPlay || state == GameState.OnDealing)
            {
                UseHitButtonState(true);
                DisableButtons();
                ShowChips(false);
            }
        }

        public void CheckVolumeState()
        {
            if (volumeSlider.value == 0)
                checkMark.sprite = noVolume;
            else
                checkMark.sprite = selected;

        }

        //手动修改
        public GameObject DealText;
        public GameObject HitText;

        public void UseHitButtonState(bool enable)
        {
            //dealButton.GetComponentInChildren<TMP_Text>().text = enable ? "Hit" : "Deal";


            // 激活相应的文本并禁用另一个
            DealText.SetActive(!enable);
            HitText.SetActive(enable);
        }
    }
}