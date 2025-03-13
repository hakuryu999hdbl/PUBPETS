using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // 确保你已经导入了TMPro命名空间


namespace Baccarat_Game
{
    public class CrapsGame : MonoBehaviour
    {
        //各个骰子脚本
        public DiceStats dice1;
        public DiceStats dice2;

        //总和
        public TMP_Text DiceValue;


        public RollDrop rollDrop;//可以扔


        public GameObject mainCamera; // 确保已经将主相机拖拽至此变量

        public float cameraMoveDuration = 1f; // 相机移动到骰子上方的时间

        public float cameraHeightAboveDice = 1f; // 相机距离骰子的高度

        private Vector3 originalCameraPosition; // 用来保存相机的初始位置
        private Quaternion originalCameraRotation; // 用来保存相机的初始旋转

        bool isSave = false;

        public Animator CameraPosition;//投出去需要关闭摄像机动画器

        public void StartCrapsGame()
        {
            StartCoroutine(RollDice());
            Debug.Log("显示花旗骰点数");
        }

        private IEnumerator RollDice()
        {
            CameraPosition.enabled = false;

            if (!isSave)
            {
                originalCameraPosition = mainCamera.transform.position; // 保存相机的初始位置
                originalCameraRotation = mainCamera.transform.rotation; // 保存相机的初始旋转

                isSave = true;
            }//只保存最开始一次


            // 移动相机到第一个骰子正上方
            Vector3 dicePosition = dice1.transform.position + Vector3.up * cameraHeightAboveDice;
            Quaternion lookDownRotation = Quaternion.Euler(90, 0, 0);
            yield return StartCoroutine(MoveCamera(dicePosition, lookDownRotation));

            yield return new WaitForSeconds(0.1f); // 在第一个骰子上方停留一秒

            // 移动相机到第二个骰子正上方
            dicePosition = dice2.transform.position + Vector3.up * cameraHeightAboveDice;
            yield return StartCoroutine(MoveCamera(dicePosition, lookDownRotation));

            yield return new WaitForSeconds(0.1f); // 在第二个骰子上方停留一秒



            // 检测骰子点数的总和
            int total = dice1.side + dice2.side;
            //Debug.Log("骰子总和" + total);
            DiceValue.text = total.ToString();


            //显示点数
            DiceValue.gameObject.SetActive(true);

            // 等待一秒再结算
            yield return new WaitForSeconds(1f);

            //隐藏点数
            DiceValue.gameObject.SetActive(false);

            // 根据总和判定结果
            DetermineResult(total);


            // 返回相机到初始位置和角度
            yield return StartCoroutine(MoveCamera(originalCameraPosition, originalCameraRotation));

            // 等待一秒再结算
            yield return new WaitForSeconds(1f);

            CameraPosition.enabled = true;



            //全部显示完毕才可以再扔
            rollDrop.canThrow = true;

        }


        public BaccaratGame baccaratGame;//输出结果
        public BetHistoryManager betHistoryManager;//知晓过线区赔率


        public int isSetPoint = 0;//是否确定点数
        public Animator Craps_OnOff;


        public TMP_Text ShowText;//显示数字





