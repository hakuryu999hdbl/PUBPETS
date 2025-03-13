using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Baccarat_Game
{
    public class BetHistoryManager : MonoBehaviour
    {

        public static BetHistoryManager _Instance;

        public Button dealButton;
        public Button clearButton;
        public Button rebetButton;
        public Button undoButton;

        //手动修改
        [Space]
        public BetSpace[] betSpaces_Baccarat;

        [Space]
        public BetSpace[] betSpaces_DragonTiger;

        [Space]
        public BetSpace[] betSpaces_SicBo;

        [Space]
        public BetSpace[] betSpaces_ChoHan;

        [Space]
        public BetSpace[] betSpaces_Craps;

        public BaccaratGame baccaratGame;//知道当前的游戏模式


        Stack<BetFootPrint> betHistory;

        public bool onGame = false;

        private void Awake()
        {
            _Instance = this;
        }

        void Start()
        {

            betHistory = new Stack<BetFootPrint>();

            undoButton.onClick.AddListener(Undo);
            clearButton.onClick.AddListener(ClearHistory);
            rebetButton.onClick.AddListener(ApplyRebet);
            ResultManager.betsEnabled = true;
        }

        public void Add(int type, float value)
        {
            dealButton.interactable = true;
            undoButton.interactable = true;
            clearButton.interactable = true;
            betHistory.Push(new BetFootPrint(type, value));
        }

        public void Undo()
        {
            //AudioManager.SoundPlay(0);
            AudioManager_2.SoundPlay(0);//手动SE音频替换


            if (betHistory.Count > 0)
            {
                RemoveLastBet();
            }
            else
            {
                undoButton.interactable = false;
                clearButton.interactable = false;
            }
        }

        private void RemoveLastBet()
        {

            if (betHistory.Count > 0)
            {
                BetFootPrint bet = betHistory.Pop();



                switch (BaccaratGame.Baccarat_GameMode)
                {
                    case 0:
                        //龙虎斗赌区
                        betSpaces_DragonTiger[bet.betType].Remove(bet.value);
                        break;
                    case 1:
                        //百家乐模式赌区
                        betSpaces_Baccarat[bet.betType].Remove(bet.value);
                        break;
                    case 2:
                        // 骰宝模式赌区
                        betSpaces_SicBo[bet.betType].Remove(bet.value);
                        break;
                    case 3:
                        // 切换到丁半模式
                        betSpaces_ChoHan[bet.betType].Remove(bet.value);
                        break;
                    case 4:
                        // 切换到花旗骰模式
                        betSpaces_Craps[bet.betType].Remove(bet.value);
                        //betSpaces_SicBo[bet.betType].Remove(bet.value);
                        break;
                }



                if (Player.totalBet <= 0)
                {
                    dealButton.interactable = false;
                }
                if (betHistory.Count == 0)
                {
                    undoButton.interactable = false;
                    clearButton.interactable = false;
                }
            }
        }

        public void ClearHistory()
        {
            dealButton.interactable = false;
            undoButton.interactable = false;
            clearButton.interactable = false;


            //AudioManager.SoundPlay(0);
            AudioManager_2.SoundPlay(0);//手动SE音频替换

            while (betHistory.Count > 0)
            {
                RemoveLastBet();
            }
            Player.totalBet = 0;
            onGame = false;
        }

        public void HideUI(bool hide)
        {
            dealButton.gameObject.SetActive(!hide);
            undoButton.gameObject.SetActive(!hide);
            clearButton.gameObject.SetActive(!hide);
        }


        float playerBet = 0;
        float bankerBet = 0;
        float tieBet = 0;


        //多重下注区测试
        float bigBet = 0;
        float smallBet = 0;


        float Crap_4Bet = 0;
        float Crap_5Bet = 0;
        float Crap_6Bet = 0;
        float Crap_8Bet = 0;
        float Crap_9Bet = 0;
        float Crap_10Bet = 0;

        float Crap_1_1Bet = 0;
        float Crap_2_2Bet = 0;
        float Crap_3_3Bet = 0;
        float Crap_4_4Bet = 0;
        float Crap_5_5Bet = 0;
        float Crap_6_6Bet = 0;
        float Crap_Ramdon_7Bet = 0;
        float Crap_1_2Bet = 0;
        float Crap_5_6Bet = 0;


        public void ResetHistory()
        {

            playerBet = 0;
            bankerBet = 0;
            tieBet = 0;


            //多重下注区测试
            bigBet = 0;
            smallBet = 0;



            Crap_4Bet = 0;
            Crap_5Bet = 0;
            Crap_6Bet = 0;
            Crap_8Bet = 0;
            Crap_9Bet = 0;
            Crap_10Bet = 0;


            Crap_1_1Bet = 0;
            Crap_2_2Bet = 0;
            Crap_3_3Bet = 0;
            Crap_4_4Bet = 0;
            Crap_5_5Bet = 0;
            Crap_6_6Bet = 0;
            Crap_Ramdon_7Bet = 0;
            Crap_1_2Bet = 0;
            Crap_5_6Bet = 0;




            if (betHistory == null)
                return;

            while (betHistory.Count > 0)
            {
                BetFootPrint bet = betHistory.Pop();

                //if (bet.betType == 0)
                //    playerBet += bet.value;
                //else if (bet.betType == 1)
                //    bankerBet += bet.value;
                //else
                //    tieBet += bet.value;

                //多重下注区测试
                switch (bet.betType)
                {
                    case 0:
                        playerBet += bet.value;
                        break;
                    case 1:
                        bankerBet += bet.value;
                        break;
                    case 2:
                        tieBet += bet.value;
                        break;


                    case 3:
                        bigBet += bet.value;
                        break;
                    case 4:
                        smallBet += bet.value;
                        break;


                    case 5:
                        Crap_4Bet += bet.value;
                        break;
                    case 6:
                        Crap_5Bet += bet.value;
                        break;
                    case 7:
                        Crap_6Bet += bet.value;
                        break;
                    case 8:
                        Crap_8Bet += bet.value;
                        break;
                    case 9:
                        Crap_9Bet += bet.value;
                        break;
                    case 10:
                        Crap_10Bet += bet.value;
                        break;



                    case 11:
                        Crap_1_1Bet += bet.value;
                        break;
                    case 12:
                        Crap_2_2Bet += bet.value;
                        break;
                    case 13:
                        Crap_3_3Bet += bet.value;
                        break;
                    case 14:
                        Crap_4_4Bet += bet.value;
                        break;
                    case 15:
                        Crap_5_5Bet += bet.value;
                        break;
                    case 16:
                        Crap_6_6Bet += bet.value;
                        break;
                    case 17:
                        Crap_Ramdon_7Bet += bet.value;
                        break;
                    case 18:
                        Crap_1_2Bet += bet.value;
                        break;
                    case 19:
                        Crap_5_6Bet += bet.value;
                        break;
                }



            }
        }

        public void ApplyRebet()
        {
            switch (BaccaratGame.Baccarat_GameMode)
            {
                case 0:
                    //龙虎斗赌区
                    betSpaces_DragonTiger[0].AddBet(playerBet);
                    betSpaces_DragonTiger[1].AddBet(bankerBet);
                    betSpaces_DragonTiger[2].AddBet(tieBet);
                    break;
                case 1:
                    //百家乐模式赌区
                    betSpaces_Baccarat[0].AddBet(playerBet);
                    betSpaces_Baccarat[1].AddBet(bankerBet);
                    betSpaces_Baccarat[2].AddBet(tieBet);
                    break;
                case 2:
                    // 骰宝模式赌区
                    betSpaces_SicBo[0].AddBet(playerBet);
                    betSpaces_SicBo[1].AddBet(bankerBet);

                    betSpaces_SicBo[2].AddBet(tieBet);     
                    betSpaces_SicBo[3].AddBet(bigBet);
                    betSpaces_SicBo[4].AddBet(smallBet);
                    break;
                case 3:
                    // 切换到丁半模式
                    betSpaces_ChoHan[0].AddBet(playerBet);
                    betSpaces_ChoHan[1].AddBet(bankerBet);
                    break;
                case 4:
                    // 切换到花旗骰模式
                    betSpaces_Craps[0].AddBet(playerBet);
                    betSpaces_Craps[1].AddBet(bankerBet);
                    betSpaces_Craps[2].AddBet(tieBet);

                    betSpaces_Craps[3].AddBet(bigBet);
                    betSpaces_Craps[4].AddBet(smallBet);

                    betSpaces_Craps[5].AddBet(Crap_4Bet);
                    betSpaces_Craps[6].AddBet(Crap_5Bet);
                    betSpaces_Craps[7].AddBet(Crap_6Bet);
                    betSpaces_Craps[8].AddBet(Crap_8Bet);
                    betSpaces_Craps[9].AddBet(Crap_9Bet);
                    betSpaces_Craps[10].AddBet(Crap_10Bet);


                    betSpaces_Craps[11].AddBet(Crap_1_1Bet);
                    betSpaces_Craps[12].AddBet(Crap_2_2Bet);
                    betSpaces_Craps[13].AddBet(Crap_3_3Bet);
                    betSpaces_Craps[14].AddBet(Crap_4_4Bet);
                    betSpaces_Craps[15].AddBet(Crap_5_5Bet);
                    betSpaces_Craps[16].AddBet(Crap_6_6Bet);
                    betSpaces_Craps[17].AddBet(Crap_Ramdon_7Bet);
                    betSpaces_Craps[18].AddBet(Crap_1_2Bet);
                    betSpaces_Craps[19].AddBet(Crap_5_6Bet);

                    break;
            }
          
        }
    }


    public class BetFootPrint
    {
        public int betType;
        public float value;

        public BetFootPrint(int type, float selectedValue)
        {
            betType = type;
            value = selectedValue;
        }
    }
}
