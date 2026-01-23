using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
//using static UnityEditor.Experimental.GraphView.GraphView;

namespace Blackjack_Game
{
    public enum GameState
    {
        OnIdle,
        OnDealing,
        OnPlay,
        OnRewards
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager _Instance;

        public Player player;
        public Dealer dealer;
        public UIStates _ui;
        public ResultManager results;
        public TableResetManager table;

        [Space] public int deckCount = 6;
        private CardDeck Deck;

        public GameState State = GameState.OnIdle;
        public static bool GameActive = false;

        private bool Trigger_DoubleDownCheck = false;
        private bool Trigger_CheckInsurance = true;
        private bool Trigger_CheckDoubleAce = false;

        private bool EvenMoneyGame = false;
        private bool EvenMoneyAccepted = false;

        private readonly float DEAL_WAIT_TIME = .5f;

        private void Awake()
        {
            if (_Instance == null)
                _Instance = this;

            Deck = new CardDeck(deckCount);
        }

        private void OnEnable()
        {
            DealQueue.OnFinishedDealing += CheckStatus;

        }

        private void OnDisable()
        {
            DealQueue.OnFinishedDealing -= CheckStatus;
        }

        public void OnClickDeal()
        {
            GameActive = true;
            if (State == GameState.OnIdle)
            {
                StartCoroutine(InitialDeal());

                ChipBox.SetInteger("Situation", 1);//开赌后筹码消失

                TreasureBox.SetActive(true);//宝箱出现


                OnPlayerDeal();//玩家下注完毕显示女荷官骚话
            }
            else
            {
                StartCoroutine(PlayerDeal());

            }
        }

        public void OnClickStand()
        {
            TreasureBox.SetActive(false); // ✅ 按下瞬间隐藏
            PeekNextCard.gameObject.SetActive(false);
            PeekSecondNextCard.gameObject.SetActive(false);

            StandPlayerHand();
        }

        public void OnClickDouble()
        {
            TreasureBox.SetActive(false); // ✅ 按下瞬间隐藏
            PeekNextCard.gameObject.SetActive(false);
            PeekSecondNextCard.gameObject.SetActive(false);


            Trigger_DoubleDownCheck = true;
            player.DoubleDown();
            StartCoroutine(PlayerDeal());
        }

        public void OnClickSplit()
        {
            TreasureBox.SetActive(false); // ✅ 按下瞬间隐藏
            PeekNextCard.gameObject.SetActive(false);
            PeekSecondNextCard.gameObject.SetActive(false);


            if (Trigger_CheckDoubleAce && !player.IsAceSplitGame)
            {
                StartCoroutine(SplitAcetStance());
            }
            else
            {
                player.Split();
                StartCoroutine(PlayerDeal());
            }
        }

        private void StandPlayerHand()
        {
            if (player.Stand()) // Was Split Stand ?
            {
                StartCoroutine(PlayerDeal());
                return;
            }

            CheckStatus();
        }

        private IEnumerator SplitAcetStance()
        {
            ChangeGameState(GameState.OnDealing);
            player.IsAceSplitGame = true;
            player.Split();
            yield return new WaitForSeconds(DEAL_WAIT_TIME / 1.6f);
            DealQueue.DealCard(player.AceSplitDeal(Deck.GetCard()));
            ChangeGameState(GameState.OnPlay);
        }

        private IEnumerator InitialDeal()
        {
            ChangeGameState(GameState.OnDealing);
            for (int i = 0; i < 2; i++)
            {
                DealQueue.DealCard(player.DealCard(Deck.GetCard()));
                yield return new WaitForSeconds(DEAL_WAIT_TIME);
                DealQueue.DealCard(dealer.DealCard(Deck.GetCard(), (FlipType)i));
                yield return new WaitForSeconds(DEAL_WAIT_TIME);
            }

            ChangeGameState(GameState.OnPlay);
        }

        private IEnumerator PlayerDeal()
        {
            ChangeGameState(GameState.OnDealing);
            yield return new WaitForSeconds(DEAL_WAIT_TIME);
            DealQueue.DealCard(player.DealCard(Deck.GetCard()));
            ChangeGameState(GameState.OnPlay);
            if (Trigger_DoubleDownCheck)
            {
                Trigger_DoubleDownCheck = false;
                StandPlayerHand();
            }
        }

