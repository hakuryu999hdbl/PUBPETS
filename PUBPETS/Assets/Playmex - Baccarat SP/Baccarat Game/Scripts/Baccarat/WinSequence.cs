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


                //Invoke(nameof(InvokeWinVoice), 0.2f);//女荷官使用物品的时候声音先别出来
            }
            else 
            {
                winPanel.SetActive(false);


                //Invoke(nameof(InvokeLoseVoice), 0.2f);//女荷官使用物品的时候声音先别出来
            }
                
        }



        //public void InvokeWinVoice() 
        //{
        //    AudioManager_2.SoundPlay(3);//手动SE音频替换
        //}
        //public void InvokeLoseVoice()
        //{
        //    AudioManager_2.SoundPlay(4);//手动SE音频替换
        //}





        //手动修改
        public void HideResult()
        {
            winPanel.SetActive(false);
        }
    }
}
