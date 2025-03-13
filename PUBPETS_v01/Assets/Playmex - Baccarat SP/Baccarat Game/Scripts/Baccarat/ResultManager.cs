using UnityEngine;
using System.Collections.Generic;
namespace Baccarat_Game
{
    public class ResultManager : MonoBehaviour
    {

        public static ResultManager Instance { get; private set; }
        public WinSequence winHandler;

        public static bool betsEnabled = true;

        public static int roundCount = -1;

        public static float totalWin = 0;

        public delegate void voidEvent();
        public static event voidEvent onResult;

        private List<BetSpace> betSpaces = new List<BetSpace>();
        public Transform chipTrayPosition;

        private static int playerResult = 0, bankerResult = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void SetResult(int player, int banker)
        {
            totalWin = 0;
            playerResult = player;
            bankerResult = banker;

            foreach (BetSpace betSpace in betSpaces)
            {
                totalWin += betSpace.ResolveBet();
            }
            print("The total win is: " + totalWin);
            BalanceManager.ChangeBalance(totalWin);
            winHandler.ShowResult(totalWin);
            onResult();
        }

        public static Vector3 GetChipWinPosition()
        {
            return Instance.chipTrayPosition.position;
        }

        public static void RegisterBetSpace(BetSpace betSpace)
        {
            Instance.betSpaces.Add(betSpace);
        }

        public static bool IsTie()
        {
            return playerResult == bankerResult;
        }
        public static bool PlayerWon()
        {
            return playerResult > bankerResult;
        }


        //----------------多重下注区测试_SicBo
        public static int total_Sicbo;

        public static bool IsBig()
        {
            // 定义“大”的逻辑，比如总分大于某个值
            //return playerResult + bankerResult >= 11; // 仅为示例，根据游戏规则调整
            return total_Sicbo >= 11; // 仅为示例，根据游戏规则调整

        }

        public static bool IsSmall()
        {
            // 定义“小”的逻辑，比如总分小于某个值
            //return playerResult + bankerResult <= 10; // 仅为示例，根据游戏规则调整
            return total_Sicbo <= 10; // 仅为示例，根据游戏规则调整


        }

        //----------------多重下注区测试_Craps


        public static int Craps_Case;

        public static bool CrapsCase_1_1()
        {
            return Craps_Case == 1;

        }

        public static bool CrapsCase_2_2()
        {
            return Craps_Case == 2;

        }
        public static bool CrapsCase_3_3()
        {
            return Craps_Case == 3;

        }

        public static bool CrapsCase_4_4()
        {
            return Craps_Case == 4;

        }
        public static bool CrapsCase_5_5()
        {
            return Craps_Case == 5;

        }

        public static bool CrapsCase_6_6()
        {
            return Craps_Case == 6;

        }

        public static bool CrapsCase_Ramdon_7()
        {
            return Craps_Case == 7;

        }

        public static bool CrapsCase_1_2()
        {
            return Craps_Case == 8;
        }

        public static bool CrapsCase_5_6()
        {
            return Craps_Case == 9;
        }
    }
}