        private IEnumerator DealereDeal()
        {
            yield return new WaitForSeconds(DEAL_WAIT_TIME);
            DealQueue.DealCard(dealer.DealCard(Deck.GetCard(), FlipType.FlipUp));
        }

        public static void SetDealer(Dealer dealer)
        {
            _Instance.dealer = dealer;
        }

        public static void SetPlayer(Player player)
        {
            _Instance.player = player;
            _Instance.ChangeGameState(GameState.OnIdle);
        }

        public void CheckStatus()
        {
            // ===== Peek 卡隐藏逻辑 =====
            if (DealQueue.CardCount >= 1 )//发第一张牌隐藏
            {
                PeekNextCard.gameObject.SetActive(false);
            }

            if (DealQueue.CardCount >= 2 )//发第二张牌隐藏
            {
                PeekSecondNextCard.gameObject.SetActive(false);
            }

            // =========================





            _ui.PlayingState(player, State == GameState.OnDealing);

            if (DealQueue.processing) return;

            if (!Trigger_CheckDoubleAce && DealQueue.CardCount == 4 && player.HasDoubleAce())
            {
                Trigger_CheckDoubleAce = true;
            }

            if (Trigger_CheckInsurance && DealQueue.CardCount == 4 && dealer.MayHave21())
            {
                Trigger_CheckInsurance = false;

                bool enoughCredit = Player.bet <= BalanceManager.GetBalance();
                if (player.CheckBlackjack())
                {
                    EvenMoneyGame = true;
                    _ui.insurancePanel.SetActive(true);
                    _ui.insuranceText.text = "Would you like to even money?";
                    return;
                }

                if (enoughCredit)
                {
                    _ui.insurancePanel.SetActive(true);
                    _ui.insuranceText.text = "Do you want to insurance?";
                    return;
                }
            }

            if (player.IsEnded() || EvenMoneyGame)
            {
                print("Player Ended");
                if (dealer.HasTurnDownCard())
                {
                    print("Has TurnDown Card");
                    dealer.RevealCard();
                    return;
                }

                if (!dealer.IsEnded(player.IsBusted()) && !player.CheckBlackjack() && !EvenMoneyGame)
                {
                    print("Dealer Turn");
                    StartCoroutine(DealereDeal());
                }
                else
                    EndGame();
            }
            else if (player.HaveCurrentHandEded())
                StandPlayerHand();
        }

        public void EndGame()
        {
            #region
            // ======= 结果判定部分 =======
            bool playerWon = false;
            bool playerPush = false;

            // 主手判定
            if (!EvenMoneyAccepted)
            {
                bool dealerBlackjack = dealer.HasBlackjack();
                bool playerBlackjack = player.HasBlackjack();
                playerWon = player.Score > dealer.Score || dealer.IsBusted() || (playerBlackjack && !dealerBlackjack);
                playerPush = player.Score == dealer.Score || (dealerBlackjack && playerBlackjack);
                playerWon &= !player.hand.IsBust();
            }

            // Split 游戏中也赢算赢
            if (player.IsSplitGame)
            {
                bool splitWin = player.SplitScore > dealer.Score || dealer.IsBusted();
                splitWin &= !player.splitHand.IsBust();
                playerWon |= splitWin;
            }

            // 平局也算赢
            if (playerPush) playerWon = true;

            // ✅ 设置最终结果
            lastResult = playerWon ? PlayerResult.Win : PlayerResult.Lose;
            #endregion


            ChangeView();//一局游戏结束时触发哪些

            print("Game Ended");
            ChangeGameState(GameState.OnRewards);

            if (EvenMoneyGame)
            {
                if (EvenMoneyAccepted)
                {
                    ChipManager.SetWinningStack(StackType.Standard, true);
                    player.hand.ShowOutCome(Outcome.Win);
                }
                else
                {
                    if (dealer.HasBlackjack())
                        player.hand.IsPush(dealer.Score);
                    else
                        EvenMoneyAccepted = false;
                }
            }

            if (!EvenMoneyAccepted)
            {
                bool dealerBlackjack = dealer.HasBlackjack();
                bool playerBlackjack = player.HasBlackjack();
                bool won = player.Score > dealer.Score || dealer.IsBusted() || playerBlackjack && !dealerBlackjack;
                bool push = player.Score == dealer.Score || dealerBlackjack && playerBlackjack;
                won &= !player.hand.IsBust();

                ChipManager.SetWinningStack(StackType.Standard, won);
                ChipManager.SetPushStack(StackType.Standard, push);
                ChipManager.SetWinningStack(StackType.Double, won);
                ChipManager.SetPushStack(StackType.Double, push);
                ChipManager.SetWinningStack(StackType.Insurance, dealerBlackjack);

                if (won)
                {
                    if (playerBlackjack && !push)
                        ChipManager.SetBlackjackStack();
                    else
                        player.hand.ShowOutCome(Outcome.Win);
                }
                else if (!player.hand.IsPush(dealer.Score, dealerBlackjack))
                    player.hand.ShowOutCome(Outcome.NoWin);

                if (player.IsSplitGame)
                {
                    won = player.SplitScore > dealer.Score || dealer.Score > 21;
                    won &= !player.splitHand.IsBust();

                    ChipManager.SetWinningStack(StackType.Split, won);
                    ChipManager.SetWinningStack(StackType.DoubleSplit, won);

                    if (won)
                        player.splitHand.ShowOutCome(Outcome.Win);
                    else if (!player.splitHand.IsPush(dealer.Score, dealerBlackjack))
                        player.splitHand.ShowOutCome(Outcome.NoWin); 
                }
            }

            StartCoroutine(ResetTable());

        }

