using UnityEngine;
using TMPro;
namespace Baccarat_Game
{
    public class WinSequence : MonoBehaviour
    {

        public GameObject winPanel;
        public TMP_Text winText;

        public void ShowResult(float totalWin)
        {
            if (totalWin > 0)
            {
                //AudioManager.getInstance().Play("win", 1.0f);
                winPanel.SetActive(true);
                winText.text = string.Format("<color=yellow>WIN</color> {0}", totalWin.ToString("0"));
                AudioManager.SoundPlay(3);
            }
            else
                winPanel.SetActive(false);
        }

        public void HideResult()
        {
            winPanel.SetActive(false);
        }
    }
}
