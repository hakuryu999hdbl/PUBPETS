using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Baccarat_Game
{
    public class BaccaratGame : MonoBehaviour
    {

        public Player player;

        private int[] handsOnGame = { 0, 1 };

        public CardDeck cardDeck;
        public UIStates uiStates;
        public ResultManager resultManager;
        public TableResetManager tabletReset;

        public int deckCount = 3;

        private float END_GAME_PAUSE = 1.2f;

        [HideInInspector]
        public delegate void OnGameReady();
        public static OnGameReady Done;

        public static bool OnPlay { get; private set; } = false;

        void Awake()
        {
            cardDeck = new CardDeck(deckCount);
        }

        private void OnEnable()
        {
            DealQueue.OnFinishedDealing += FinishedDealing;
            TableResetManager.Done += HardResetTable;
            //ChipManager.clearBets += ClearAllPLayerBets;
        }

        private void OnDisable()
        {
            DealQueue.OnFinishedDealing -= FinishedDealing;
            TableResetManager.Done -= HardResetTable;
        }

        void Start()
        {
            ResetTable();

            Baccarat_GameMode = 4;
        }

        public void ClearAllPLayerBets()
        {
            player.ClearBet();
        }

        public void Deal()
        {
            resultManager.winHandler.HideResult();
            if (player.IsPlacingBet()&&player.canDeal)//这里增加了&&player.canDeal防止玩家按两下可不可以赌
            {
                ResultManager.betsEnabled = false;
                StartCoroutine(InitialDeal());
            }
        }

        //手动修改
        public static int Baccarat_GameMode = 0;//0龙虎斗   1百家乐   2骰宝   3丁半   4花旗骰

        public Image modeImage; // 拖拽你的Image组件到这个变量上
        public Sprite baccaratSprite; // 百家乐的图片
        public Sprite dragonTigerSprite; // 龙虎斗的图片
        public Sprite SicBoSprite; // 骰宝的图片
        public Sprite ChoHanSprite; // 丁半的图片
        public Sprite CrapsSprite; //花旗骰的图片

        [Header("龙虎斗")]
        public GameObject DragonTigerBetSpace;
        public GameObject DragonTigerTable;
        public GameObject DragonTiger_Info;

        [Header("百家乐")]
        public GameObject BaccaratBetSpace;
        public GameObject BaccaratTable;
        public GameObject Baccarat_Info;

        [Header("骰宝")]
        public SicBoGame sicBoGame;
        public GameObject SicBoGameObject;
        public GameObject SicBoBetSpace;
        public GameObject SicBoTable;
        public GameObject SicBo_Info;

        [Header("丁半")]
        public ChoHanGame choHanGame;
        public GameObject ChoHanGameObject;
        public GameObject ChoHanBetSpace;
        public GameObject ChoHanTable;
        public GameObject ChoHan_Info;

        [Header("花旗骰")]
        public GameObject CrapsGameObject;
        public GameObject CrapsBetSpace;
        public GameObject CrapsTable_All, OtherTable_All;
        public GameObject Craps_Info;

        [Header("摄像头")]
        public Animator mainCamera;


        public void DragonTiger_Baccarat() 
        {
           

            Baccarat_GameMode += 1;
            if (Baccarat_GameMode >= 5){ Baccarat_GameMode = 0; }


            switch (Baccarat_GameMode)
            {
                case 0:
                    // 切换到龙虎斗模式
                    modeImage.sprite = dragonTigerSprite;

                    SicBoGameObject.SetActive(false);
                    ChoHanGameObject.SetActive(false);
                    CrapsGameObject.SetActive(false);

                    //教程切换
                    DragonTiger_Info.SetActive(true); Baccarat_Info.SetActive(false); SicBo_Info.SetActive(false); ChoHan_Info.SetActive(false); Craps_Info.SetActive(false);

                    //下注区切换
                    DragonTigerBetSpace.SetActive(true); BaccaratBetSpace.SetActive(false); SicBoBetSpace.SetActive(false); ChoHanBetSpace.SetActive(false); CrapsBetSpace.SetActive(false);
                    //桌子切换
                    OtherTable_All.SetActive(true); CrapsTable_All.SetActive(false);
                    DragonTigerTable.SetActive(true); BaccaratTable.SetActive(false); SicBoTable.SetActive(false); ChoHanTable.SetActive(false);

                    break;
                case 1:
                    // 切换到百家乐模式
                    modeImage.sprite = baccaratSprite;

                    SicBoGameObject.SetActive(false);
                    ChoHanGameObject.SetActive(false);
                    CrapsGameObject.SetActive(false);


                    //教程切换
                    DragonTiger_Info.SetActive(false); Baccarat_Info.SetActive(true); SicBo_Info.SetActive(false); ChoHan_Info.SetActive(false); Craps_Info.SetActive(false);

                    //下注区切换
                    DragonTigerBetSpace.SetActive(false); BaccaratBetSpace.SetActive(true); SicBoBetSpace.SetActive(false); ChoHanBetSpace.SetActive(false); CrapsBetSpace.SetActive(false);
                    //桌子切换
                    OtherTable_All.SetActive(true); CrapsTable_All.SetActive(false);
                    DragonTigerTable.SetActive(false); BaccaratTable.SetActive(true); SicBoTable.SetActive(false); ChoHanTable.SetActive(false);

                    break;
                case 2:
                    // 切换到骰宝模式
                    modeImage.sprite = SicBoSprite;

                    SicBoGameObject.SetActive(true);
                    ChoHanGameObject.SetActive(false);
                    CrapsGameObject.SetActive(false);


                    //教程切换
                    DragonTiger_Info.SetActive(false); Baccarat_Info.SetActive(false); SicBo_Info.SetActive(true); ChoHan_Info.SetActive(false); Craps_Info.SetActive(false);

                    //下注区切换
                    DragonTigerBetSpace.SetActive(false); BaccaratBetSpace.SetActive(false); SicBoBetSpace.SetActive(true); ChoHanBetSpace.SetActive(false); CrapsBetSpace.SetActive(false);
                    //桌子切换
                    OtherTable_All.SetActive(true); CrapsTable_All.SetActive(false);
                    DragonTigerTable.SetActive(false); BaccaratTable.SetActive(false); SicBoTable.SetActive(true); ChoHanTable.SetActive(false);

                    break;
                case 3:
                    // 切换到丁半模式
                    modeImage.sprite = ChoHanSprite;

                    SicBoGameObject.SetActive(false);
                    ChoHanGameObject.SetActive(true);
                    CrapsGameObject.SetActive(false);


                    //教程切换
                    DragonTiger_Info.SetActive(false); Baccarat_Info.SetActive(false); SicBo_Info.SetActive(false); ChoHan_Info.SetActive(true); Craps_Info.SetActive(false);

                    //下注区切换
                    DragonTigerBetSpace.SetActive(false); BaccaratBetSpace.SetActive(false); SicBoBetSpace.SetActive(false); ChoHanBetSpace.SetActive(true); CrapsBetSpace.SetActive(false);
                    //桌子切换
                    OtherTable_All.SetActive(true); CrapsTable_All.SetActive(false);
                    DragonTigerTable.SetActive(false); BaccaratTable.SetActive(false); SicBoTable.SetActive(false); ChoHanTable.SetActive(true);

                    break;
                case 4:
                    // 切换到花旗骰模式
                    modeImage.sprite = CrapsSprite;

                    SicBoGameObject.SetActive(false);
                    ChoHanGameObject.SetActive(false);
                    CrapsGameObject.SetActive(false);
                    //CrapsGameObject.SetActive(true);//花旗骰是直到下注完毕才能投

                    //教程切换
                    DragonTiger_Info.SetActive(false); Baccarat_Info.SetActive(false); SicBo_Info.SetActive(false); ChoHan_Info.SetActive(false); Craps_Info.SetActive(true);

                    //下注区切换
                    DragonTigerBetSpace.SetActive(false); BaccaratBetSpace.SetActive(false); SicBoBetSpace.SetActive(false); ChoHanBetSpace.SetActive(false); CrapsBetSpace.SetActive(true);
                    
                    //桌子切换
                    OtherTable_All.SetActive(false); CrapsTable_All.SetActive(true);
                    DragonTigerTable.SetActive(false); BaccaratTable.SetActive(false); SicBoTable.SetActive(false); ChoHanTable.SetActive(false);

                    //SicBoGameObject.SetActive(true);
                    //ChoHanGameObject.SetActive(false);
                    //CrapsGameObject.SetActive(false);
                    //
                    ////下注区切换
                    //DragonTigerBetSpace.SetActive(false); BaccaratBetSpace.SetActive(false); SicBoBetSpace.SetActive(true); ChoHanBetSpace.SetActive(false); CrapsBetSpace.SetActive(false);
                    ////桌子切换
                    //OtherTable_All.SetActive(true); CrapsTable_All.SetActive(false);
                    //DragonTigerTable.SetActive(false); BaccaratTable.SetActive(false); SicBoTable.SetActive(true); ChoHanTable.SetActive(false);
                    break;
            }

        }

        private IEnumerator InitialDeal()
        {
            OnPlay = true;



            switch (Baccarat_GameMode)
            {
                case 0:
                    // 龙虎斗模式
                    // 龙虎斗模式，只发一张牌
                    DealHand(0);
                    yield return new WaitForSeconds(.3f);
                    DealHand(1);
                    break;
                case 1:
                    // 百家乐模式，发两张牌
                    for (int i = 0; i < 2; i++)
                    {
                        DealHand(0);
                        yield return new WaitForSeconds(.3f);
                        DealHand(1);
                        yield return new WaitForSeconds(.3f);
                    }
                    break;
                case 2:
                    // 骰宝模式,扔三个骰子
                    uiStates.SetEnabled(false);
                    sicBoGame.StartSicBoGame();
                    mainCamera.SetInteger("ChangeView", 1);//骰子需要放大视角
                    break;
                case 3:
                    // 丁半模式,扔两个骰子
                    uiStates.SetEnabled(false);
                    choHanGame.StartChoHanGame();
                    mainCamera.SetInteger("ChangeView", 1);//骰子需要放大视角
                    break;
                case 4:
                    uiStates.SetEnabled(false);
                    //花旗骰，允许投掷
                    CrapsGameObject.SetActive(true);
                    break;
            }

            player.canDeal = false;
        }

        private void DealHand(int id) // 0 Is player, 1 is Banker
        {
            AudioManager_2.SoundPlay(1);//手动SE音频替换


            if (player.playerHands[id].GetNumberOfCards() < 3)
            {
                uiStates.SetEnabled(false);
                CardData card = cardDeck.GetCard();
                DealQueue.DealCard(player.DealCard(card, id));
            }
        }

        void FinishedDealing()
        {
            uiStates.SetEnabled(true);
            uiStates.OnStateChange();

            Debug.Log("DEALING FINISHED");
            CheckIfEnded();
        }

        private bool playerHas3 = false;

        //手动修改
        public void CheckIfEnded()
        {

            if (Baccarat_GameMode == 1)
            {
                // 原有的百家乐结算代码

                if (player.IsPlayerEnded())
                {
                    player.NextHand();
                }// 检查玩家是否结束了本轮，如果是则开始下一手牌
                else if (player.IsBankerEnded())
                {
                    PlayerIsFinished();
                }// 检查庄家是否结束了本轮，如果是则结束当前游戏轮次

                // 玩家规则
                // 如果玩家分数在0到5之间，并且手上只有两张牌，则继续发牌给玩家
                if (player.hand.GetCurrentScore() >= 0 && player.hand.GetCurrentScore() < 6 && player.hand.GetNumberOfCards() == 2)
                {
                    DealHand(0);// 给玩家发一张牌
                    playerHas3 = true;// 标记玩家已经拿到第三张牌
                    player.hand.Stand(); // 玩家站牌，不再拿牌
                }
                // 如果玩家分数达到6分或以上，或者已经有三张牌，玩家站牌
                else if (player.hand.GetCurrentScore() >= 6 || player.hand.GetNumberOfCards() >= 3)
                    player.hand.Stand();

                // 庄家规则
                // 如果玩家的分数是6或7，且庄家分数小于6分并且庄家手上只有两张牌，庄家继续拿牌
                if ((player.hand.GetCurrentScore() == 6 || player.hand.GetCurrentScore() == 7) && player.bankerHand.GetCurrentScore() < 6 && player.bankerHand.GetNumberOfCards() == 2)
                {
                    DealHand(1);// 给庄家发一张牌
                    player.bankerHand.Stand();// 庄家站牌
                }

                // 如果玩家已经有三张牌，并且庄家只有两张牌，且庄家的分数小于7分
                else if (playerHas3 && player.bankerHand.GetNumberOfCards() == 2 && player.hand.GetNumberOfCards() == 3 && player.bankerHand.GetCurrentScore() < 7)
                {
                    int playerThirdCard = player.hand.TakeValueFromCard(2);// 取玩家第三张牌的值
                    int bankerScore = player.bankerHand.GetCurrentScore();// 庄家当前分数

                    // 根据庄家分数和玩家第三张牌的值决定庄家是否拿牌
                    if (bankerScore < 3)
                    {
                        DealHand(1);
                        player.bankerHand.Stand();
                    }
                    else if (bankerScore == 3 && playerThirdCard != 8)
                    {
                        DealHand(1);
                        player.bankerHand.Stand();
                    }
                    else if (bankerScore == 4 && playerThirdCard > 1 && playerThirdCard < 8)
                    {
                        DealHand(1);
                        player.bankerHand.Stand();
                    }
                    else if (bankerScore == 5 && playerThirdCard > 3 && playerThirdCard < 8)
                    {
                        DealHand(1);
                        player.bankerHand.Stand();
                    }
                    else if (bankerScore == 6 && playerThirdCard == 6 || playerThirdCard == 7)
                    {
                        DealHand(1);
                        player.bankerHand.Stand();
                    }
                    else
                    {
                        player.bankerHand.Stand();
                    }
                }
                else
                {
                    // 如果不符合上述条件，庄家站牌
                    player.bankerHand.Stand();
                }

                // 当所有决策都完成后，结束本轮游戏
                PlayerIsFinished();

            }
            else 
            {            

                player.hand.Stand(); // 玩家站牌
                player.bankerHand.Stand(); // 如果不符合上述条件，庄家站牌

                PlayerIsFinished(); // 结束游戏，准备下一轮

                if(Baccarat_GameMode==2|| Baccarat_GameMode == 3 )
                {
                    mainCamera.SetInteger("ChangeView", 0);//骰子需要视角放回
                }

            }//龙虎斗/骰宝/丁半/花旗骰共用


        }

        private void PlayerIsFinished()
        {
            if (player.BothHandsEnded())
            {
                StartCoroutine(EndGame());
            }
        }

        private IEnumerator EndGame()
        {
            while (DealQueue.processing)
            {
                yield return new WaitForSeconds(.5f);
            }

            if (!player.endingGame)
            {
                player.endingGame = true;
                Debug.Log(player.name + " Is getting their awards...");
                resultManager.SetResult(player.hand.GetCurrentScore(), player.bankerHand.GetCurrentScore());//这个地方比较了庄闲分数
                uiStates.gameEnded = true;
                yield return new WaitForSeconds(2);
                ResetTable();
            }
        }

        private void ResetTable()
        {
            player.ResetScore();
            tabletReset.Cleanup();
            BetHistoryManager._Instance.ResetHistory();
            Done();
            ResultManager.betsEnabled = !MenuManager.IsActive;
            Player.totalBet = 0;
        }

        public void Rebet()
        {
            resultManager.winHandler.HideResult();
            uiStates.OnStateChange();
            player.canDeal = true;
        }

        private void HardResetTable()
        {
            OnPlay = false;
            player.ResetTable();
            player.canDeal = true;
        }


        //重新撤销
        public void OnPlayfalse() 
        {
            OnPlay = false;

        }



    }
}