        private void DetermineResult(int total)
        {
            //暂时让骰子消失
            baccaratGame.CrapsGameObject.SetActive(false);


            //优先结算指定
            if (dice1.side == 1 && dice2.side == 1 && betHistoryManager.betSpaces_Craps[11].Bet != 0)
            {
                ResultManager.Craps_Case = 1;
                baccaratGame.CheckIfEnded();
            }
            //优先结算指定
            if (dice1.side == 2 && dice2.side == 2 && betHistoryManager.betSpaces_Craps[12].Bet != 0)
            {
                ResultManager.Craps_Case = 2;
                baccaratGame.CheckIfEnded();
            }
            //优先结算指定
            if (dice1.side == 3 && dice2.side == 3 && betHistoryManager.betSpaces_Craps[13].Bet != 0)
            {
                ResultManager.Craps_Case = 3;
                baccaratGame.CheckIfEnded();
            }
            //优先结算指定
            if (dice1.side == 4 && dice2.side == 4 && betHistoryManager.betSpaces_Craps[14].Bet != 0)
            {
                ResultManager.Craps_Case = 4;
                baccaratGame.CheckIfEnded();
            }
            //优先结算指定
            if (dice1.side == 5 && dice2.side == 5 && betHistoryManager.betSpaces_Craps[15].Bet != 0)
            {
                ResultManager.Craps_Case = 5;
                baccaratGame.CheckIfEnded();
            }
            //优先结算指定
            if (dice1.side == 6 && dice2.side == 6 && betHistoryManager.betSpaces_Craps[16].Bet != 0)
            {
                ResultManager.Craps_Case = 6;
                baccaratGame.CheckIfEnded();
            }
            //优先结算指定
            if (total == 7 && betHistoryManager.betSpaces_Craps[17].Bet != 0)
            {
                ResultManager.Craps_Case = 7;
                baccaratGame.CheckIfEnded();
            }

            //优先结算指定
            if (total == 3 && betHistoryManager.betSpaces_Craps[18].Bet != 0)
            {
                ResultManager.Craps_Case = 8;
                baccaratGame.CheckIfEnded();
            }

            //优先结算指定
            if (total == 11 && betHistoryManager.betSpaces_Craps[19].Bet != 0)
            {
                ResultManager.Craps_Case = 9;
                baccaratGame.CheckIfEnded();
            }




            if (isSetPoint==0)
            {
                if (total == 7 || total == 11)
                {
                    //过线区结算

                    if (betHistoryManager.betSpaces_Craps[0].Bet != 0)
                    {
                        //赢了也会移走赌注
                        baccaratGame.player.bankerHand.currentScore = 0;
                        baccaratGame.player.hand.currentScore = 1;

                        baccaratGame.CheckIfEnded();


                    }
                   


                    //非过线区结算
                    if (betHistoryManager.betSpaces_Craps[1].Bet!=0) 
                    {
                        CleanDesk();
                    }

                    //场内结算
                    //if (betHistoryManager.betSpaces_Craps[2].Bet != 0)
                    //{
                    //    FieldBet(true);
                    //}


                    //防止卡死
                    CanBetBack();

                }
                else if (total == 2 || total == 3 || total == 12)
                {

                    //非过线区结算
                    if (betHistoryManager.betSpaces_Craps[1].Bet != 0)
                    {

                        //赢了也会移走赌注
                        baccaratGame.player.bankerHand.currentScore = 1;
                        baccaratGame.player.hand.currentScore = 0;

                        baccaratGame.CheckIfEnded();
                    }

                    //过线区结算
                    if (betHistoryManager.betSpaces_Craps[0].Bet != 0)
                    {
                        CleanDesk();
                    }


                    //场内结算
                    //if (betHistoryManager.betSpaces_Craps[2].Bet != 0)
                    //{
                    //    if (total == 2 || total == 12)
                    //    {
                    //        FieldBet(true);
                    //    }
                    //}


                    //防止卡死
                    CanBetBack();

                }
                else
                {




                    //其他选项
                    CanBetBack();

                    //进入确定点数状态
                    isSetPoint = total;
                    Craps_OnOff.SetInteger("Point", isSetPoint);

                    //AudioManager.SoundPlay(0);
                    AudioManager_2.SoundPlay(0);//手动SE音频替换




                    //场内结算
                    //if (betHistoryManager.betSpaces_Craps[2].Bet != 0)
                    //{
                    //
                    //
                    //    if (total == 4 || total == 9)
                    //    {
                    //        FieldBet(false);
                    //    }
                    //
                    //    if (total == 5 || total == 6 || total == 7 || total == 8)
                    //    {
                    //
                    //        CleanDesk();
                    //
                    //    }
                    //}



                }
            }
            else 
            {

                switch (total) 
                {
                    case 4:
                        //表达结果
                        float winAmount4 = betHistoryManager.betSpaces_Craps[5].Bet * (9.0f / 5.0f);
                        NumberCount(winAmount4, 4);
                        break;
                    case 10:
                        //表达结果
                        float winAmount10 = betHistoryManager.betSpaces_Craps[10].Bet * (9.0f / 5.0f);
                        NumberCount(winAmount10, 10);
                        break;



                    case 5:
                        //表达结果
                        float winAmount5 = betHistoryManager.betSpaces_Craps[6].Bet * (7.0f / 5.0f);
                        NumberCount(winAmount5, 5);
                        break;
                    case 9:
                        //表达结果
                        float winAmount9 = betHistoryManager.betSpaces_Craps[9].Bet * (7.0f / 5.0f);
                        NumberCount(winAmount9, 9);
                        break;


                    case 6:
                        //表达结果
                        float winAmount6 = betHistoryManager.betSpaces_Craps[7].Bet * (7.0f / 6.0f);
                        NumberCount(winAmount6, 6);
                        break;
                    case 8:
                        //表达结果
                        float winAmount8 = betHistoryManager.betSpaces_Craps[8].Bet * (7.0f / 6.0f);
                        NumberCount(winAmount8, 8);
                        break;

                    case 7:


                        //非过线区结算
                        //if (betHistoryManager.betSpaces_Craps[3].Bet != 0)
                        //{
                        //    ComeBet();//来区1：1
                        //}


                        //AudioManager.SoundPlay(4);
                        AudioManager_2.SoundPlay(4);//手动SE音频替换

                        //我不放置【和】的选项，然后这个就是100%失败
                        baccaratGame.player.bankerHand.currentScore = 0;
                        baccaratGame.player.hand.currentScore = 0;

                        baccaratGame.CheckIfEnded();

                        //挂了Point也要回去了
                        isSetPoint = 0; Craps_OnOff.SetInteger("Point", 0);




                        break;

                    case 11:

                        //非过线区结算
                        if (betHistoryManager.betSpaces_Craps[3].Bet != 0)
                        {
                            ComeBet();//来区1：1
                        }

                        CanBetBack();

                        break;


                    case 2:
                    case 3:
                    case 12:

                        //非过线区结算
                        if (betHistoryManager.betSpaces_Craps[4].Bet != 0)
                        {
                            DontComeBet();//不来区1：1
                        }

                        CanBetBack();

                        break;

                    default:
                        // 其他情况，不处理
                        CanBetBack();
                        break;

                }

            }


        }


