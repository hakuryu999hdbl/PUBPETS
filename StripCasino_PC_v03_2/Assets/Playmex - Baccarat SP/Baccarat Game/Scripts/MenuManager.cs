using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Baccarat_Game
{
    public class MenuManager : MonoBehaviour
    {
        private Image background;
        public GameObject[] windows;

        public static bool IsActive { get; private set; } = false;

        private void OnEnable()
        {
            if (!background)
                background = GetComponent<Image>();
            else
            {
                background.DOComplete();
                background.DOFade(0, .3f).From().SetEase(Ease.InCubic);
            }
            ResultManager.betsEnabled = false;
            IsActive = true;
        }

        private void OnDisable()
        {
            ResultManager.betsEnabled = !BaccaratGame.OnPlay;
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
