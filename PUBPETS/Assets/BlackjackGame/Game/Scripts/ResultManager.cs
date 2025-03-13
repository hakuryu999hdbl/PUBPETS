using UnityEngine;
using TMPro;
using DG.Tweening;
namespace Blackjack_Game
{
    public class ResultManager : MonoBehaviour
    {

        public static ResultManager _Instance { get; private set; }

        public static bool betsEnabled = true;

        [SerializeField]
        private TMP_Text winText;

        private void Awake()
        {
            _Instance = this;
        }

        public void SetResult()
        {

            float totalWin = ChipManager.GetWinnings();
            BalanceManager.ChangeBalance(totalWin);
            ShowResult(totalWin);
        }

        public void HideResult()
        {
            winText.text = "";
        }

        //手动修改
        public TMP_Text ShowText;//显示数字

        public void ShowResult(float totalWin)
        {
            print("The total win is: " + totalWin);
            if (totalWin > 0)
            {
                // winText.text = string.Format("<color=yellow>WIN</color> {0}", totalWin.ToString());
                ShowText.gameObject.SetActive(true);
                ShowText.text = totalWin.ToString();



                //AudioManager.SoundPlay(2);
                AudioManager_2.SoundPlay(2);//手动SE音频替换

            }
            else 
            {
                winText.text = "";



                //AudioManager.SoundPlay(5);
                AudioManager_2.SoundPlay(5);//手动SE音频替换

            }
               
        }
    }
}