        void CleanDesk() 
        {

            //AudioManager.SoundPlay(4);
            AudioManager_2.SoundPlay(4);//手动SE音频替换

            //我不放置【和】的选项，然后这个就是100%失败
            baccaratGame.player.bankerHand.currentScore = 0;
            baccaratGame.player.hand.currentScore = 0;

            baccaratGame.CheckIfEnded();
        }

        void NumberCount(float Value, int Total) //该数字上放了多少筹码，这是什么数字
        {         
            BalanceManager.ChangeBalance(Value);
            baccaratGame.resultManager.winHandler.ShowResult(Value);

            //AudioManager.SoundPlay(3);
            AudioManager_2.SoundPlay(3);//手动SE音频替换

            ShowText.gameObject.SetActive(true);
            ShowText.text = Value.ToString("F2");

            CanBetBack();

            //返回不确定点数
            if (Total == isSetPoint)
            {
                isSetPoint = 0; Craps_OnOff.SetInteger("Point", 0);
                //AudioManager.SoundPlay(0);
                AudioManager_2.SoundPlay(0);//手动SE音频替换


            }
        }


        //void FieldBet(bool is2_12)//2,12赔率2：1  //3，4，9，10，11赔率1：1
        //{
        //    if (is2_12)
        //    {
        //        BalanceManager.ChangeBalance(betHistoryManager.betSpaces_Craps[2].Bet *2);
        //        baccaratGame.resultManager.winHandler.ShowResult(betHistoryManager.betSpaces_Craps[2].Bet * 2);
        //    }
        //    else 
        //    {
        //        BalanceManager.ChangeBalance(betHistoryManager.betSpaces_Craps[2].Bet);
        //        baccaratGame.resultManager.winHandler.ShowResult(betHistoryManager.betSpaces_Craps[2].Bet);
        //    }
        //
        //  
        //
        //    //AudioManager.SoundPlay(3);
        //    AudioManager_2.SoundPlay(3);//手动SE音频替换
        //
        //    ShowText.gameObject.SetActive(true);
        //    ShowText.text = betHistoryManager.betSpaces_Craps[2].Bet.ToString("F2");
        //
        //}


        void ComeBet() 
        {
            BalanceManager.ChangeBalance(betHistoryManager.betSpaces_Craps[3].Bet);
            baccaratGame.resultManager.winHandler.ShowResult(betHistoryManager.betSpaces_Craps[3].Bet);

            //AudioManager.SoundPlay(3);
            AudioManager_2.SoundPlay(3);//手动SE音频替换

            ShowText.gameObject.SetActive(true);
            ShowText.text = betHistoryManager.betSpaces_Craps[3].Bet.ToString("F2");


        }

        void DontComeBet() 
        {
            BalanceManager.ChangeBalance(betHistoryManager.betSpaces_Craps[4].Bet);
            baccaratGame.resultManager.winHandler.ShowResult(betHistoryManager.betSpaces_Craps[4].Bet);

            //AudioManager.SoundPlay(3);
            AudioManager_2.SoundPlay(3);//手动SE音频替换

            ShowText.gameObject.SetActive(true);
            ShowText.text = betHistoryManager.betSpaces_Craps[4].Bet.ToString("F2");



        }

        void CanBetBack()
        {
            //允许放筹码
            baccaratGame.uiStates.SetEnabled(true);
            baccaratGame.player.canDeal = true;
            baccaratGame.OnPlayfalse();
            ResultManager.betsEnabled = true;
            baccaratGame.resultManager.winHandler.winPanel.SetActive(false);

            BetHistoryManager._Instance.dealButton.interactable = true;
            BetHistoryManager._Instance.undoButton.interactable = true;
            BetHistoryManager._Instance.clearButton.interactable = true;
        }



        private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
        {
            float elapsedTime = 0;
            Vector3 startingPos = mainCamera.transform.position;
            Quaternion startingRot = mainCamera.transform.rotation;

            while (elapsedTime < cameraMoveDuration)
            {
                mainCamera.transform.position = Vector3.Lerp(startingPos, targetPosition, elapsedTime / cameraMoveDuration);
                mainCamera.transform.rotation = Quaternion.Lerp(startingRot, targetRotation, elapsedTime / cameraMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            mainCamera.transform.position = targetPosition;
            mainCamera.transform.rotation = targetRotation;
        }
    }

}
