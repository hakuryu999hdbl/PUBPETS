using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Blackjack_Game
{
    public class MenuManager : MonoBehaviour
    {
        private CanvasGroup background;
        public GameObject[] windows;

        public static bool IsActive { get; private set; } = false;

        private void OnEnable()
        {
            if (!background)
                background = GetComponent<CanvasGroup>();
            background.DOComplete();
            background.DOFade(0, .3f).From().SetEase(Ease.InCubic);
            ResultManager.betsEnabled = false;
            IsActive = true;
        }

        private void OnDisable()
        {
            ResultManager.betsEnabled = !GameManager.GameActive;
            IsActive = false;
            HideAllWidows();
        }

        private void HideAllWidows()
        {
            for (int i = 0; i < windows.Length; i++)
            {
                windows[i].SetActive(false);
            }
        }
    }
}