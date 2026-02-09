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


            // ✅ 奖励翻倍（只在赢的时候加成）
            //float mul = GameManager._Instance != null ? GameManager._Instance.rewardMultiplier : 1f;
            //if (totalWin > 0) totalWin *= mul;

            if (totalWin > 0)
            {
                float mul = GameManager._Instance.rewardMultiplier;
                totalWin *= mul;
            }


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




                AudioManager_2.SoundPlay(3);//手动SE音频替换

            }
            else
            {
                winText.text = "";



                AudioManager_2.SoundPlay(4);//手动SE音频替换

            }

        }
    }
}