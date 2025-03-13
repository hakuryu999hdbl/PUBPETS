using UnityEngine;
using TMPro;
namespace Baccarat_Game
{
    public class WinSequence : MonoBehaviour
    {

        public GameObject winPanel;
        public TMP_Text winText;

        public TMP_Text ShowText;//显示数字

        public void ShowResult(float totalWin)
        {
            if (totalWin > 0)
            {
                ShowText.gameObject.SetActive(true);
                ShowText.text = totalWin.ToString("F2");

                //winPanel.SetActive(true);
                //winText.text = string.Format("<color=yellow>WIN</color> {0}", totalWin.ToString("0"));

                //AudioManager.SoundPlay(3);
                AudioManager_2.SoundPlay(3);//手动SE音频替换
            }
            else 
            {
                winPanel.SetActive(false);

                //AudioManager.SoundPlay(4);//增加了失败音效
                AudioManager_2.SoundPlay(4);//手动SE音频替换
            }
                
        }
        //手动修改
        public void HideResult()
        {
            winPanel.SetActive(false);
        }
    }
}