        public void AcceptInsurance()
        {
            if (player.CheckBlackjack())
                EvenMoneyAccepted = true;
            else
                player.Insurance();

            CheckStatus();
        }

        public void DenyInsurance()
        {
            if (player.HasBlackjack())
                CheckStatus();
        }

        public IEnumerator ResetTable()
        {
            DealQueue.CardCount = 0;
            results.SetResult();
            Trigger_CheckInsurance = true;
            Trigger_CheckDoubleAce = false;
            EvenMoneyGame = false;
            EvenMoneyAccepted = false;
            _ui.ShowChips(false);
            yield return new WaitForSeconds(3);
            player.ResetTable();
            results.HideResult();
            dealer.ResetTable();
            table.Cleanup();
            ChangeGameState(GameState.OnIdle);
            GameActive = false;


        }



        private void ChangeGameState(GameState newState)
        {
            State = newState;
            _ui.ChangeByGameState(State);
        }

        /// <summary>
        /// 游戏结束时镜头转向女荷官
        /// </summary>
        #region

        [Header("摄像头/桌子变淡动画器")]
        public Animator mainCamera;
        public Animator TableAnim;


        public GameObject ChangeViewButon;

        public GameObject TreasureBox;//宝箱

        //玩家输/赢判断
        public enum PlayerResult
        {
            None,
            Win,
            Lose
        }
        private PlayerResult lastResult = PlayerResult.None;

        public void ChangeView()
        {

            HideDialogue();//这个阶段会隐藏之前下筹码的时候的话语
    

            if (SameScore) { dealer.hand.SetScore(player.Score); SameScore = false; }//女荷官强制变成玩家点数
            if (SaveScore && player.Score > 21)
            {
                Debug.Log("【救場：点数超过21，强制削减随机3~5】");
                player.hand.ChangeScore(-Random.Range(3, 6));
            }// 点数超过21，强制削减随机3~5
            SaveScore = false;

            StartCoroutine(ShowRandomGuestsSequentially());//展示客人骚话

            mainCamera.SetInteger("ChangeView", 2);//摄像头朝向女荷官
            //ChangeViewButon.SetActive(true);
            TableAnim.SetInteger("ChangeColor", 1);//桌子强制变淡

            /////////////////////////////////////////////////////VoiceManager.instance.PauseMoanLoop();//暂停娇喘
            #region 显示女荷官垃圾话
            //Invoke("StartDialog", 2f);//显示女荷官垃圾话

            switch (lastResult)
            {
                case PlayerResult.Win:
                    Invoke("OnPlayerWin", 2f); // 显示赢的垃圾话
                    break;
                case PlayerResult.Lose:
                    Invoke("OnPlayerLose", 2f); // 显示输的垃圾话
                    break;
                default:
                    break;
            }
            lastResult = PlayerResult.None; // 重置状态

            #endregion


            player.hand.CheatNumber = 0;//作弊點數清零
            dealer.hand.CheatNumber = 0;//作弊點數清零

            TreasureBox.SetActive(false);//宝箱消失

            PeekCard.gameObject.SetActive(false);//盖牌消失
            PeekNextCard.gameObject.SetActive(false);//下一张卡消失
            PeekSecondNextCard.gameObject.SetActive(false);//下一张卡消失
        }


