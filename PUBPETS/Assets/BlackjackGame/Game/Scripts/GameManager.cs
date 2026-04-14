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

            // 一旦正式开始赌局，隐藏暂停和离开
            SetMatchUIVisible(false);



            if (State == GameState.OnIdle)
            {
                StartCoroutine(InitialDeal());

                ChipBox.SetInteger("Situation", 1);//开赌后筹码消失

                OnPlayerDeal();//玩家下注完毕显示女荷官骚话
            }
            else
            {

                if (currentDealer == DealerType.Hetty)
                {
                    Dealer_TryBurnTopCard();//女荷官使用匕首
                }



                if (PeekNextCard.gameObject.activeInHierarchy)
                {
                    PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
                    Invoke(nameof(CheckAgain_Item_ViewNextCard), 1f);
                }
                if (PeekSecondNextCard.gameObject.activeInHierarchy)
                {
                    PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
                    Invoke(nameof(CheckAgain_Item_ViewSecondNextCard), 1f);
                }


                StartCoroutine(PlayerDeal());

                HideTreasureAndItemUI();      // ✅ 按下瞬间强制关

                isHit = true;//只有要牌才能再让宝箱出现


               


            }
        }

        public void OnClickStand()
        {

            PeekCard.gameObject.SetActive(false);//站牌的时候，盖牌就必须消失

            if (PeekNextCard.gameObject.activeInHierarchy)
            {
                PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            }
            if (PeekSecondNextCard.gameObject.activeInHierarchy)
            {
                PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            }

            HideTreasureAndItemUI();      // ✅ 按下瞬间强制关



            StandPlayerHand();

            if (currentDealer == DealerType.Alice && Random.Range(0, 1) == 0)
            {
                Dealer_AliceMilk();//女荷官使用回复生命
            }

          
          
        }

        public void OnClickDouble()
        {
            if (PeekNextCard.gameObject.activeInHierarchy)
            {
                PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            }
            if (PeekSecondNextCard.gameObject.activeInHierarchy)
            {
                PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            }


            HideTreasureAndItemUI();      // ✅ 按下瞬间强制关




            Trigger_DoubleDownCheck = true;
            player.DoubleDown();
            StartCoroutine(PlayerDeal());
        }

        public void OnClickSplit()
        {

            if (PeekNextCard.gameObject.activeInHierarchy)
            {
                PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            }
            if (PeekSecondNextCard.gameObject.activeInHierarchy)
            {
                PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            }



            HideTreasureAndItemUI();      // ✅ 按下瞬间强制关



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

            // ✅ 发牌结束后再决定要不要开宝箱
            ShowTreasureBoxIfAllowed();
        }

        [Header("Treasure / Item UI")]
        public GameObject TreasureBox;//宝箱按钮
        public GameObject ItemPanel; // 物品栏面板（你实际名字替换）

        private void HideTreasureAndItemUI()
        {
            //TreasureBox.SetActive(false);
            TreasureBox.GetComponent<Animator>().SetTrigger("Out");

            ItemPanel.SetActive(false);
        }//统一隐藏宝箱

        private void ShowTreasureBoxIfAllowed()
        {
            if (State != GameState.OnPlay) return;
            if (player.Score == 21) return; // 或 player.HasBlackjack()

            if (TreasureBox != null) TreasureBox.SetActive(true);
        }//统一出现宝箱（仅有发牌结束/Hit结束两种）



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
            //if (DealQueue.CardCount >= 1)//发第一张牌隐藏
            //{
            //    PeekNextCard.gameObject.SetActive(false);
            //}
            //
            //if (DealQueue.CardCount >= 2)//发第二张牌隐藏
            //{
            //    PeekSecondNextCard.gameObject.SetActive(false);
            //}
            //if (PeekNextCard.gameObject.activeInHierarchy)
            //{
            //    PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            //}
            //if (PeekSecondNextCard.gameObject.activeInHierarchy)
            //{
            //    PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            //
            //}
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

            // 赌局结束，恢复暂停和离开
            SetMatchUIVisible(true);
        }



        private void ChangeGameState(GameState newState)
        {
            State = newState;
            _ui.ChangeByGameState(State);


            // ✅ 只要不是可操作阶段，一律关宝箱+物品栏
            if (State == GameState.OnDealing || State == GameState.OnRewards || State == GameState.OnIdle)
            {
                HideTreasureAndItemUI();
                return;
            }

            // ✅ 回到可操作阶段才允许显示宝箱
            if (State == GameState.OnPlay && isHit)
            {
                Invoke(nameof(ShowTreasureBoxIfAllowed), 1f);
                isHit = false;
            }


        }


        //只有Hit可以再次显示宝箱
        public bool isHit = false;



        public void CheckAgain_Item_ViewNextCard()
        {
            StartCoroutine(Item_ViewNextCard());//看牌堆下张卡
        }

        public void CheckAgain_Item_ViewSecondNextCard()
        {
            StartCoroutine(Item_ViewSecondNextCard());//看牌堆下下张卡
        }



        /// <summary>
        /// 游戏结束时镜头转向女荷官
        /// </summary>
        #region

        [Header("摄像头/桌子变淡动画器")]
        public Animator mainCamera;
        public Animator TableAnim;


        public GameObject ChangeViewButon;


        //玩家输/赢判断
        public enum PlayerResult
        {
            None,
            Win,
            Lose
        }
        private PlayerResult lastResult = PlayerResult.None;

        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ChangeView()
        {

            HideDialogue();//这个阶段会隐藏之前下筹码的时候的话语


            #region   在开牌的瞬间触发

            //均衡徽章
            if (SameScore)
            {
                Debug.Log("玩家使用【强制平局：玩家点数和女荷官相同】");

                player.hand.SetScore(dealer.Score);

                SameScore = false;
                Sign_SameScore.SetActive(false);

            }//女荷官强制变成玩家点数


            //藏宝图残片
            if (SaveScore && player.Score > 21)
            {
                Debug.Log("【救場：点数超过21，强制削减随机3~5】");


                player.hand.ChangeScore(-SaveNumber);



            }// 玩家点数超过21，强制削减随机3~5
            SaveScore = false;
            Sign_SaveScore.SetActive(false);
            SaveNumber = 0;//强制清零


            //我在Result里立即重置了
            //Invoke(nameof(ResetDoubleReward), 1f);



            if (currentDealer == DealerType.Anto && Random.Range(0,2)==0)
            {

                Dealer_IncreaseFemaleDealerScore_3();//女荷官使用绿色心情

                Dealer_IncreaseFemaleDealerScore_1();//女荷官使用幸运币

                Dealer_DecreaseFemaleDealerScore_3();//女荷官使用紫色心情

                Dealer_DecreaseFemaleDealerScore_1();//女荷官使用厄运币
            }


            if (currentDealer == DealerType.Hetty)
            {

                bool isBigDeal = Player.bet >= LimitPlace * 0.5f;//判断是大注还是小注

                if (isBigDeal)
                {
                    Dealer_SameScore();//女荷官使用均衡徽章


                    Dealer_SaveScore();//女荷官使用藏宝图残片
                }
              

            }







            //if (currentDealer == DealerType.Alice)
            //{
            //    Invoke(nameof(Dealer_AliceMilk),2f);//女荷官使用回复生命
            //}          

            #endregion



            StartCoroutine(ShowRandomGuestsSequentially());//展示客人骚话

            mainCamera.SetInteger("ChangeView", 2);//摄像头朝向女荷官
            //ChangeViewButon.SetActive(true);
            TableAnim.SetInteger("ChangeColor", 1);//桌子强制变淡


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

            swampRate = 1f;//爱丽丝的沼泽吞噬盈利恢复原状
            Sign_SwampRate.SetActive(false);


            //TreasureBox.SetActive(false);//宝箱消失
            TreasureBox.GetComponent<Animator>().SetTrigger("Out");

            PeekCard.gameObject.SetActive(false);//盖牌消失
            PeekNextCard.gameObject.SetActive(false);//下一张卡消失
            PeekSecondNextCard.gameObject.SetActive(false);//下一张卡消失
            //PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
            //PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失

            //使用欢呼声
            switch (Random.Range(0, 3))
            {
                case 0:
                    AudioManager_2.SoundPlay(9);//手动SE音频替换
                    break;

                case 1:
                    AudioManager_2.SoundPlay(10);//手动SE音频替换
                    break;
                case 2:
                    AudioManager_2.SoundPlay(11);//手动SE音频替换
                    break;
            }



            turns++;

        }//一局的结算阶段


        public void ChangeViewBack()
        {
            HideAllGuests();//隐藏客人骚话

            //ChangeViewButon.SetActive(false);
            mainCamera.SetInteger("ChangeView", 0);//摄像头转回

            TableAnim.SetInteger("ChangeColor", 0);//桌子强制变回颜色


            // 停止显示女荷官垃圾话对话框
            //HideDialogue();//为了不打断女荷官说话停止，所以暂时不隐藏


            ChipBox.SetInteger("Situation", 0);//筹码出现


            if (Progress > 6)//123456关女荷官不会恢复爱心
            {
                //不同的女荷官反击时恢复的爱心不同
                switch (currentDealer)
                {
                    case DealerType.Anto:
                        bool isBigDeal = Player.bet >= LimitPlace * 0.5f;//判断是大注还是小注

                        if (isBigDeal)
                        {
                            if (Progress == 7 && Progress == 8 && Progress == 9)
                            {
                                if (Random.Range(0, 2) == 0)
                                {
                                    AddCounter(1);//在7，8，9关的时候安托看见大注随机恢复1颗
                                }
                            }

                            if (Progress >= 10)
                            {

                                if (Random.Range(0, 2) == 0)
                                {
                                    AddCounter(2);//在10关的时候安托看见大注固定恢复2颗

                                }

                            }

                        }
                        break;

                    case DealerType.Hetty:


                        if (currentHealth < maxHealth / 2)
                        {
                            if(Progress == 7&& Progress == 8&& Progress == 9 && Progress == 10)
                            {
                                if (Random.Range(0, 2) == 0)
                                {
                                    AddCounter(1);//在7，8，9，10关的时候赫蒂半血以下随机恢复1颗
                                }
                            }

                        }

                        if (Progress >= 4)
                        {
                            if (currentCounter <= 0)
                            {
                                AddCounter(1);//赫蒂无爱心情况下固定恢复1颗
                            }

                        }

                    

                        break;

                    case DealerType.Alice:

                        if (Progress >= 10)
                        {
                            if (Random.Range(0, 2) == 0)
                            {
                                AddCounter(1);//在10关的时候爱丽丝半血以下随机恢复1颗
                            }

                        }


                        break;
                }



            } //1~3关女荷官不会回复反制槽


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
            int numberToShow = Random.Range(1, 4);

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


        [Header("[安托发情]玩家下注")]
        public List<GameObject> Anto_LewdSound_DealDialogues = new List<GameObject>();

        [Header("[安托发情]玩家赢一局垃圾话")]
        public List<GameObject> Anto_LewdSound_PlayerWinDialogues = new List<GameObject>();

        [Header("[安托发情]玩家输一局垃圾话")]
        public List<GameObject> Anto_LewdSound_PlayerLoseDialogues = new List<GameObject>();



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


        [Header("[赫蒂发情]玩家下注")]
        public List<GameObject> Hetty_LewdSound_DealDialogues = new List<GameObject>();

        [Header("[赫蒂发情]玩家赢一局垃圾话")]
        public List<GameObject> Hetty_LewdSound_PlayerWinDialogues = new List<GameObject>();

        [Header("[赫蒂发情]玩家输一局垃圾话")]
        public List<GameObject> Hetty_LewdSound_PlayerLoseDialogues = new List<GameObject>();




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


        [Header("[爱丽丝发情]玩家下注")]
        public List<GameObject> Alice_LewdSound_DealDialogues = new List<GameObject>();

        [Header("[爱丽丝发情]玩家赢一局垃圾话")]
        public List<GameObject> Alice_LewdSound_PlayerWinDialogues = new List<GameObject>();

        [Header("[爱丽丝发情]玩家输一局垃圾话")]
        public List<GameObject> Alice_LewdSound_PlayerLoseDialogues = new List<GameObject>();


        private GameObject currentDisplayedDialogue;


        public enum DialogueEvent
        {
            Start,
            PlayerWin,
            PlayerLose,
            BigDeal,
            SmallDeal,
            LewdSoundDeal,
            LewdSoundPlayerWin,
            LewdSoundPlayerLose
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

            ChipBox.SetInteger("Situation", 0);//弹出筹码


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
                        case DialogueEvent.LewdSoundDeal: return Anto_LewdSound_DealDialogues;
                        case DialogueEvent.LewdSoundPlayerWin: return Anto_LewdSound_PlayerWinDialogues;
                        case DialogueEvent.LewdSoundPlayerLose: return Anto_LewdSound_PlayerLoseDialogues;
                    }
                    CheckoutScreen_Dealer.sprite = CheckoutScreen_Anto;//结算画面女荷官
                    break;

                case DealerType.Hetty:
                    switch (evt)
                    {
                        case DialogueEvent.Start: return Hetty_StartDialogues;
                        case DialogueEvent.PlayerWin: return Hetty_PlayerWinDialogues;
                        case DialogueEvent.PlayerLose: return Hetty_PlayerLoseDialogues;
                        case DialogueEvent.BigDeal: return Hetty_BigDealDialogues;
                        case DialogueEvent.SmallDeal: return Hetty_SmallDealDialogues;
                        case DialogueEvent.LewdSoundDeal: return Hetty_LewdSound_DealDialogues;
                        case DialogueEvent.LewdSoundPlayerWin: return Hetty_LewdSound_PlayerWinDialogues;
                        case DialogueEvent.LewdSoundPlayerLose: return Hetty_LewdSound_PlayerLoseDialogues;
                    }
                    CheckoutScreen_Dealer.sprite = CheckoutScreen_Hetty;//结算画面女荷官
                    break;

                case DealerType.Alice:
                    switch (evt)
                    {
                        case DialogueEvent.Start: return Alice_StartDialogues;
                        case DialogueEvent.PlayerWin: return Alice_PlayerWinDialogues;
                        case DialogueEvent.PlayerLose: return Alice_PlayerLoseDialogues;
                        case DialogueEvent.BigDeal: return Alice_BigDealDialogues;
                        case DialogueEvent.SmallDeal: return Alice_SmallDealDialogues;
                        case DialogueEvent.LewdSoundDeal: return Alice_LewdSound_DealDialogues;
                        case DialogueEvent.LewdSoundPlayerWin: return Alice_LewdSound_PlayerWinDialogues;
                        case DialogueEvent.LewdSoundPlayerLose: return Alice_LewdSound_PlayerLoseDialogues;
                    }
                    CheckoutScreen_Dealer.sprite = CheckoutScreen_Alice;//结算画面女荷官
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
            if (dealerAnimator.GetInteger("Undress") >= 4) //女荷官是否进入发情状态
            {
                ShowDialogue(GetDialogueList(DialogueEvent.LewdSoundPlayerWin));
            }
            else
            {
                ShowDialogue(GetDialogueList(DialogueEvent.PlayerWin));
            }

        }

        void OnPlayerLose()
        {
            // 玩家输
            if (dealerAnimator.GetInteger("Undress") >= 4) //女荷官是否进入发情状态
            {
                ShowDialogue(GetDialogueList(DialogueEvent.LewdSoundPlayerLose));
            }
            else
            {
                ShowDialogue(GetDialogueList(DialogueEvent.PlayerLose));
            }

        }

        void OnPlayerDeal()
        {
            HideDialogue();//防止上面的话还没说完


            if (dealerAnimator.GetInteger("Undress") >= 4) //女荷官是否进入发情状态
            {
                ShowDialogue(GetDialogueList(DialogueEvent.LewdSoundDeal));
            }
            else
            {
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




        }

        #endregion


        /// <summary>
        /// 筹码栏
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

        public Animator LimitNumber;


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
            if (UIManager.GameOver == false)
            {
                USE_Button.SetActive(true);
            }//如果赌局结束，物品栏不会跳出使用键

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public enum DealerReaction
        {
            None,
            Interrupt,  // 赫蒂
            Steal       // 安托
        }
        DealerReaction GetDealerReaction(int itemId)
        {
            if (!HasCounter())
                return DealerReaction.None;

            switch (currentDealer)
            {
                case DealerType.Hetty:
                    if (Random.Range(0, 3) == 0&& HasCounter())
                        return DealerReaction.Interrupt;
                    break;

                case DealerType.Anto:
                    if (Random.Range(0, 3) == 0 && HasCounter())
                        return DealerReaction.Steal;
                    break;

                case DealerType.Alice:
                    // 爱丽丝永远不干涉物品
                    return DealerReaction.None;
            }

            return DealerReaction.None;
        }

        public void _UseItem()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            int currentCount;  //削减物品数量


            DealerReaction reaction = GetDealerReaction(CurrentItem);

            if (reaction == DealerReaction.Interrupt)
            {
                ConsumeOne();//消耗一颗爱心
                Interrupt_Item(CurrentItem);//女荷官反制


                //该物品依旧被消耗
                switch (CurrentItem)
                {
                    case 0:
                        //紫色心情
                        currentCount = data.Item_1;
                        currentCount--;
                        data.Item_1 = currentCount;
                        break;
                    case 1:
                        //占卜水晶
                        currentCount = data.Item_2;
                        currentCount--;
                        data.Item_2 = currentCount;
                        break;
                    case 2:
                        //均衡徽章
                        currentCount = data.Item_3;
                        currentCount--;
                        data.Item_3 = currentCount;
                        break;
                    case 3:
                        //魔眼石
                        currentCount = data.Item_4;
                        currentCount--;
                        data.Item_4 = currentCount;
                        break;
                    case 4:
                        //酒瓶
                        currentCount = data.Item_5;
                        currentCount--;
                        data.Item_5 = currentCount;
                        break;
                    case 5:
                        //藏宝图残片
                        currentCount = data.Item_6;
                        currentCount--;
                        data.Item_6 = currentCount;
                        break;
                    case 6:
                        //幸运币
                        currentCount = data.Item_7;
                        currentCount--;
                        data.Item_7 = currentCount;
                        break;
                    case 7:
                        //透视药水
                        currentCount = data.Item_8;
                        currentCount--;
                        data.Item_8 = currentCount;
                        break;



                    case 8:
                        //绿色心情
                        currentCount = data.Item_9;
                        currentCount--;
                        data.Item_9 = currentCount;
                        break;

                    case 9:
                        //匕首
                        currentCount = data.Item_10;
                        currentCount--;
                        data.Item_10 = currentCount;
                        break;

                    case 10:
                        //黑棋子
                        currentCount = data.Item_11;
                        currentCount--;
                        data.Item_11 = currentCount;
                        break;

                    case 11:
                        //魔眼药水
                        currentCount = data.Item_12;
                        currentCount--;
                        data.Item_12 = currentCount;
                        break;

                    case 12:
                        //空瓶
                        currentCount = data.Item_13;
                        currentCount--;
                        data.Item_13 = currentCount;
                        break;

                    case 13:
                        //白棋子
                        currentCount = data.Item_14;
                        currentCount--;
                        data.Item_14 = currentCount;
                        break;

                    case 14:
                        //厄运币
                        currentCount = data.Item_15;
                        currentCount--;
                        data.Item_15 = currentCount;
                        break;

                    case 15:
                        //皇室徽章
                        currentCount = data.Item_16;
                        currentCount--;
                        data.Item_16 = currentCount;
                        break;
                }



            }
            else if (reaction == DealerReaction.Steal)
            {
              

                //盗窃物品并使用
                switch (CurrentItem)
                {
                    case 0:
                                    
                        Dealer_StealItems_0(); //玩家使用紫色心情，安托偷过来给自己-3
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）        


                        currentCount = data.Item_1;
                        currentCount--;
                        data.Item_1 = currentCount;

                        break;
                    case 1:
                        //占卜水晶
                        StartCoroutine(Item_ViewNextCard());//看牌堆下一张卡
                        StartCoroutine(Item_ViewCard());//看女荷官的盖牌   
                        StartCoroutine(Item_ViewSecondNextCard());//看牌堆下下张卡

                        switch (PlayerPrefs.GetInt("language"))
                        {
                            case 0: // 日语
                                Show(1, "蓋牌+トップ2枚");
                                break;
                            case 1: // 简体中文
                                Show(1, "查看盖牌和顶牌2张");
                                break;
                            case 2: // 繁体中文
                                Show(1, "查看蓋牌和頂牌2張");
                                break;
                            case 3: // 英语
                                Show(1, "Peek Hole + Top 2");
                                break;
                            case 4: // 韩语
                                Show(1, "홀카드+탑 2장");
                                break;
                        }

                        currentCount = data.Item_2;
                        currentCount--;
                        data.Item_2 = currentCount;

                        break;
                    case 2:
                        //均衡徽章
                        Item_SameScore();//强制平局

                        currentCount = data.Item_3;
                        currentCount--;
                        data.Item_3 = currentCount;

                        break;
                    case 3:
                        //魔眼石
                        StartCoroutine(Item_ViewCard());//看女荷官的盖牌     
                        StartCoroutine(Item_ViewNextCard());//看牌堆下一张卡

                        switch (PlayerPrefs.GetInt("language"))
                        {
                            case 0: // 日语
                                Show(3, "蓋牌+トップ1枚");
                                break;
                            case 1: // 简体中文
                                Show(3, "查看盖牌和顶牌1张");
                                break;
                            case 2: // 繁体中文
                                Show(3, "查看蓋牌和頂牌1張");
                                break;
                            case 3: // 英语
                                Show(3, "Peek Hole + Top 1");
                                break;
                            case 4: // 韩语
                                Show(3, "홀카드+탑 1장");
                                break;
                        }

                        currentCount = data.Item_4;
                        currentCount--;
                        data.Item_4 = currentCount;

                        break;
                    case 4:                      
                        Dealer_StealItems_4(); //玩家使用酒瓶，安托偷过来给自己双倍
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）    

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
                        Dealer_StealItems_6(); //玩家使用幸运币，安托偷过来给自己+1
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）    

                        currentCount = data.Item_7;
                        currentCount--;
                        data.Item_7 = currentCount;

                        break;
                    case 7:
                        //透视药水
                        StartCoroutine(Item_ViewNextCard());//看牌堆下一张卡
                        StartCoroutine(Item_ViewSecondNextCard());//看牌堆下下张卡

                        switch (PlayerPrefs.GetInt("language"))
                        {
                            case 0: // 日语
                                Show(7, "トップ2枚表示");
                                break;
                            case 1: // 简体中文
                                Show(7, "查看顶牌2张");
                                break;
                            case 2: // 繁体中文
                                Show(7, "查看頂牌2張");
                                break;
                            case 3: // 英语
                                Show(7, "See Top 2 Cards");
                                break;
                            case 4: // 韩语
                                Show(7, "탑 2장 보기");
                                break;
                        }

                        currentCount = data.Item_8;
                        currentCount--;
                        data.Item_8 = currentCount;

                        break;



                    case 8:         
                        Dealer_StealItems_8(); //玩家使用绿色心情，安托偷过来给自己＋3
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）    


                        currentCount = data.Item_9;
                        currentCount--;
                        data.Item_9 = currentCount;

                        break;

                    case 9:
                        //匕首
                        Item_TryBurnTopCard();//移除牌堆顶牌

                        currentCount = data.Item_10;
                        currentCount--;
                        data.Item_10 = currentCount;

                        break;

                    case 10:           
                        Dealer_StealItems_10();//玩家使用黑棋子，安托偷过来给自己＋5
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）    

                        currentCount = data.Item_11;
                        currentCount--;
                        data.Item_11 = currentCount;

                        break;

                    case 11:
                        //魔眼药水
                        Item_TryShuffleDeck();//洗牌

                        currentCount = data.Item_12;
                        currentCount--;
                        data.Item_12 = currentCount;

                        break;

                    case 12:
                        Dealer_StealItems_12(); //玩家使用空瓶，安托偷过来给玩家双倍
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）    

                        currentCount = data.Item_13;
                        currentCount--;
                        data.Item_13 = currentCount;

                        break;

                    case 13:                      
                        Dealer_StealItems_13(); //玩家使用白棋子，安托偷过来给玩家＋5
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）    

                        currentCount = data.Item_14;
                        currentCount--;
                        data.Item_14 = currentCount;

                        break;

                    case 14:
                        Dealer_StealItems_14(); //玩家使用厄运币，安托偷过来给自己-1
                        ConsumeOne();//消耗一颗爱心（先执行再扣爱心）    

                        currentCount = data.Item_15;
                        currentCount--;
                        data.Item_15 = currentCount;

                        break;

                    case 15:
                        //皇室徽章
                        ActivateDoubleRewardThisRound();//本局如果获胜获得双倍奖励

                        currentCount = data.Item_16;
                        currentCount--;
                        data.Item_16 = currentCount;
                        break;
                }
            }
            else
            {

                //爱丽丝在玩家使用道具后会再度使用回血道具(随机)
                if (currentDealer == DealerType.Alice && Random.Range(0, 2) == 0)
                {
                    //Invoke(nameof(Dealer_AliceMilk), 2f);
                    //女荷官使用回复生命
                    Dealer_AliceMilk();
                }

                //玩家正常使用物品
                switch (CurrentItem)
                {
                    case 0:
                        //紫色心情
                        Item_DecreasePlayerScore_3();//修改玩家点数-3

                        currentCount = data.Item_1;
                        currentCount--;
                        data.Item_1 = currentCount;
                        break;
                    case 1:
                        //占卜水晶
                        StartCoroutine(Item_ViewNextCard());//看牌堆下一张卡
                        StartCoroutine(Item_ViewCard());//看女荷官的盖牌   
                        StartCoroutine(Item_ViewSecondNextCard());//看牌堆下下张卡

                        switch (PlayerPrefs.GetInt("language"))
                        {
                            case 0: // 日语
                                Show(1, "蓋牌+トップ2枚");
                                break;
                            case 1: // 简体中文
                                Show(1, "查看盖牌和顶牌2张");
                                break;
                            case 2: // 繁体中文
                                Show(1, "查看蓋牌和頂牌2張");
                                break;
                            case 3: // 英语
                                Show(1, "Peek Hole + Top 2");
                                break;
                            case 4: // 韩语
                                Show(1, "홀카드+탑 2장");
                                break;
                        }

                        currentCount = data.Item_2;
                        currentCount--;
                        data.Item_2 = currentCount;
                        break;
                    case 2:
                        //均衡徽章
                        Item_SameScore();//强制平局

                        currentCount = data.Item_3;
                        currentCount--;
                        data.Item_3 = currentCount;
                        break;
                    case 3:
                        //魔眼石
                        StartCoroutine(Item_ViewCard());//看女荷官的盖牌     
                        StartCoroutine(Item_ViewNextCard());//看牌堆下一张卡

                        switch (PlayerPrefs.GetInt("language"))
                        {
                            case 0: // 日语
                                Show(3, "蓋牌+トップ1枚");
                                break;
                            case 1: // 简体中文
                                Show(3, "查看盖牌和顶牌1张");
                                break;
                            case 2: // 繁体中文
                                Show(3, "查看蓋牌和頂牌1張");
                                break;
                            case 3: // 英语
                                Show(3, "Peek Hole + Top 1");
                                break;
                            case 4: // 韩语
                                Show(3, "홀카드+탑 1장");
                                break;
                        }

                        currentCount = data.Item_4;
                        currentCount--;
                        data.Item_4 = currentCount;
                        break;
                    case 4:
                        //酒瓶
                        Item_PlayerDoubleScore();//玩家一方双倍    

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
                        Item_IncreasePlayerScore();//修改你的点数+1

                        currentCount = data.Item_7;
                        currentCount--;
                        data.Item_7 = currentCount;
                        break;
                    case 7:
                        //透视药水
                        StartCoroutine(Item_ViewNextCard());//看牌堆下一张卡
                        StartCoroutine(Item_ViewSecondNextCard());//看牌堆下下张卡


                        switch (PlayerPrefs.GetInt("language"))
                        {
                            case 0: // 日语
                                Show(7, "トップ2枚表示");
                                break;
                            case 1: // 简体中文
                                Show(7, "查看顶牌2张");
                                break;
                            case 2: // 繁体中文
                                Show(7, "查看頂牌2張");
                                break;
                            case 3: // 英语
                                Show(7, "See Top 2 Cards");
                                break;
                            case 4: // 韩语
                                Show(7, "탑 2장 보기");
                                break;
                        }

                        currentCount = data.Item_8;
                        currentCount--;
                        data.Item_8 = currentCount;
                        break;



                    case 8:
                        //绿色心情
                        Item_IncreasePlayerScore_3();//修改玩家点数+3

                        currentCount = data.Item_9;
                        currentCount--;
                        data.Item_9 = currentCount;
                        break;

                    case 9:
                        //匕首
                        Item_TryBurnTopCard();//移除牌堆顶牌

                        currentCount = data.Item_10;
                        currentCount--;
                        data.Item_10 = currentCount;
                        break;

                    case 10:
                        //黑棋子
                        Item_IncreasePlayerScore_5();//修改你的点数+5

                        currentCount = data.Item_11;
                        currentCount--;
                        data.Item_11 = currentCount;
                        break;

                    case 11:
                        //魔眼药水
                        Item_TryShuffleDeck();//洗牌

                        currentCount = data.Item_12;
                        currentCount--;
                        data.Item_12 = currentCount;
                        break;

                    case 12:
                        //空瓶
                        Item_DealerDoubleScore();//女荷官一方双倍

                        currentCount = data.Item_13;
                        currentCount--;
                        data.Item_13 = currentCount;
                        break;

                    case 13:
                        //白棋子
                        Item_IncreaseFemaleDealerScore_5();//修改女荷官点数+5

                        currentCount = data.Item_14;
                        currentCount--;
                        data.Item_14 = currentCount;
                        break;

                    case 14:
                        //厄运币
                        Item_DecreasePlayerScore();//修改你的点数-1

                        currentCount = data.Item_15;
                        currentCount--;
                        data.Item_15 = currentCount;
                        break;

                    case 15:
                        //皇室徽章
                        ActivateDoubleRewardThisRound();//本局如果获胜获得双倍奖励

                        currentCount = data.Item_16;
                        currentCount--;
                        data.Item_16 = currentCount;
                        break;
                }


            }




            //写回存档
            SaveManager.SaveGame(data);

            itemManager.UpdateInventoryUI();

            //隐藏使用按钮
            USE_Button.SetActive(false);


            //增加使用物品次数
            itemsUsed++;

            //使用物品的时候，宝箱消失
            HideTreasureAndItemUI();

            //物品通常是1秒钟出现消失
            Invoke(nameof(ShowTreasureBoxIfAllowed), 1f);


          
        }




        public IEnumerator Item_ViewCard()
        {

            //锁按钮再解锁
            //_ui.DisableButtons();


            dealer.ConcealCard();

            CardData nextCard = dealer.hand.GetSecondCard().cardData;

            //等待再结算
            yield return new WaitForSeconds(1.5f);


            PeekCard.gameObject.SetActive(true);
            PeekCard.mesh = nextCard.GetMesh();

            //switch (PlayerPrefs.GetInt("language"))
            //{
            //    case 0:
            //        // 日语
            //        Show(3, "ディーラーの伏せ札を見る");
            //        break;
            //
            //    case 1:
            //        // 简体中文
            //        Show(3, "查看庄家盖牌");
            //        break;
            //
            //    case 2:
            //        // 繁体中文
            //        Show(3, "查看莊家蓋牌");
            //        break;
            //
            //    case 3:
            //        // 英语
            //        Show(3, "Reveal Dealer's Hole Card");
            //        break;
            //
            //    case 4:
            //        // 韩语
            //        Show(3, "딜러의 히든 카드 확인");
            //        break;
            //
            //}


            //Invoke(nameof(Unclock), 0.5f);

        }//看女荷官的盖牌

        [Header("展示牌")]
        public MeshFilter ShowCard;

        public MeshFilter PeekCard;
        public MeshFilter PeekNextCard;
        public MeshFilter PeekSecondNextCard;

        public IEnumerator Item_ViewNextCard()
        {
            CardData nextCard = Deck.PeekCard();  // 检视但不抽取下一张牌
            //ShowCard.gameObject.SetActive(true);
            //ShowCard.mesh = nextCard.GetMesh();
            //
            //
            ////等待再结算
            //yield return new WaitForSeconds(2.5f);
            yield return new WaitForSeconds(0.1f);

            PeekNextCard.gameObject.SetActive(true); PeekNextCard.GetComponent<Animator>().Rebind();//动画恢复状态
            PeekNextCard.mesh = nextCard.GetMesh();


            //switch (PlayerPrefs.GetInt("language"))
            //{
            //    case 0:
            //        // 日语
            //        Show(1, "山札の次のカードを見る");
            //        break;
            //
            //    case 1:
            //        // 简体中文
            //        Show(1, "查看牌堆下一张牌");
            //        break;
            //
            //    case 2:
            //        // 繁体中文
            //        Show(1, "查看牌堆下一張牌");
            //        break;
            //
            //    case 3:
            //        // 英语
            //        Show(1, "Reveal Next Card in Deck");
            //        break;
            //
            //    case 4:
            //        // 韩语
            //        Show(1, "덱의 다음 카드 확인");
            //        break;
            //
            //}

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);

        }//看你的下一张卡


        public IEnumerator Item_ViewSecondNextCard()
        {
            CardData nextCard = Deck.PeekSecondNextCard();  // 检视但不抽取下下张牌  
            //ShowCard.gameObject.SetActive(true);
            //ShowCard.mesh = nextCard.GetMesh();
            //
            //
            ////等待再结算
            //yield return new WaitForSeconds(2.5f);
            yield return new WaitForSeconds(0.1f);

            PeekSecondNextCard.gameObject.SetActive(true); PeekSecondNextCard.GetComponent<Animator>().Rebind();//动画恢复状态
            PeekSecondNextCard.mesh = nextCard.GetMesh();


            //switch (PlayerPrefs.GetInt("language"))
            //{
            //    case 0:
            //        // 日语
            //        Show(7, "山札の二枚目を見る");
            //        break;
            //
            //    case 1:
            //        // 简体中文
            //        Show(7, "查看牌堆下下张牌");
            //        break;
            //
            //    case 2:
            //        // 繁体中文
            //        Show(7, "查看牌堆下下張牌");
            //        break;
            //
            //    case 3:
            //        // 英语
            //        Show(7, "Reveal Second Card in Deck");
            //        break;
            //
            //    case 4:
            //        // 韩语
            //        Show(7, "덱의 두 번째 카드 확인");
            //        break;
            //
            //}

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);

        }//看你的下一张卡


        public void Item_IncreasePlayerScore_5()
        {
            player.hand.ChangeScore(5);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(10, "プレイヤーの点数 +5");
                    break;

                case 1:
                    // 简体中文
                    Show(10, "玩家点数 +5");
                    break;

                case 2:
                    // 繁体中文
                    Show(10, "玩家點數 +5");
                    break;

                case 3:
                    // 英语
                    Show(10, "Player Score +5");
                    break;

                case 4:
                    // 韩语
                    Show(10, "플레이어 점수 +5");
                    break;

            }


            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);


        }//修改你的点数+5

        public void Item_IncreasePlayerScore()
        {
            player.hand.ChangeScore(1);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(6, "プレイヤーの点数 +1");
                    break;

                case 1:
                    // 简体中文
                    Show(6, "玩家点数 +1");
                    break;

                case 2:
                    // 繁体中文
                    Show(6, "玩家點數 +1");
                    break;

                case 3:
                    // 英语
                    Show(6, "Player Score +1");
                    break;

                case 4:
                    // 韩语
                    Show(6, "플레이어 점수  +1");
                    break;

            }

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);


        }//修改你的点数+1

        public void Item_DecreasePlayerScore()
        {
            player.hand.ChangeScore(-1);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(14, "プレイヤーの点数 -1");
                    break;

                case 1:
                    // 简体中文
                    Show(14, "玩家点数 -1");
                    break;

                case 2:
                    // 繁体中文
                    Show(14, "玩家點數 -1");
                    break;

                case 3:
                    // 英语
                    Show(14, "Player Score -1");
                    break;

                case 4:
                    // 韩语
                    Show(14, "플레이어 점수  -1");
                    break;

            }


            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);


        }//修改你的点数-1

        public void Item_IncreasePlayerScore_3()
        {
            player.hand.ChangeScore(3);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(8, "プレイヤーの点数 +3");
                    break;

                case 1:
                    // 简体中文
                    Show(8, "玩家点数 +3");
                    break;

                case 2:
                    // 繁体中文
                    Show(8, "玩家點數 +3");
                    break;

                case 3:
                    // 英语
                    Show(8, "Player Score +3");
                    break;

                case 4:
                    // 韩语
                    Show(8, "플레이어 점수  +3");
                    break;

            }

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);


        }//修改你的点数+3

        public void Item_DecreasePlayerScore_3()
        {
            player.hand.ChangeScore(-3);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(0, "プレイヤーの点数 -3");
                    break;

                case 1:
                    // 简体中文
                    Show(0, "玩家点数 -3");
                    break;

                case 2:
                    // 繁体中文
                    Show(0, "玩家點數 -3");
                    break;

                case 3:
                    // 英语
                    Show(0, "Player Score -3");
                    break;

                case 4:
                    // 韩语
                    Show(0, "플레이어 점수  -3");
                    break;

            }


            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);


        }//修改你的点数-3

        public void Item_IncreaseFemaleDealerScore_5()
        {
            dealer.hand.ChangeScore(5);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(13, "ディーラーの点数 +5");
                    break;

                case 1:
                    // 简体中文
                    Show(13, "庄家点数 +5");
                    break;

                case 2:
                    // 繁体中文
                    Show(13, "莊家點數 +5");
                    break;

                case 3:
                    // 英语
                    Show(13, "Dealer Score +5");
                    break;

                case 4:
                    // 韩语
                    Show(13, "주가의 점수  +5");
                    break;

            }

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);


        }//修改女荷官点数+5


        #region  女荷官±1
        // public void Item_IncreaseFemaleDealerScore()
        // {
        //     dealer.hand.ChangeScore(1);
        //
        //     switch (PlayerPrefs.GetInt("language"))
        //     {
        //         case 0:
        //             // 日语
        //             Show(0, "ディーラーの点数 +1");
        //             break;
        //
        //         case 1:
        //             // 简体中文
        //             Show(0, "庄家点数 +1");
        //             break;
        //
        //         case 2:
        //             // 繁体中文
        //             Show(0, "莊家點數 +1");
        //             break;
        //
        //         case 3:
        //             // 英语
        //             Show(0, "Dealer Score +1");
        //             break;
        //
        //         case 4:
        //             // 韩语
        //             Show(0, "주가의 점수 +1");
        //             break;
        //
        //     }
        //
        //     //锁按钮再解锁
        //     //_ui.DisableButtons();
        //     //Invoke(nameof(Unclock), 0.5f);
        //
        //
        // }//修改女荷官点数+1
        //
        // public void Item_DecreaseFemaleDealerScore()
        // {
        //     dealer.hand.ChangeScore(-1);
        //
        //     switch (PlayerPrefs.GetInt("language"))
        //     {
        //         case 0:
        //             // 日语
        //             Show(8, "ディーラーの点数 -1");
        //             break;
        //
        //         case 1:
        //             // 简体中文
        //             Show(8, "庄家点数 -1");
        //             break;
        //
        //         case 2:
        //             // 繁体中文
        //             Show(8, "莊家點數 -1");
        //             break;
        //
        //         case 3:
        //             // 英语
        //             Show(8, "Dealer Score -1");
        //             break;
        //
        //         case 4:
        //             // 韩语
        //             Show(8, "주가의 점수  -1");
        //             break;
        //
        //     }
        //
        //     //锁按钮再解锁
        //     //_ui.DisableButtons();
        //     //Invoke(nameof(Unclock), 0.5f);
        //
        // }//修改女荷官点数-1
        #endregion


        public void Item_PlayerDoubleScore()
        {

            player.hand.ChangeScore(player.Score);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(4, "プレイヤー点数2倍");
                    break;

                case 1:
                    // 简体中文
                    Show(4, "玩家点数翻倍");
                    break;

                case 2:
                    // 繁体中文
                    Show(4, "玩家點數翻倍");
                    break;

                case 3:
                    // 英语
                    Show(4, "Dealer Score Doubled");
                    break;

                case 4:
                    // 韩语
                    Show(4, "딜러 점수 2배");
                    break;

            }


            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);

        }//玩家一方双倍

        public void Item_DealerDoubleScore()
        {
            dealer.hand.ChangeScore(dealer.Score);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(12, "ディーラー点数2倍");
                    break;

                case 1:
                    // 简体中文
                    Show(12, "庄家点数翻倍");
                    break;

                case 2:
                    // 繁体中文
                    Show(12, "莊家點數翻倍");
                    break;

                case 3:
                    // 英语
                    Show(12, "Dealer Score Doubled");
                    break;

                case 4:
                    // 韩语
                    Show(12, "딜러 점수 2배");
                    break;

            }


            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);

        }//女荷官一方双倍

        public Animator FadeDeck;

        public bool Item_TryBurnTopCard()
        {

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(9, "一番上のカードをデッキの底に置く");
                    break;

                case 1:
                    // 简体中文
                    Show(9, "牌库第一张牌置于牌库底部");
                    break;

                case 2:
                    // 繁体中文
                    Show(9, "牌庫第一張牌置於牌庫底部");
                    break;

                case 3:
                    // 英语
                    Show(9, "Move top card to the bottom");
                    break;

                case 4:
                    // 韩语
                    Show(9, "덱의 맨 위 카드를 덱 바닥으로 이동");
                    break;

            }






            // 只允许在玩家操作阶段用
            if (State != GameState.OnPlay) return false;
            if (!GameActive) return false;

            // 你也可以加更多限制：比如发牌队列处理中不能用
            if (DealQueue.processing) return false;

            Deck.GetCard(); // ✅ 直接抽走顶牌（= 丢弃）

            // ✅ 无论第一张牌被抽取还是怎么样，Peek的牌隐藏
            if (PeekNextCard.gameObject.activeInHierarchy)
            {
                PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
                Invoke(nameof(CheckAgain_Item_ViewNextCard), 1f);
            }
            if (PeekSecondNextCard.gameObject.activeInHierarchy)
            {
                PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
                Invoke(nameof(CheckAgain_Item_ViewSecondNextCard), 1f);
            }



            FadeDeck.SetTrigger("TakeOneCard");

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.9f);

            return true;
        }//移除牌堆第一张牌



        public bool Item_TryShuffleDeck()
        {

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(11, "デッキをシャッフル");
                    break;

                case 1:
                    // 简体中文
                    Show(11, "牌库洗牌");
                    break;

                case 2:
                    // 繁体中文
                    Show(11, "牌庫洗牌");
                    break;

                case 3:
                    // 英语
                    Show(11, "Shuffle the Deck");
                    break;

                case 4:
                    // 韩语
                    Show(11, "덱 셔플");
                    break;

            }



            // ✅ 只允许玩家可操作阶段洗牌（避免发牌过程中洗牌导致“已经排队的牌”变了）
            if (State != GameState.OnPlay) return false;
            if (!GameActive) return false;
            if (DealQueue.processing) return false;

            Deck.ShuffleNow();

            // ✅ 洗牌后，Peek 系列全部无效，直接关掉
            if (PeekNextCard.gameObject.activeInHierarchy)
            {
                PeekNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
                Invoke(nameof(CheckAgain_Item_ViewNextCard), 1.2f);
            }
            if (PeekSecondNextCard.gameObject.activeInHierarchy)
            {
                PeekSecondNextCard.GetComponent<Animator>().SetTrigger("Out");//卡片回去动画帧事件消失
                Invoke(nameof(CheckAgain_Item_ViewSecondNextCard), 1.2f);
            }

            //ShowCard.gameObject.SetActive(false);

            FadeDeck.SetTrigger("Shuffle");

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 1.1f);

            return true;
        }//洗牌


        bool SameScore = false;
        public GameObject Sign_SameScore;
        public void Item_SameScore()
        {
            SameScore = true;
            Sign_SameScore.SetActive(true);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(2, "ディーラーの点数をプレイヤーと同じにする");
                    break;

                case 1:
                    // 简体中文
                    Show(2, "庄家强制变成玩家点数");
                    break;

                case 2:
                    // 繁体中文
                    Show(2, "莊家強制變成玩家點數");
                    break;

                case 3:
                    // 英语
                    Show(2, "Dealer Score Equals Player Score");
                    break;

                case 4:
                    // 韩语
                    Show(2, "딜러 점수를 플레이어와 동일하게 변경");
                    break;

            }

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);

        }//强制平局




        bool SaveScore = false;
        public GameObject Sign_SaveScore;
        int SaveNumber;
        public Text Sign_SaveScore_SaveNumber;
        public void Item_SaveScore()
        {
            SaveScore = true;
            Sign_SaveScore.SetActive(true);


            SaveNumber = Random.Range(3, 6);

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(5, "プレイヤー点数を強制的に-" + SaveNumber);
                    break;

                case 1:
                    // 简体中文
                    Show(5, "玩家点数强制削减" + SaveNumber);
                    break;

                case 2:
                    // 繁体中文
                    Show(5, "玩家點數強制削減" + SaveNumber);
                    break;

                case 3:
                    // 英语
                    Show(5, "Player Score -" + SaveNumber);
                    break;

                case 4:
                    // 韩语
                    Show(5, "플레이어 점수 강제 -" + SaveNumber);
                    break;

            }

            Sign_SaveScore_SaveNumber.text = "-" + SaveNumber.ToString();//数字展示出来

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);

        }//点数超过21，强制削减随机3~5



        public float rewardMultiplier = 1f; // 本局奖励倍率（默认1）
        public GameObject Sign_DoubleReward;
        public void ActivateDoubleRewardThisRound()
        {

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Show(15, "勝利で報酬2倍");
                    break;

                case 1:
                    // 简体中文
                    Show(15, "获胜的话双倍奖励");
                    break;

                case 2:
                    // 繁体中文
                    Show(15, "獲勝的話雙倍獎勵");
                    break;

                case 3:
                    // 英语
                    Show(15, "Double reward if win");
                    break;

                case 4:
                    // 韩语
                    Show(15, "이 판 승리 시 보상 2배");
                    break;

            }
            Sign_DoubleReward.SetActive(true);

            rewardMultiplier = 2f;

            //锁按钮再解锁
            //_ui.DisableButtons();
            //Invoke(nameof(Unclock), 0.5f);

        }//双倍奖励


        private void ResetDoubleReward()
        {
            //双倍奖励标志消失
            Sign_DoubleReward.SetActive(false);
            rewardMultiplier = 1f; // ✅ 双倍奖励重置

        }//重置双倍奖励状态




        void Unclock()
        {
            _ui.PlayingState(player, false);
        }


        #endregion


        /// <summary>
        /// 物品被使用提示
        /// </summary>
        #region
        [Header("物品被使用提示")]
        public GameObject root;                 // 整个提示面板（用于SetActive）
        public Image itemImage;
        public Text tipText;                    // 老版Text
        public List<Sprite> List_Item_Image; // 8个图片对象，对应物品0~7


        public Text Score_HUD;//更改点数通用


        public void Show(int itemId, string text)
        {
            root.SetActive(true);

            if (itemId >= 0 && itemId < List_Item_Image.Count)
                itemImage.sprite = List_Item_Image[itemId];

            tipText.text = text;
        }



        #endregion


        /// <summary>
        /// 关卡与女荷官生命值/女荷官动画
        /// </summary>
        #region
        [Header("女荷官生命值")]
        public Text healthText;
        public Image healthFillImage;       // 前景真实血条
        public Image healthGhostFillImage;     // 半透明覆盖血条
        public float maxHealth = 1000f;
        public float currentHealth;

        private float ghostHealth;             // 半透明条显示值
        private float displayHealth;    // UI当前显示的血量
        private Coroutine healthAnimCoroutine;

        public Text Limit;//本局赌注上限
        public int LimitPlace;//本局赌注上限


        int Progress;//女荷官进度


        public enum DealerType
        {
            Anto,
            Hetty,
            Alice
        }

        private DealerType currentDealer;//女荷官类型


        #region  兼合了通关后的关卡选择

        private int GetStageProgressBySceneId(string sceneId)
        {
            switch (sceneId)
            {
                case "Anto_CG_01_1": return 1;
                case "Anto_CG_02_1": return 2;
                case "Anto_CG_03_1": return 3;
                case "Anto_CG_04_1": return 4;
                case "Anto_CG_05_1": return 5;
                case "Anto_CG_06_1": return 6;
                case "Anto_CG_07_1": return 7;
                case "Anto_CG_08_1": return 8;
                case "Anto_CG_09_1": return 9;
                case "Anto_CG_10_1": return 10;

                case "Hetty_CG_01_1": return 1;
                case "Hetty_CG_02_1": return 2;
                case "Hetty_CG_03_1": return 3;
                case "Hetty_CG_04_1": return 4;
                case "Hetty_CG_05_1": return 5;
                case "Hetty_CG_06_1": return 6;
                case "Hetty_CG_07_1": return 7;
                case "Hetty_CG_08_1": return 8;
                case "Hetty_CG_09_1": return 9;
                case "Hetty_CG_10_1": return 10;

                case "Alice_CG_01_1": return 1;
                case "Alice_CG_02_1": return 2;
                case "Alice_CG_03_1": return 3;
                case "Alice_CG_04_1": return 4;
                case "Alice_CG_05_1": return 5;
                case "Alice_CG_06_1": return 6;
                case "Alice_CG_07_1": return 7;
                case "Alice_CG_08_1": return 8;
                case "Alice_CG_09_1": return 9;
                case "Alice_CG_10_1": return 10;
            }

            return 0; // 0 = 没有指定关卡，走正常存档进度
        }

        private int GetEffectiveProgress(DealerType dealerType, SaveData data)
        {
            int stageProgress = GetStageProgressBySceneId(GameFlowData.nextAVGId);

            // 如果端口名里带了关卡，就优先用那个
            if (stageProgress > 0)
            {
                return stageProgress;
            }

            // 否则走正常存档进度
            switch (dealerType)
            {
                case DealerType.Anto:
                    return data.antoProgress;
                case DealerType.Hetty:
                    return data.hettyProgress;
                case DealerType.Alice:
                    return data.aliceProgress;
                default:
                    return 1;
            }
        }

        private int ClampProgress(int progress)
        {
            return Mathf.Clamp(progress, 1, 10);
        }


        #endregion


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
            
                case "VSAnto":
                case "Anto_CG_01_1":
                case "Anto_CG_02_1":
                case "Anto_CG_03_1":
                case "Anto_CG_04_1":
                case "Anto_CG_05_1":
                case "Anto_CG_06_1":
                case "Anto_CG_07_1":
                case "Anto_CG_08_1":
                case "Anto_CG_09_1":
                case "Anto_CG_10_1":
                    dealerAnimator = antoAnimator;
                    currentDealer = DealerType.Anto;

                    //Progress = data.antoProgress;
                    Progress = ClampProgress(GetEffectiveProgress(currentDealer, data));//兼具了关卡读取的功能

                    CheckoutScreen_Dealer.sprite = CheckoutScreen_Anto;

                    //选择女荷官界面BGM
                    BGM.instance.Stop();
                    BGM.instance.AudioPlayBackgroundMusic(2);


                    //女荷官反制槽
                    int antoHearts = GetAntoHearts(Progress);
                    maxCounter = 3;   // 安托上限
                    Init(antoHearts);




                    break;
                
                case "VSHetty":
                case "Hetty_CG_01_1":
                case "Hetty_CG_02_1":
                case "Hetty_CG_03_1":
                case "Hetty_CG_04_1":
                case "Hetty_CG_05_1":
                case "Hetty_CG_06_1":
                case "Hetty_CG_07_1":
                case "Hetty_CG_08_1":
                case "Hetty_CG_09_1":
                case "Hetty_CG_10_1":
                    dealerAnimator = hettyAnimator;
                    currentDealer = DealerType.Hetty;

                    //Progress = data.hettyProgress;
                    Progress = ClampProgress(GetEffectiveProgress(currentDealer, data));//兼具了关卡读取的功能

                    CheckoutScreen_Dealer.sprite = CheckoutScreen_Hetty;

                    //选择女荷官界面BGM
                    BGM.instance.Stop();
                    BGM.instance.AudioPlayBackgroundMusic(8);


                    //女荷官反制槽
                    int hettyHearts = GetHettyHearts(Progress);
                    maxCounter = 2;   // 赫蒂上限
                    Init(hettyHearts);


                    break;
                default:
                case "VSAlice":
                case "Alice_CG_01_1":
                case "Alice_CG_02_1":
                case "Alice_CG_03_1":
                case "Alice_CG_04_1":
                case "Alice_CG_05_1":
                case "Alice_CG_06_1":
                case "Alice_CG_07_1":
                case "Alice_CG_08_1":
                case "Alice_CG_09_1":
                case "Alice_CG_10_1":
                    dealerAnimator = aliceAnimator;
                    currentDealer = DealerType.Alice;

                    //Progress = data.aliceProgress;
                    Progress = ClampProgress(GetEffectiveProgress(currentDealer, data));//兼具了关卡读取的功能

                    CheckoutScreen_Dealer.sprite = CheckoutScreen_Alice;

                    //选择女荷官界面BGM
                    BGM.instance.Stop();
                    BGM.instance.AudioPlayBackgroundMusic(10);



                    //女荷官反制槽
                    int aliceHearts = GetAliceHearts(Progress);
                    maxCounter = 4;   // 爱丽丝上限
                    Init(aliceHearts);



                    break;


            }

            dealerAnimator.gameObject.SetActive(true);



            //检测对应女荷官等级
            LimitPlace = Progress * 100;
            Limit.text = LimitPlace.ToString();//本局赌注上限


            //直接通过GameManager去设置
            //LimitBetPlate._Instance.max = LimitPlace;
            //LimitBetPlate.SetLimit(1, LimitPlace);

            maxHealth = Progress * 500;

            currentHealth = maxHealth;
            //UpdateFill();

            displayHealth = currentHealth;
            ghostHealth = currentHealth;
            UpdateAllBarsImmediate();



            Invoke("StartMatch", 1f); // 开局时显示女荷官垃圾话
            //Invoke("StartDialog", 1f);//游戏开始时显示女荷官垃圾话

            TableAnim.SetInteger("ChangeColor", 1);//桌子强制变淡
        }

        int GetAntoHearts(int p)
        {
            if (p <= 3) return 0;
            if (p <= 6) return 2;
            if (p <= 9) return 3;
            return 5;
        }
        int GetHettyHearts(int p)
        {
            if (p <= 3) return 0;
            if (p <= 6) return 1;
            if (p <= 9) return 2;
            return 3;
        }
        int GetAliceHearts(int p)
        {
            if (p <= 3) return 0;
            if (p <= 6) return 3;
            if (p <= 9) return 5;
            return 7;
        }







        public Text Health_HUD;

        public static void ChangeHealth(float amount, bool NeedAnimator)//调整女荷官生命值的时候不一定需要动画
        {



            //显示回血伤血提示
            if (amount > 0)
            {
                _Instance.Health_HUD.color = Color.green;
                _Instance.Health_HUD.text = "+" + amount.ToString();
            }
            else
            {
                _Instance.Health_HUD.color = Color.red;
                _Instance.Health_HUD.text = amount.ToString();
            }
            _Instance.Health_HUD.gameObject.SetActive(true);

            float oldHealth = _Instance.currentHealth;


            _Instance.currentHealth += amount;
            _Instance.currentHealth = Mathf.Clamp(_Instance.currentHealth, 0, _Instance.maxHealth);
            //_Instance.UpdateFill();

            //血条动画
            // 不直接 UpdateFill()
            if (_Instance.healthAnimCoroutine != null)
            {
                _Instance.StopCoroutine(_Instance.healthAnimCoroutine);
            }
            _Instance.healthAnimCoroutine = _Instance.StartCoroutine(
                  _Instance.AnimateHealthBarDual(oldHealth, _Instance.currentHealth)
              );



            if (NeedAnimator)
            {
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

            }




        }

        //void UpdateFill()
        //{
        //    healthFillImage.fillAmount = currentHealth / maxHealth;
        //    healthText.text = $"{currentHealth} / {maxHealth}";
        //}


        //血条动画
        private IEnumerator AnimateHealthBarDual(float oldHealth, float newHealth)
        {
            // 受伤
            if (newHealth < oldHealth)
            {
                // 前景真实血条先立刻到目标
                displayHealth = newHealth;
                UpdateFrontBar();

                // 半透明条停顿一下，再慢慢掉
                yield return new WaitForSeconds(0.15f);

                while (ghostHealth > newHealth)
                {
                    ghostHealth -= 100f;
                    if (ghostHealth < newHealth)
                        ghostHealth = newHealth;

                    UpdateGhostBar();
                    UpdateHealthText();

                    yield return new WaitForSeconds(0.02f);
                }
            }
            // 治疗
            else if (newHealth > oldHealth)
            {
                // 半透明条先立刻到目标
                ghostHealth = newHealth;
                UpdateGhostBar();

                // 前景真实血条慢慢追上去
                while (displayHealth < newHealth)
                {
                    displayHealth += 100f;
                    if (displayHealth > newHealth)
                        displayHealth = newHealth;

                    UpdateFrontBar();
                    UpdateHealthText();

                    yield return new WaitForSeconds(0.02f);
                }
            }

            UpdateAllBarsImmediate();
            healthAnimCoroutine = null;
        }
        void UpdateFrontBar()
        {
            healthFillImage.fillAmount = displayHealth / maxHealth;
        }

        void UpdateGhostBar()
        {
            healthGhostFillImage.fillAmount = ghostHealth / maxHealth;
        }

        void UpdateHealthText()
        {
            healthText.text = $"{displayHealth} / {maxHealth}";
        }

        void UpdateAllBarsImmediate()
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
            healthGhostFillImage.fillAmount = currentHealth / maxHealth;
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


            _Instance.ShowResult();//结算画面数据显示





        }//显示胜利画面
        public void ReLoadScene()
        {
            UIManager.LoadingScene_BJ_Mobile();
        }

        [Header("女荷官动画")]
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
                //_Instance.ApplyDamage(newSituation);

                // 每次只推进一个阶段
                _Instance.ApplyDamage(_Instance.currentSituation + 1);
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



            //dealerAnimator.SetInteger("Undress", 0); // 重置参数防止连播
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
        /// 女荷官反制动画
        /// </summary>
        #region

        [Header("女荷官反制槽")]
        public GameObject heartPrefab;
        public Transform heartGroup;

        private List<GameObject> hearts = new List<GameObject>();

        private int currentCounter;
        private int maxCounter = 7;   // 默认7颗爱心

        // 初始化
        public void Init(int max)
        {
            maxCounter = max;
            currentCounter = max;

            CreateHearts();
        }

        void CreateHearts()
        {
            // 清空旧的
            foreach (Transform child in heartGroup)
            {
                Destroy(child.gameObject);
            }

            hearts.Clear();

            for (int i = 0; i < maxCounter; i++)
            {
                GameObject h = Instantiate(heartPrefab, heartGroup);
                hearts.Add(h);
            }
        }

        // 消耗一颗
        public bool ConsumeOne()
        {
            if (currentCounter <= 0)
                return false;

            currentCounter--;




            //hearts[currentCounter].SetActive(false);

            //改成触发动画
            var heart = hearts[currentCounter];
            var anim = heart.GetComponent<Animator>();
            anim.SetTrigger("Out");



            return true;
        }

        //增加X颗爱心
        public void AddCounter(int amount)
        {
            if (amount <= 0) return;

            int oldValue = currentCounter;

            currentCounter += amount;

            if (currentCounter > maxCounter)
                currentCounter = maxCounter;


            // 播放新增部分的 In 动画
            for (int i = oldValue; i < currentCounter; i++)
            {
                if (i < hearts.Count)
                {
                    hearts[i].SetActive(true);
                }
            }
        }



        // 重置
        public void ResetCounter()
        {
            currentCounter = maxCounter;

            for (int i = 0; i < hearts.Count; i++)
            {
                hearts[i].SetActive(true);
            }
        }

        public bool HasCounter()
        {
            return currentCounter > 0;
        }




        [Header("女荷官反制动画")]
        public Animator Interrupt;
        public Text Dealer_Interrupt_Text;
        public Image Dealer_Interrupt_Image;
        public GameObject Sign_Forbidden;
        public GameObject Sign_Steal;
        public void Interrupt_Item(int Item_Image)
        {
            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Dealer_Interrupt_Text.text = "反制アイテム";
                    break;
                case 1:
                    // 简体中文
                    Dealer_Interrupt_Text.text = "反制物品";
                    break;
                case 2:
                    // 繁体中文
                    Dealer_Interrupt_Text.text = "反制道具";
                    break;
                case 3:
                    // 英语
                    Dealer_Interrupt_Text.text = "Counter Item";
                    break;
                case 4:
                    // 韩语
                    Dealer_Interrupt_Text.text = "반제 아이템";
                    break;

            }

            Dealer_Interrupt_Image.sprite = List_Item_Image[Item_Image];
            Sign_Forbidden.SetActive(true);
            ShowDealerInterrputAnimator();

        }//反制玩家使用道具


        public void ShowDealerInterrputAnimator()
        {
            switch (currentDealer)
            {
                case DealerType.Anto:
                    Interrupt.SetTrigger("Anto");
                    break;

                case DealerType.Hetty:
                    Interrupt.SetTrigger("Hetty");
                    break;

                case DealerType.Alice:
                    Interrupt.SetTrigger("Alice");
                    break;
            }
        }//显示对应CutIn

        //----------------------------------------------------------------------------------------------------------------------------------------
        public void DealerUse_Item(int Item_Image)
        {

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Dealer_Interrupt_Text.text = "使用アイテム";
                    break;
                case 1:
                    // 简体中文
                    Dealer_Interrupt_Text.text = "使用物品";
                    break;
                case 2:
                    // 繁体中文
                    Dealer_Interrupt_Text.text = "使用物品";
                    break;
                case 3:
                    // 英语
                    Dealer_Interrupt_Text.text = "Use items";
                    break;
                case 4:
                    // 韩语
                    Dealer_Interrupt_Text.text = "사용 아이템";
                    break;

            }

            Dealer_Interrupt_Image.sprite = List_Item_Image[Item_Image];
            Sign_Forbidden.SetActive(false);
            Sign_Steal.SetActive(false);
            ShowDealerInterrputAnimator();

        }//女荷官使用道具

        public void Dealer_SaveScore()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具


            if (dealer.Score > 21)
            {
                Debug.Log("女荷官使用【救場：点数超过21，强制削减随机3~5】");

                int Dealer_SaveNumber = Random.Range(3, 6);

                dealer.hand.ChangeScore(-Dealer_SaveNumber);

                DealerUse_Item(5);


                ConsumeOne();//消耗一颗爱心

            }// 女荷官点数超过21，强制削减随机3~5


        }//女荷官使用藏宝图残片

        public void Dealer_SameScore()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具


            // 1) 玩家爆牌，庄家本来就赢，不用徽章
            if (player.Score > 21) return;

            // 2) 庄家爆牌，这时候应该走藏宝图残片救场，不用徽章
            if (dealer.Score > 21) return;

            // 3) 庄家本来就 >= 玩家，不会输，不用徽章
            if (dealer.Score >= player.Score) return;

            Debug.Log("女荷官使用【强制平局：女荷官点数和玩家相同】");

            dealer.hand.SetScore(player.Score);

            DealerUse_Item(2);

            ConsumeOne();//消耗一颗爱心

        }//女荷官在停牌之后，发现自己点数比玩家低，玩家也没有爆牌（高于21），强制让自己点数和玩家一致

        public void Dealer_TryBurnTopCard()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            // ✅ 无论第一张牌被抽取还是怎么样，Peek的牌隐藏
            if (PeekNextCard.gameObject.activeInHierarchy)
            {
                Deck.GetCard(); // ✅ 直接抽走顶牌（= 丢弃）
                FadeDeck.SetTrigger("TakeOneCard");

                DealerUse_Item(9);

                ConsumeOne();//消耗一颗爱心
            }
          
        }//女荷官在玩家有展示第一张的时候抓牌瞬间，使用匕首


        public void Dealer_IncreaseFemaleDealerScore_3() 
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            
            
            // 1) 玩家爆牌，庄家本来就赢，不用徽章
            if (player.Score > 21) return;
            

            

            if (dealer.Score == 17 || dealer.Score == 18) 
            {
                dealer.hand.ChangeScore(3);
            
                DealerUse_Item(8);
            
                ConsumeOne();//消耗一颗爱心
            }
            

        }//女荷官在结算时 17，18自动升级为 20，21

        public void Dealer_IncreaseFemaleDealerScore_1()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具


            // 1) 玩家爆牌，庄家本来就赢，不用徽章
            if (player.Score > 21) return;


            if (dealer.Score == 19 || dealer.Score == 20)
            {
                dealer.hand.ChangeScore(1);

                DealerUse_Item(6);

                ConsumeOne();//消耗一颗爱心
            }


        }//女荷官在结算时 19，20自动升级为 20，21

        public void Dealer_DecreaseFemaleDealerScore_1()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具


            // 1) 玩家爆牌，庄家本来就赢，不用徽章
            if (player.Score > 21) return;


            if (dealer.Score == 22)
            {
                dealer.hand.ChangeScore(-1);

                DealerUse_Item(14);

                ConsumeOne();//消耗一颗爱心
            }


        }//女荷官在结算时 22 爆牌 自动削减为 21

        public void Dealer_DecreaseFemaleDealerScore_3()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具


            // 1) 玩家爆牌，庄家本来就赢，不用徽章
            if (player.Score > 21) return;


            if (dealer.Score == 23|| dealer.Score == 24)
            {
                dealer.hand.ChangeScore(-3);

                DealerUse_Item(14);

                ConsumeOne();//消耗一颗爱心
            }


        }//女荷官在结算时 23  24 爆牌 自动削减为 20  21


        //----------------------------------------------------------------------------------------------------------------------------------------

        public Sprite Alice_Milk;
        public void Dealer_AliceMilk()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具

            if (_Instance.currentHealth >= _Instance.maxHealth) return;//满血的时候没必要使用道具

            if (_Instance.currentHealth <= 0) { return; }

            ChangeHealth(Player.bet, false);//爱丽丝回复玩家赌注生命值


            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Dealer_Interrupt_Text.text = "体力を回復した";
                    break;

                case 1:
                    // 简体中文
                    Dealer_Interrupt_Text.text = "回复生命值";
                    break;

                case 2:
                    // 繁体中文
                    Dealer_Interrupt_Text.text = "回復生命值";
                    break;

                case 3:
                    // 英语
                    Dealer_Interrupt_Text.text = "HP Restored";
                    break;

                case 4:
                    // 韩语
                    Dealer_Interrupt_Text.text = "체력을 회복했다";
                    break;

            }

            Dealer_Interrupt_Image.sprite = Alice_Milk;
            Sign_Forbidden.SetActive(false);
            Sign_Steal.SetActive(false);
            ShowDealerInterrputAnimator();

            ConsumeOne();//消耗一颗爱心


            swampRate = 0.5f;//爱丽丝的沼泽吞噬盈利
            Sign_SwampRate.SetActive(true);

        }//女荷官回复生命值



        public float swampRate = 1f;   // 1 = 正常   //0.5 玩家收益减半
        public GameObject Sign_SwampRate;

        public float ApplySwampEffect(float winAmount)
        {
            if (currentDealer == DealerType.Alice)
                return winAmount * swampRate;

            return winAmount;
        }//女荷官沼泽吞噬玩家盈利空间

        //----------------------------------------------------------------------------------------------------------------------------------------
        public void Dealer_StealItems(int Item_Image)
        {
            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    // 日语
                    Dealer_Interrupt_Text.text = "アイテムを盗まれた";
                    break;

                case 1:
                    // 简体中文
                    Dealer_Interrupt_Text.text = "偷窃物品";
                    break;

                case 2:
                    // 繁体中文
                    Dealer_Interrupt_Text.text = "偷竊道具";
                    break;

                case 3:
                    // 英语
                    Dealer_Interrupt_Text.text = "Item Stolen";
                    break;

                case 4:
                    // 韩语
                    Dealer_Interrupt_Text.text = "아이템 도난";
                    break;

            }

            Dealer_Interrupt_Image.sprite = List_Item_Image[Item_Image];
            Sign_Steal.SetActive(true);
            ShowDealerInterrputAnimator();

        }//偷窃玩家的道具

        public void Dealer_StealItems_10() 
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            dealer.hand.ChangeScore(5);
            Dealer_StealItems(10);
        }//女荷官偷窃道具黑棋子，给自己＋5

        public void Dealer_StealItems_13()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            player.hand.ChangeScore(5);
            Dealer_StealItems(13);
        }//女荷官偷窃道具白棋子，给玩家＋5

        public void Dealer_StealItems_6()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            dealer.hand.ChangeScore(1);
            Dealer_StealItems(6);
        }//女荷官偷窃道具幸运币，给自己+1

        public void Dealer_StealItems_14()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            dealer.hand.ChangeScore(-1);
            Dealer_StealItems(14);
        }//女荷官偷窃道具厄运币，给自己-1


        public void Dealer_StealItems_0()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            dealer.hand.ChangeScore(-3);
            Dealer_StealItems(0);
        }//女荷官偷窃道具紫色心情，给自己-3

        public void Dealer_StealItems_8()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            dealer.hand.ChangeScore(3);
            Dealer_StealItems(8);
        }//女荷官偷窃道具绿色心情，给自己+3


        public void Dealer_StealItems_4()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            dealer.hand.ChangeScore(dealer.Score);
            Dealer_StealItems(4);
        }//女荷官偷窃道具酒瓶，给自己双倍

        public void Dealer_StealItems_12()
        {
            if (!HasCounter()) return;//女荷官爱心消耗完毕无法使用道具
            player.hand.ChangeScore(player.Score);
            Dealer_StealItems(12);
        }//女荷官偷窃道具空瓶，给玩家双倍

        #endregion


        /// <summary>
        /// 结算画面
        /// </summary>
        #region
        [Header("结算画面")]
        public Image CheckoutScreen_Dealer;
        public Sprite CheckoutScreen_Anto, CheckoutScreen_Hetty, CheckoutScreen_Alice;

        public void ShowResult()
        {
            Turns.text = turns.ToString();
            Revenue.text = revenue.ToString();
            ItemsUsed.text = itemsUsed.ToString();

            if (itemsUsed>=3) 
            {
                UIManager.Achieventment_ACH_CHEAT_BARTENDER();//【作弊酒保】单局内使用3个及以上物品。
            }

            if (revenue >= 1000)
            {
                UIManager.Achieventment_ACH_GAMBLER_BARTENDER();//【赌徒酒保】单局内收益超过1000的情况下战胜女荷官。
            }

            if (revenue <= -1000)
            {
                UIManager.Achieventment_ACH_REVENGE_BARTENDER();//【复仇酒保】单局内亏损超过1000的情况下战胜女荷官。
            }
        }

        public int turns;//一共经过的回合数
        public float revenue;//总共的收益
        public int itemsUsed;//总共使用物品数量
        public Text Turns;
        public Text Revenue;
        public Text ItemsUsed;

        public Text Revenue_2;//展示在金币下方的常驻亏损赚取显示

        #endregion


        /// <summary>
        /// 设置好的快捷键触发
        /// </summary>
        #region

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.A) && _ui.dealButton.interactable == true)
            // {
            //     OnClickDeal();//下注完成或者拿牌
            // }
            // if (Input.GetKeyDown(KeyCode.S) && _ui.standButton.interactable == true)
            // {
            //     OnClickStand();//站牌
            // }
            // if (Input.GetKeyDown(KeyCode.D) && _ui.doubleButton.interactable == true)
            // {
            //     OnClickDouble();//双倍
            // }
            // if (Input.GetKeyDown(KeyCode.F) && _ui.splitButton.interactable == true)
            // {
            //     OnClickSplit();//分牌
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Z) && _ui.undoButton.interactable == true)
            // {
            //     BetHistoryManager._Instance.Undo();
            //     //取消
            // }
            //
            // if (Input.GetKeyDown(KeyCode.X) && _ui.clearButton.interactable == true)
            // {
            //     BetHistoryManager._Instance.ClearHistory();
            //     //清除
            // }


        }
        #endregion
        [Header("赌局中需要隐藏的按钮")]
        public GameObject PauseButton;
        public GameObject LeaveButton;
        private void SetMatchUIVisible(bool visible)
        {
            if (PauseButton != null)
                PauseButton.SetActive(visible);

            if (LeaveButton != null)
                LeaveButton.SetActive(visible);
        }//我刚才又发现了，如果赌局开始，玩家退出，那么还是会产生（下一次进来就会赌注上限混乱）的问题  一旦赌局开始就把暂停菜单和离开按钮去掉，直到赌局结束



        public void Exit_CleanTable() 
        {
            // 赌局开始后不允许退出清桌
            //if (GameActive || State != GameState.OnIdle)
            //{
            //    Debug.Log("赌局已开始，禁止退出清桌");
            //    return;
            //}



            // 1. 如果桌上还有未结算下注，先退钱
            if (Player.bet > 0)
            {
                BalanceManager.ChangeBalance(Player.bet);
                Player.bet = 0;
            }

            // 2. 清下注历史，避免下次 Undo/Clear 还残留
            if (BetHistoryManager._Instance != null)
            {
                BetHistoryManager._Instance.ResetHistory();
            }

            // 3. 清筹码堆显示
            ChipManager.ClearStack(StackType.Standard);
            ChipManager.ClearStack(StackType.Split);
            ChipManager.ClearStack(StackType.Double);
            ChipManager.ClearStack(StackType.DoubleSplit);
            ChipManager.ClearStack(StackType.Insurance);

            // 4. 清桌上的牌
            table.Cleanup();

            // 5. 收尾状态
            GameActive = false;
            ChangeGameState(GameState.OnIdle);
        }
    }
}