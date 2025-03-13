using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // 确保你已经导入了TMPro命名空间

namespace Baccarat_Game
{
    public class ChoHanGame : MonoBehaviour
    {
        //各个骰子脚本
        public DiceStats dice1;
        public DiceStats dice2;

        //总和
        public TMP_Text DiceValue;

        public RollJump rollJump;//让三个骰子跳

        public BaccaratGame baccaratGame;//输出结果

        public void StartChoHanGame()
        {
            StartCoroutine(RollDice());        

        }

        private IEnumerator RollDice()
        {
            //让ChoHan骰子跳
            rollJump.ButtonRollDice();
            AudioManager_2.SoundPlay(2);//手动SE音频替换

            // 等待两秒，保证骰子停止旋转
            yield return new WaitForSeconds(2);

            // 检测骰子点数的总和
            int total = dice1.side + dice2.side;
            //Debug.Log("骰子总和" + total);
            DiceValue.text = total.ToString();


            //显示点数
            DiceValue.gameObject.SetActive(true);

            // 等待一秒再结算
            yield return new WaitForSeconds(1);

            //隐藏点数
            DiceValue.gameObject.SetActive(false);

            // 根据总和判定结果
            DetermineResult(total);
        }



        private void DetermineResult(int total)
        {
            if (total % 2 == 0)//余数为0
            {
                Debug.Log("丁");//庄家，大，虎，偶数【1赔1】

                baccaratGame.player.bankerHand.currentScore = 1;
                baccaratGame.player.hand.currentScore = 0;
            }
            else
            {
                Debug.Log("半");//闲家，小，龙，奇数【1赔1】

                baccaratGame.player.bankerHand.currentScore = 0;
                baccaratGame.player.hand.currentScore = 1;
            }


            baccaratGame.CheckIfEnded();
        }
    }
}