        public void ChangeViewBack()
        {
            HideAllGuests();//隐藏客人骚话

            //ChangeViewButon.SetActive(false);
            mainCamera.SetInteger("ChangeView", 0);//摄像头转回

            TableAnim.SetInteger("ChangeColor", 0);//桌子强制变回颜色


            // 停止显示女荷官垃圾话对话框
            //HideDialogue();//为了不打断女荷官说话停止，所以暂时不隐藏


            ChipBox.SetInteger("Situation", 0);//筹码出现


        }
        #endregion


        /// <summary>
        /// 随机显示客人骚话
        /// </summary>
        #region
        [Header("客人列表")]
        public List<GameObject> Guests = new List<GameObject>();

        // 隐藏所有游戏对象
        public void HideAllGuests()
        {
            foreach (var guest in Guests)
            {
                guest.SetActive(false);
            }
        }

        // 随机显示一部分游戏对象，每隔一秒显示一个
        public IEnumerator ShowRandomGuestsSequentially()
        {
            HideAllGuests();  // 首先隐藏所有游戏对象

            // 随机决定显示的数量
            int numberToShow = Random.Range(1,4);

            // 随机排序列表
            List<GameObject> shuffledGuests = new List<GameObject>(Guests);
            Shuffle(shuffledGuests);

            // 取出要显示的游戏对象部分
            List<GameObject> guestsToShow = shuffledGuests.GetRange(0, numberToShow);

            // 逐个显示，每个间隔一秒
            foreach (var guest in guestsToShow)
            {
                guest.SetActive(true);
                yield return new WaitForSeconds(1); // 等待一秒
            }
        }

        // 随机排序列表
        private void Shuffle(List<GameObject> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int swapIndex = Random.Range(0, i + 1);
                GameObject temp = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = temp;
            }
        }

        #endregion


        /// <summary>
        /// 随机显示女荷官垃圾话
        /// </summary>
        #region
        [Header("[安托]对局开始垃圾话")]
        public List<GameObject> Anto_StartDialogues = new List<GameObject>();

        [Header("[安托]玩家赢一局垃圾话")]
        public List<GameObject> Anto_PlayerWinDialogues = new List<GameObject>();

        [Header("[安托]玩家输一局垃圾话")]
        public List<GameObject> Anto_PlayerLoseDialogues = new List<GameObject>();

        [Header("[安托]玩家的筹码大")]
        public List<GameObject> Anto_BigDealDialogues = new List<GameObject>();

        [Header("[安托]玩家的筹码小")]
        public List<GameObject> Anto_SmallDealDialogues = new List<GameObject>();


        [Header("[赫蒂]对局开始垃圾话")]
        public List<GameObject> Hetty_StartDialogues = new List<GameObject>();

        [Header("[赫蒂]玩家赢一局垃圾话")]
        public List<GameObject> Hetty_PlayerWinDialogues = new List<GameObject>();

        [Header("[赫蒂]玩家输一局垃圾话")]
        public List<GameObject> Hetty_PlayerLoseDialogues = new List<GameObject>();

        [Header("[赫蒂]玩家的筹码大")]
        public List<GameObject> Hetty_BigDealDialogues = new List<GameObject>();

        [Header("[赫蒂]玩家的筹码小")]
        public List<GameObject> Hetty_SmallDealDialogues = new List<GameObject>();



        [Header("[爱丽丝]对局开始垃圾话")]
        public List<GameObject> Alice_StartDialogues = new List<GameObject>();

        [Header("[爱丽丝]玩家赢一局垃圾话")]
        public List<GameObject> Alice_PlayerWinDialogues = new List<GameObject>();

        [Header("[爱丽丝]玩家输一局垃圾话")]
        public List<GameObject> Alice_PlayerLoseDialogues = new List<GameObject>();

        [Header("[爱丽丝]玩家的筹码大")]
        public List<GameObject> Alice_BigDealDialogues = new List<GameObject>();

        [Header("[爱丽丝]玩家的筹码小")]
        public List<GameObject> Alice_SmallDealDialogues = new List<GameObject>();


        private GameObject currentDisplayedDialogue;


        public enum DialogueEvent
        {
            Start,
            PlayerWin,
            PlayerLose,
            BigDeal,
            SmallDeal
        }


