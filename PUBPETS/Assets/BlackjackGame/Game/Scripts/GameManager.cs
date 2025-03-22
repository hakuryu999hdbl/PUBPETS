using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.Experimental.GraphView.GraphView;

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
            }
            else
            {
                StartCoroutine(PlayerDeal());
            }
        }

        public void OnClickStand()
        {

            StandPlayerHand();
        }

        public void OnClickDouble()
        {
            Trigger_DoubleDownCheck = true;
            player.DoubleDown();
            StartCoroutine(PlayerDeal());
        }

        public void OnClickSplit()
        {
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

            ChangeView();//游戏结束时触发哪些

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

        public void ChangeView()
        {
            if (SameScore) { dealer.hand.SetScore(player.Score); SameScore = false; }//女荷官强制变成玩家点数
            if (SaveScore&& player.Score>21)
            {
                Debug.Log("【救場：点数超过21，强制削减随机3~5】");
                player.hand.ChangeScore(-Random.Range(3,6)); 
            }// 点数超过21，强制削减随机3~5
            SaveScore = false;

            StartCoroutine(ShowRandomGuestsSequentially());//展示客人骚话

            mainCamera.SetInteger("ChangeView", 2);//摄像头朝向女荷官
            ChangeViewButon.SetActive(true);
            TableAnim.SetInteger("ChangeColor", 1);//桌子强制变淡



            Invoke("StartDialog", 2f);//显示女荷官垃圾话


            player.hand.CheatNumber = 0;//作弊點數清零
        }


        public void ChangeViewBack()
        {
            HideAllGuests();//隐藏客人骚话

            ChangeViewButon.SetActive(false);
            mainCamera.SetInteger("ChangeView", 0);//摄像头转回

            TableAnim.SetInteger("ChangeColor", 0);//桌子强制变回颜色


            // 停止显示女荷官垃圾话对话框
            OverDialog();
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
            int numberToShow = Random.Range(1, Guests.Count + 1);

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

        [Header("女荷官垃圾话列表")]
        public List<GameObject> Diagol = new List<GameObject>();
        private GameObject currentDisplayedDialogue; // 当前显示的对话框



        void StartDialog()
        {
            // 随机选择一个对话框并显示
            int randomIndex = Random.Range(0, Diagol.Count);
            currentDisplayedDialogue = Diagol[randomIndex];
            currentDisplayedDialogue.SetActive(true);
        }

        void OverDialog() 
        {
            foreach (var diagol in Diagol)
            {
                diagol.SetActive(false);
            }
        }

        #endregion


        /// <summary>
        /// 物品栏和筹码栏
        /// </summary>
        #region
        [Header("物品栏和筹码栏")]
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

       //public Animator ItemBox;
       //bool ItemBoxisClosed = true;
       //public void ItemBoxTrigger()
       //{
       //    AudioManager.SoundPlay(1);
       //
       //    ItemBoxisClosed = !ItemBoxisClosed;
       //
       //    if (ItemBoxisClosed)
       //    {
       //        ItemBox.SetInteger("Situation", 0);
       //    }
       //    else
       //    {
       //        ItemBox.SetInteger("Situation", 1);
       //    }
       //}
       
       public void Item_ViewCard() 
       {
           dealer.ConcealCard();
       }//看女荷官的盖牌
       public MeshFilter ShowCard;
       public void Item_ViewNextCard() 
       {
           CardData nextCard = Deck.PeekCard();  // 检视但不抽取下一张牌
           ShowCard.gameObject.SetActive(true);
           ShowCard.mesh = nextCard.GetMesh();
       
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
        }// 强制平局

        bool SaveScore = false;
        public void Item_SaveScore()
        {
            SaveScore = true;
        }// 点数超过21，强制削减随机3~5

        #endregion



        /// <summary>
        /// 关卡与女荷官生命值
        /// </summary>
        #region
        [Header("女荷官生命值")]
        public Text healthText;
        public Image healthFillImage;
        public float maxHealth = 1000f;
        private float currentHealth;

        public Text Lv_Anto;

        void Start()
        {
            Debug.Log("目前储存的关卡进度_安托" + PlayerPrefs.GetInt("Story_Anto"));
            if (PlayerPrefs.GetInt("Story_Anto") <= 0) { PlayerPrefs.SetInt("Story_Anto", 1); }

            //检测安托等级
            Lv_Anto.text = PlayerPrefs.GetInt("Story_Anto").ToString();
            maxHealth = PlayerPrefs.GetInt("Story_Anto") * 1000;

            currentHealth = maxHealth;
            UpdateFill();

        }

        public static void ChangeHealth(float amount)
        {

            _Instance.currentHealth += amount;
            _Instance.currentHealth = Mathf.Clamp(_Instance.currentHealth, 0, _Instance.maxHealth);
            _Instance.UpdateFill();


            if (_Instance.currentHealth<=0) 
            {
                _Instance.Invoke("ShowWINButton", 1f);
            }

        }

        void UpdateFill()
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
            healthText.text = $"{currentHealth} / {maxHealth}";
        }

        public UIManager UIManager;
        public GameObject WinButton;
        void ShowWINButton() 
        {
            WinButton.SetActive(true);
            int currentLV = PlayerPrefs.GetInt("Story_Anto");
            PlayerPrefs.SetInt("Story_Anto", currentLV += 1);
        }
        public void ReLoadScene()
        {
            UIManager.LoadingGame();
        }

        #endregion
    }
}