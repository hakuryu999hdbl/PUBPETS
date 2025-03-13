using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // 确保你已经导入了TMPro命名空间

namespace Baccarat_Game
{
    public class SicBoGame : MonoBehaviour
    {
        //各个骰子脚本
        public DiceStats dice1;
        public DiceStats dice2;
        public DiceStats dice3;

        //总和
        public TMP_Text DiceValue;

        public RollJump rollJump;//让三个骰子跳

        public BaccaratGame baccaratGame;//输出结果

        public BetSpace betSpace;//由于豹子出现几率过低，百家乐的和局1赔8改成豹子1赔30

        public void StartSicBoGame()
        {
            StartCoroutine(RollDice());


            betSpace.payout = 30;//豹子【1赔30】

        }

        private IEnumerator RollDice()
        {
            //让SicBo骰子跳
            rollJump.ButtonRollDice();
            AudioManager_2.SoundPlay(2);//手动SE音频替换

            // 等待两秒，保证骰子停止旋转
            yield return new WaitForSeconds(2);

            // 检测骰子点数的总和
            int total = dice1.side + dice2.side + dice3.side;
            //Debug.Log("骰子总和" + total);
            DiceValue.text = total.ToString();

            //显示点数
            DiceValue.gameObject.SetActive(true);

            // 等待一秒再结算
            yield return new WaitForSeconds(1);

            //隐藏点数
            DiceValue.gameObject.SetActive(false);

            // 根据总和判定结果
            //DetermineResult(total);

            ResultManager.total_Sicbo = total;//传过去

            baccaratGame.CheckIfEnded();
        }



        //private void DetermineResult(int total)
        //{
        //    if (total >= 11)
        //    {
        //        Debug.Log("大");//庄家，大，虎，偶数【1赔1】
        //
        //        baccaratGame.player.bankerHand.currentScore = 1;
        //        baccaratGame.player.hand.currentScore = 0;
        //    }
        //    else if (total <= 10)
        //    {
        //        Debug.Log("小");//闲家，小，龙，奇数【1赔1】
        //
        //        baccaratGame.player.bankerHand.currentScore = 0;
        //        baccaratGame.player.hand.currentScore = 1;
        //    }
        //
        //    // 假设和为三个骰子点数相同
        //    if (dice1.side == dice2.side && dice2.side == dice3.side)
        //    {
        //        Debug.Log("和");//豹子【1赔30】豹子出现的时候大小都不算
        //
        //        baccaratGame.player.bankerHand.currentScore = 0;
        //        baccaratGame.player.hand.currentScore = 0;
        //    }
        //
        //
        //    baccaratGame.CheckIfEnded();
        //}

    }
}