        void ShowDialogue(List<GameObject> dialogueList)
        {
            if (currentDisplayedDialogue != null)
                currentDisplayedDialogue.SetActive(false);

            if (dialogueList == null || dialogueList.Count == 0)
                return;

            int randomIndex = Random.Range(0, dialogueList.Count);
            currentDisplayedDialogue = dialogueList[randomIndex];
            currentDisplayedDialogue.SetActive(true);

            //Invoke("HideDialogue", 3f); // 3秒后隐藏

            ChipBox.SetInteger("Situation", 0);//弹出


        }//显示女荷官垃圾话


        List<GameObject> GetDialogueList(DialogueEvent evt)
        {
            switch (currentDealer)
            {
                case DealerType.Anto:
                    switch (evt)
                    {
                        case DialogueEvent.Start: return Anto_StartDialogues;
                        case DialogueEvent.PlayerWin: return Anto_PlayerWinDialogues;
                        case DialogueEvent.PlayerLose: return Anto_PlayerLoseDialogues;
                        case DialogueEvent.BigDeal: return Anto_BigDealDialogues;
                        case DialogueEvent.SmallDeal: return Anto_SmallDealDialogues;
                    }
                    break;

                case DealerType.Hetty:
                    switch (evt)
                    {
                        case DialogueEvent.Start: return Hetty_StartDialogues;
                        case DialogueEvent.PlayerWin: return Hetty_PlayerWinDialogues;
                        case DialogueEvent.PlayerLose: return Hetty_PlayerLoseDialogues;
                        case DialogueEvent.BigDeal: return Hetty_BigDealDialogues;
                        case DialogueEvent.SmallDeal: return Hetty_SmallDealDialogues;
                    }
                    break;

                case DealerType.Alice:
                    switch (evt)
                    {
                        case DialogueEvent.Start: return Alice_StartDialogues;
                        case DialogueEvent.PlayerWin: return Alice_PlayerWinDialogues;
                        case DialogueEvent.PlayerLose: return Alice_PlayerLoseDialogues;
                        case DialogueEvent.BigDeal: return Alice_BigDealDialogues;
                        case DialogueEvent.SmallDeal: return Alice_SmallDealDialogues;
                    }
                    break;
            }

            return null;
        }







        void HideDialogue()
        {
            if (currentDisplayedDialogue != null)
            {
                currentDisplayedDialogue.SetActive(false);
                currentDisplayedDialogue = null;
            }
        }//隐藏女荷官垃圾话

        void StartMatch()
        {
            // 开局时
            ShowDialogue(GetDialogueList(DialogueEvent.Start));
        }

        void OnPlayerWin()
        {
            // 玩家赢
            ShowDialogue(GetDialogueList(DialogueEvent.PlayerWin));
        }

        void OnPlayerLose()
        {
            // 玩家输
            ShowDialogue(GetDialogueList(DialogueEvent.PlayerLose));
        }

        void OnPlayerDeal() 
        {
            HideDialogue();//防止上面的话还没说完

            bool isBigDeal = Player.bet >= LimitPlace * 0.5f;//判断是大注还是小注


            if (isBigDeal)
            {
                //玩家下大注
                ShowDialogue(GetDialogueList(DialogueEvent.BigDeal));
            }
            else
            {
                //玩家下小注
                ShowDialogue(GetDialogueList(DialogueEvent.SmallDeal));
            }
            
        }

        #endregion


        /// <summary>
        /// 物品栏和筹码栏
        /// </summary>
        #region
        [Header("筹码栏")]
        public Animator ChipBox;
        bool ChipBoxisClosed = false;
        public void ChipBoxTrigger()
        {
            AudioManager.SoundPlay(1);

            ChipBoxisClosed = !ChipBoxisClosed;

            if (ChipBoxisClosed)
            {
                ChipBox.SetInteger("Situation", 1);
            }
            else
            {
                ChipBox.SetInteger("Situation", 0);
            }
        }
        #endregion


        /// <summary>
        /// 使用物品
        /// </summary>
        #region
        [Header("使用物品")]
        public GameObject USE_Button;
        public GameObject Item_Panel;
        int CurrentItem;
        public List<GameObject> List_Item_Light; // 使用List来存储多个物品选中
        public List<GameObject> List_Item_Introduce; // 使用List来存储多个物品介绍

        public ItemManager itemManager;//刷新物品
       

        public void Item_Setting(int Item_Number) 
        {
            CurrentItem = Item_Number;

            foreach (GameObject Light in List_Item_Light) 
            {
                Light.SetActive(false);
            }
            foreach (GameObject Introduce in List_Item_Introduce)
            {
                Introduce.SetActive(false);
            }

            List_Item_Light[Item_Number].SetActive(true);
            List_Item_Introduce[Item_Number].SetActive(true);

            AudioManager.SoundPlay(0);
            if(UIManager.GameOver == false)
            {
                USE_Button.SetActive(true);
            }//如果赌局结束，物品栏不会跳出使用键
            
        }


        public void _UseItem() 
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            int currentCount;  //削减物品数量


            switch (CurrentItem) 
            {
                case 0:
                    //紫色心情
                    Item_ChangeFemaleDealerScore();//修改女荷官点数
                    currentCount = data.Item_1;
                    currentCount--;
                    data.Item_1 = currentCount;
                    break;
                case 1:
                    //占卜水晶
                    StartCoroutine(Item_ViewNextCard());
                    //Item_ViewNextCard();//看牌堆下一张卡
                    currentCount = data.Item_2;
                    currentCount--;
                    data.Item_2 = currentCount;
                    break;
                case 2:
                    //均衡法杖
                    Item_SameScore();//强制平局
                    currentCount = data.Item_3;
                    currentCount--;
                    data.Item_3 = currentCount;
                    break;
                case 3:
                    //魔眼石
                    StartCoroutine(Item_ViewCard());
                    //Item_ViewCard();//看女荷官的盖牌
                    currentCount = data.Item_4;
                    currentCount--;
                    data.Item_4 = currentCount;
                    break;
                case 4:
                    //酒瓶
                    Item_RandomDoubleScore();//双方随机一方双倍
                    currentCount = data.Item_5;
                    currentCount--;
                    data.Item_5 = currentCount;
                    break;
                case 5:
                    //藏宝图残片
                    Item_SaveScore();//点数超过21，强制削减随机3~5
                    currentCount = data.Item_6;
                    currentCount--;
                    data.Item_6 = currentCount;
                    break;
                case 6:
                    //幸运币
                    Item_ChangePlayerScore();//修改你的点数
                    currentCount = data.Item_7;
                    currentCount--;
                    data.Item_7 = currentCount;
                    break;
                case 7:
                    //透视药水
                    StartCoroutine(Item_ViewSecondNextCard());//看牌堆下下张卡
                    currentCount = data.Item_8;
                    currentCount--;
                    data.Item_8 = currentCount;
                    break;
            }

            Item_Panel.SetActive(false);


            // 写回存档

            SaveManager.SaveGame(data);         

            itemManager.UpdateInventoryUI();

            //隐藏使用按钮
            USE_Button.SetActive(false);

        }



        public IEnumerator Item_ViewCard()
        {
            dealer.ConcealCard();

            CardData nextCard = dealer.hand.GetSecondCard().cardData;

            //等待再结算
            yield return new WaitForSeconds(1.5f);


            PeekCard.gameObject.SetActive(true);
            PeekCard.mesh = nextCard.GetMesh();

        }//看女荷官的盖牌

        [Header("展示牌")]
        public MeshFilter ShowCard;

        public MeshFilter PeekCard;
        public MeshFilter PeekNextCard;
        public MeshFilter PeekSecondNextCard;

        public IEnumerator Item_ViewNextCard()
        {
            CardData nextCard = Deck.PeekCard();  // 检视但不抽取下一张牌
            ShowCard.gameObject.SetActive(true);
            ShowCard.mesh = nextCard.GetMesh();


            //等待再结算
            yield return new WaitForSeconds(2.5f);


            PeekNextCard.gameObject.SetActive(true);
            PeekNextCard.mesh = nextCard.GetMesh();

           
        }//看你的下一张卡


        public IEnumerator Item_ViewSecondNextCard()
        {
            CardData nextCard = Deck.PeekSecondNextCard();  // 检视但不抽取下下张牌  
            ShowCard.gameObject.SetActive(true);
            ShowCard.mesh = nextCard.GetMesh();


            //等待再结算
            yield return new WaitForSeconds(2.5f);


            PeekSecondNextCard.gameObject.SetActive(true);
            PeekSecondNextCard.mesh = nextCard.GetMesh();


        }//看你的下一张卡



        public void Item_ChangePlayerScore()
        {
            if (Random.Range(0, 2) == 0)
            {
                player.hand.ChangeScore(-1);
            }
            else
            {
                player.hand.ChangeScore(1);
            }
        }//修改你的点数

        public void Item_ChangeFemaleDealerScore()
        {
            if (Random.Range(0, 2) == 0)
            {
                dealer.hand.ChangeScore(-1);
            }
            else
            {
                dealer.hand.ChangeScore(1);
            }


        }//修改女荷官点数

        public void Item_RandomDoubleScore()
        {

            if (Random.Range(0, 2) == 0)
            {
                player.hand.ChangeScore(player.Score);
            }
            else
            {
                dealer.hand.ChangeScore(dealer.Score);
            }

        }//双方随机一方双倍

        bool SameScore = false;
        public void Item_SameScore()
        {
            SameScore = true;
        }//强制平局

        bool SaveScore = false;
        public void Item_SaveScore()
        {
            SaveScore = true;
        }//点数超过21，强制削减随机3~5

        #endregion



        /// <summary>
        /// 关卡与女荷官生命值/女荷官动画
        /// </summary>
        #region
        [Header("女荷官生命值")]
        public Text healthText;
        public Image healthFillImage;
        public float maxHealth = 1000f;
        private float currentHealth;

        public Text Limit;//本局赌注上限
        int LimitPlace;//本局赌注上限

        public VoiceManager voiceManager;//一旦超过这个阈值就能触发安托的呻吟

        int Progress;//女荷官进度


        public enum DealerType
        {
            Anto,
            Hetty,
            Alice
        }

        private DealerType currentDealer;//女荷官类型



        void Start()
        {

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            //int Story_Anto = data.antoProgress;
            //
            //Debug.Log("目前储存的关卡进度_安托" + Story_Anto);
            //if (Story_Anto <= 0)
            //{
            //    data.antoProgress = 1;
            //    SaveManager.SaveGame(data);
            //}


            //根据存档来显示对应的动画器
            switch (GameFlowData.nextAVGId)
            {
                default:
                case "VSAnto":
                    dealerAnimator = antoAnimator;
                    Progress = data.antoProgress;
                    currentDealer = DealerType.Anto;
                    break;

                case "VSHetty":
                    dealerAnimator = hettyAnimator;
                    Progress = data.hettyProgress;
                    currentDealer = DealerType.Hetty;
                    break;

                case "VSAlice":
                    dealerAnimator = aliceAnimator;
                    Progress = data.aliceProgress;
                    currentDealer = DealerType.Alice;
                    break;


            }

            dealerAnimator.gameObject.SetActive(true);
            


            //检测对应女荷官等级
            LimitPlace = Progress * 200;
            Limit.text = LimitPlace.ToString();//本局赌注上限

            maxHealth = Progress * 1000;

            currentHealth = maxHealth;
            UpdateFill();


            Invoke("StartMatch", 1f); // 开局时显示女荷官垃圾话
            //Invoke("StartDialog", 1f);//游戏开始时显示女荷官垃圾话

            TableAnim.SetInteger("ChangeColor", 1);//桌子强制变淡
        }

        public static void ChangeHealth(float amount)
        {

            _Instance.currentHealth += amount;
            _Instance.currentHealth = Mathf.Clamp(_Instance.currentHealth, 0, _Instance.maxHealth);
            _Instance.UpdateFill();


            // —— 新增：根据 currentHealth 计算脱衣阶段 ——
            float segment = _Instance.maxHealth / 8f;
            // 计算已脱几段：当 currentHealth 掉到小于 (maxHealth - segment * n) 时，就脱到第 n+1 段
            int newSituation = Mathf.FloorToInt((_Instance.maxHealth - _Instance.currentHealth) / segment) + 1;
            newSituation = Mathf.Clamp(newSituation, 1, 8);

            // 延迟调用处理逻辑
            _Instance.StartCoroutine(_Instance.HandleSituationTransition(newSituation));

            // —— 结束 新增 ——

            if (_Instance.currentHealth <= 0)
            {
                _Instance.Invoke("ShowWINButton", 1f);
            }

            //当女荷官生命值低于过半开始呻吟
            if (_Instance.currentHealth <= _Instance.maxHealth){ _Instance.voiceManager.CanScream = true; }

        }

        void UpdateFill()
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
            healthText.text = $"{currentHealth} / {maxHealth}";
        }


        [Header("胜利图标")]
        public UIManager UIManager;
        public GameObject WinButton;

        public GameObject Icon_Win;
        public GameObject Icon_NoWin;
        public GameObject Icon_Push;
        public GameObject Icon_Bust;
        void ShowWINButton()
        {
            WinButton.SetActive(true);
            int currentLV = PlayerPrefs.GetInt("Story_Anto");
            PlayerPrefs.SetInt("Story_Anto", currentLV += 1);
        }//显示胜利画面
        public void ReLoadScene()
        {
            UIManager.LoadingScene_BJ_Mobile();
        }

        [Header("安托动画")]
        [SerializeField] Animator dealerAnimator;
        public Animator antoAnimator, hettyAnimator, aliceAnimator;

        private int currentSituation = 1; // 当前状态阶段（1~8）
        private int maxSituation = 8;

        private bool isTransitioning = false;

        public void ApplyDamage(int newSituation)
        {
            if (newSituation <= currentSituation || isTransitioning || newSituation > maxSituation)
                return;

            StartCoroutine(PlayUndressSequence(currentSituation, newSituation));
            currentSituation = newSituation;
        }
        private IEnumerator HandleSituationTransition(int newSituation)
        {
            yield return new WaitForSeconds(1f);

            if (newSituation > _Instance.currentSituation)
            {
                _Instance.ApplyDamage(newSituation);
            }
            else
            {
                _Instance.PlayLose();
            }
        }
        private IEnumerator PlayUndressSequence(int from, int to)
        {
            isTransitioning = true;

            #region 连续播放脱衣动画

            //for (int i = from; i < to; i++)
            //{
            //    antorAnimator.SetInteger("Undress", i);
            //
            //    // 等待状态机进入 Undress_i，再播放完
            //    yield return new WaitUntil(() => IsPlayingState($"Situation_{i}_Undress"));
            //    yield return new WaitUntil(() => !IsPlayingState($"Situation_{i}_Undress"));
            //}

            #endregion


            #region 直接跳到当前生命值阶段脱衣动画
            // 目标前一档，例如 to=4，则 prev=3；保证在 [1, maxSituation] 内
            int prev = Mathf.Clamp(to - 1, 1, maxSituation);

            // 1) 瞬间切到 "Situation_prev_Idle"
            //    用 CrossFade 或 Play 都行；CrossFade 0 秒可避免受 Exit Time 干扰
            dealerAnimator.CrossFade($"Situation_{prev}_Idle", 0f, 0, 0f);
            yield return null; // 等一帧让 Animator 应用

            // 2) 只播放这一档的脱衣动画："prev -> to"
            dealerAnimator.SetInteger("Undress", prev);

            // 等状态机真正进入 Undress_prev 并播放完
            yield return new WaitUntil(() => IsPlayingState($"Situation_{prev}_Undress"));
            yield return new WaitUntil(() => !IsPlayingState($"Situation_{prev}_Undress"));
            #endregion



            dealerAnimator.SetInteger("Undress", 0); // 重置参数防止连播
            isTransitioning = false;
        }

        private bool IsPlayingState(string stateName)
        {
            AnimatorStateInfo info = dealerAnimator.GetCurrentAnimatorStateInfo(0);
            return info.IsName(stateName);
        }

        public void InvokeWin() 
        {
            Invoke("PlayWin", 1f);//女荷官获胜后嘲讽
        }

        public void PlayWin()
        {


            dealerAnimator.SetTrigger("Win");
        }

        public void PlayLose()//这个阶段会隐藏之前下筹码的时候的话语
        {
 

            dealerAnimator.SetTrigger("Lose");
        }


        #endregion


        /// <summary>
        /// 设置好的快捷键触发
        /// </summary>
        #region

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) && _ui.dealButton.interactable == true)
            {
                OnClickDeal();//下注完成或者拿牌
            }
            if (Input.GetKeyDown(KeyCode.S) && _ui.standButton.interactable == true)
            {
                OnClickStand();//站牌
            }
            if (Input.GetKeyDown(KeyCode.D) && _ui.doubleButton.interactable == true)
            {
                OnClickDouble();//双倍
            }
            if (Input.GetKeyDown(KeyCode.F) && _ui.splitButton.interactable == true)
            {
                OnClickSplit();//分牌
            }

            if (Input.GetKeyDown(KeyCode.Z) && _ui.undoButton.interactable == true)
            {
                BetHistoryManager._Instance.Undo();
                //取消
            }

            if (Input.GetKeyDown(KeyCode.X) && _ui.clearButton.interactable == true)
            {
                BetHistoryManager._Instance.ClearHistory();
                //清除
            }
        }
        #endregion

    }
}