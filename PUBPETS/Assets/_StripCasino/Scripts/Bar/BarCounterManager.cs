using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Blackjack_Game
{
    public class BarCounterManager : MonoBehaviour
    {
        [Header("摄像头/客人动画器")]
        public Animator mainCamera;
        public Animator Queues_Guest;

        public UIManager uiManager;

        public GameObject StopWorkButton;//打烊按钮

        void Start()
        {
            //mainCamera.SetInteger("ChangeView", 2);//摄像头朝向女荷官

            if (PlayerPrefs.GetInt("Story") == 0)
            {
                uiManager.Load_AVG(1);//介绍男主背景
                PlayerPrefs.SetInt("Story", 1);//记录（下次进来不会出现这个介绍背景剧情）
            }
            else 
            {
                uiManager.Load_AVG(100);//开启新的一天
            }
            

            //StartWork();//设定为先开始
        }

        public List<GameObject> NoAVG_Object;//游戏开始或者AVG画面不需要别的按钮

        public void StartWork() 
        {
            Work.SetActive(true);//展示需要沙威玛界面的全部要素
            Guests.SetActive(true);//展示所有客人

            InitAllGuestSkin(); // 游戏一开始，初始化 5 个皮肤
            StartCoroutine(StartCountdown());//开启营业

            currentTime = totalTime;//计时充能

            //酒保工作BGM
            BGM.instance.Stop();
            BGM.instance.AudioPlayBackgroundMusic(1);//暂时通过这个改变音乐


            //非桌子上的按钮什么的全部去掉
            foreach (var NoAVG in NoAVG_Object)
            {
                NoAVG.SetActive(false);
            }

            StopWorkButton.SetActive(true);
        }

        public void StopWork() 
        {
            Work.SetActive(false);//展示需要沙威玛界面的全部要素
            Guests.SetActive(false);//展示所有客人

            //非桌子上的按钮什么的全部去掉
            foreach (var NoAVG in NoAVG_Object)
            {
                NoAVG.SetActive(true);
            }

            timeRunning = false;

            //继续
            Time.timeScale = 1;


            //选择女荷官界面BGM
            BGM.instance.Stop();
            BGM.instance.AudioPlayBackgroundMusic(3);//暂时通过这个改变音乐

            StopWorkButton.SetActive(false);
        }

        /// <summary>
        /// 321倒计时
        /// </summary>
        #region
        [Header("321倒计时")]
        public TMP_Text startText;

        IEnumerator StartCountdown()
        {
            startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(1);//手动SE音频替换
            startText.text = "3"; yield return new WaitForSeconds(1.2f);
            startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(1);//手动SE音频替换
            startText.text = "2"; yield return new WaitForSeconds(1.2f);
            startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(1);//手动SE音频替换
            startText.text = "1"; yield return new WaitForSeconds(1.2f);
            startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(0);//手动SE音频替换
            startText.text = "Go!"; yield return new WaitForSeconds(1.2f);
            startText.text = "";

            StartGame();
        }

        void StartGame()
        {


            GenerateNewCustomer();//顾客提要求（生成配方）

            timeRunning = true;//计时开始
        }

        #endregion

        /// <summary>
        /// 客人逐步上前
        /// </summary>
        #region
        int LeaveGuestNumber = 1;
        public void Guest_Move()
        {
         

            Queues_Guest.SetTrigger("Move");
            Debug.Log("Move");


        }

        [Header("客人逐步上前")]
        public List<Sprite> GuestSkin;
        public SpriteRenderer Guest_1, Guest_2, Guest_3, Guest_4, Guest_5;
        public void ChangeLeaveGuestSkin()
        {
            List<Sprite> tempList = new List<Sprite>(GuestSkin);
            Sprite newSprite = GetRandomSprite(tempList);

            switch (LeaveGuestNumber)
            {
                case 1:
                    Guest_1.sprite = newSprite;
                    break;
                case 2:
                    Guest_2.sprite = newSprite;
                    break;
                case 3:
                    Guest_3.sprite = newSprite;
                    break;
                case 4:
                    Guest_4.sprite = newSprite;
                    break;
                case 5:
                    Guest_5.sprite = newSprite;
                    break;
            }

            // 下一位客人将离开
            LeaveGuestNumber++;

            // 超出就从1重新开始（循环）
            if (LeaveGuestNumber > 5)
                LeaveGuestNumber = 1;

            //StartDialog();//随机抽取对话
        }

        private Sprite GetRandomSprite(List<Sprite> pool)
        {
            if (pool.Count == 0) return null;

            int index = Random.Range(0, pool.Count);
            Sprite chosen = pool[index];
            pool.RemoveAt(index); // 避免重复
            return chosen;
        }


        public void InitAllGuestSkin()
        {
            List<Sprite> tempList = new List<Sprite>(GuestSkin); // 克隆可用皮肤列表


            Guest_1.sprite = GetRandomSprite(tempList);
            Guest_2.sprite = GetRandomSprite(tempList);
            Guest_3.sprite = GetRandomSprite(tempList);
            Guest_4.sprite = GetRandomSprite(tempList);
            Guest_5.sprite = GetRandomSprite(tempList);
        } // 给 5 位客人各分配一个不重复皮肤


        #endregion

        /// <summary>
        /// 随机显示顾客要求
        /// </summary>
        #region

        [Header("顾客要求列表")]
        public List<GameObject> Diagol = new List<GameObject>();
        private GameObject currentDisplayedDialogue; // 当前显示的对话框

        void OverDialog()
        {
            foreach (var diagol in Diagol)
            {
                diagol.SetActive(false);
            }
        }// 关闭所有对话框

        #endregion




        /// <summary>
        /// 按顺序点击物品
        /// </summary>
        #region
        [Header("按顺序点击物品")]
        public GameObject Work;
        public GameObject Guests;

        [System.Serializable]
        public class SpecialDrink
        {
            public string name;           // 显示名，例如“魔女之吻”
            public List<string> recipe;   // 固定顺序配料ID，例如 { "Wine_2", "Lemon", "Honey" }
            public int price;             // 完成这杯酒后的奖励金
        }
        [Header("特调酒")]
        public List<SpecialDrink> specialDrinks;
        private SpecialDrink currentSpecial = null;//是否是随机酒

        [System.Serializable]
        public class DrinkIngredient
        {
            public string id;
            public Sprite icon;
            public GameObject button;
        }

        public List<DrinkIngredient> allIngredients;
        public Transform hintPanel; // 显示提示栏图标的父物体
        public GameObject hintIconPrefab;

        private List<string> currentRecipe = new List<string>();
        private int currentIndex = 0;
        bool isSpecial;//是否是特制酒
        public void GenerateNewCustomer()
        {
            currentIndex = 0;
            currentSpecial = null;

            // 50% 概率是随机饮品，50% 概率是特调饮品
            isSpecial = Random.Range(0f, 1f) < 0.5f;

            if (isSpecial && specialDrinks.Count > 0)
            {
                currentSpecial = specialDrinks[Random.Range(0, specialDrinks.Count)];
                currentRecipe = new List<string>(currentSpecial.recipe);
                Debug.Log("顾客点了：" + currentSpecial.name);

                switch (currentSpecial.name)
                {
                    case "龙炎酒":
                        currentDisplayedDialogue = Diagol[1];
                        break;
                    case "魔女之吻":
                        currentDisplayedDialogue = Diagol[2];
                        break;
                    case "精灵树蜂蜜":
                        currentDisplayedDialogue = Diagol[3];
                        break;
                    case "冰结之息":
                        currentDisplayedDialogue = Diagol[4];
                        break;
                    case "秘法红石酒":
                        currentDisplayedDialogue = Diagol[5];
                        break;
                    case "雾花酒":
                        currentDisplayedDialogue = Diagol[6];
                        break;
                    case "狼毒酒":
                        currentDisplayedDialogue = Diagol[7];
                        break;
                }
                currentDisplayedDialogue.SetActive(true);

            }
            else
            {
                int count = Random.Range(2, 6);
                currentRecipe = allIngredients
                    .OrderBy(x => Random.value)
                    .Take(count)
                    .Select(i => i.id)
                    .ToList();
                Debug.Log("顾客：随便来点啥");

                currentDisplayedDialogue = Diagol[0];
                currentDisplayedDialogue.SetActive(true);
            }

            //currentIndex = 0;

            // 清空提示栏 UI
            foreach (Transform child in hintPanel)
                Destroy(child.gameObject);

            // 生成新提示栏
            foreach (string id in currentRecipe)
            {
                var ingredient = allIngredients.Find(i => i.id == id);
                if (ingredient == null || ingredient.icon == null)
                {
                    Debug.LogWarning($"未找到或未设置 icon：{id}");
                    continue;
                }

                var icon = Instantiate(hintIconPrefab, hintPanel);
                icon.GetComponent<Image>().sprite = ingredient.icon;
            }
        }


        // 这个函数绑定到每个物品按钮上
        public void OnClickIngredient(string id)
        {
            if (!timeRunning) { return; }//计时开始钱戳物品

            if (id == currentRecipe[currentIndex])
            {
                // 正确 → 隐藏当前提示图标
                hintPanel.GetChild(currentIndex).gameObject.SetActive(false);
                currentIndex++;

                if (currentIndex >= currentRecipe.Count)
                {
                    if (isSpecial)
                    {
                        //播放酒好了动画
                        switch (currentSpecial.name)
                        {
                            case "龙炎酒":
                                ShakerPlane.SetTrigger("Wine_1");
                                break;
                            case "魔女之吻":
                                ShakerPlane.SetTrigger("Wine_2");
                                break;
                            case "精灵树蜂蜜":
                                ShakerPlane.SetTrigger("Wine_3");
                                break;
                            case "冰结之息":
                                ShakerPlane.SetTrigger("Wine_4");
                                break;
                            case "秘法红石酒":
                                ShakerPlane.SetTrigger("Wine_5");
                                break;
                            case "雾花酒":
                                ShakerPlane.SetTrigger("Wine_6");
                                break;
                            case "狼毒酒":
                                ShakerPlane.SetTrigger("Wine_7");
                                break;

                        }
                    }
                    else 
                    {
                        switch (Random.Range(8,14))
                        {
                            case 8:
                                ShakerPlane.SetTrigger("Wine_8");
                                break;
                            case 9:
                                ShakerPlane.SetTrigger("Wine_9");
                                break;
                            case 10:
                                ShakerPlane.SetTrigger("Wine_10");
                                break;
                            case 11:
                                ShakerPlane.SetTrigger("Wine_11");
                                break;
                            case 12:
                                ShakerPlane.SetTrigger("Wine_12");
                                break;
                            case 13:
                                ShakerPlane.SetTrigger("Wine_13");
                                break;

                        }
                    }

                    timeRunning = false;//暂时暂停计时
                }

                AudioManager_2.SoundPlay(5);//手动SE音频替换

            }
            else
            {
                Debug.Log("按错了，重头来！");
                //GenerateNewCustomer(); // 重置

                AudioManager_2.SoundPlay(4);//手动SE音频替换
            }
        }




        public Animator ShakerPlane;

        //酒完成动画调用
        public void MakeWineSuccess() 
        {
            OverDialog();//目前要求消失

            Debug.Log("调酒成功！");
            int reward = currentSpecial != null ? currentSpecial.price : 100;
            BalanceManager.ChangeBalance(reward);
            AddGuest(reward);//营收记录
            startText.gameObject.SetActive(true);
            startText.text = reward.ToString();//营收数字显示

            GenerateNewCustomer();
            Guest_Move();//下一位客人

            AudioManager_2.SoundPlay(3);//手动SE音频替换

            timeRunning = true;//继续计时
        }


        //在点击暂停营业的时候，外部调用

        public void timeRunningFalse() 
        {
            //timeRunning = false;//计时暂停

            Time.timeScale = 0f;
        }

        public void timeRunningTrue()
        {
            //timeRunning = true;//继续计时
            Time.timeScale = 1f;
        }


        #endregion

        /// <summary>
        /// 倒计时
        /// </summary>
        #region
        [Header("倒计时设定")]
        float totalTime = 30f; // 一天营业时长（秒）
        private float currentTime;

        [Header("UI元素")]
        public Text countdownText;
        public Image countdownBar;

        [Header("营业结果面板")]
        public GameObject timeUpPanel;
        public Text guestCountText;
        public Text revenueText;

        [Header("数据统计")]
        public int guestCount = 0;
        public int revenue = 0;

        private bool timeRunning = false;
        void Update()
        {
            if (!timeRunning) return;

            currentTime -= Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0, totalTime);

            // 更新UI
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            int milliseconds = Mathf.FloorToInt((currentTime % 1f) * 100f);
            countdownText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

            countdownBar.fillAmount = currentTime / totalTime;

            if (currentTime <= 0)
            {
                TimeUp();
            }
        }

        public void TimeUp()
        {
            timeRunning = false;

            // 显示结束面板
            timeUpPanel.SetActive(true);
            guestCountText.text = guestCount.ToString();
            revenueText.text = revenue.ToString();

            // 可选：暂停游戏等
            //Time.timeScale = 0;
        }

        // 调用这个方法当顾客完成调酒后记录
        public void AddGuest(int reward)
        {
            guestCount++;
            revenue += reward;
        }
        #endregion



        /// <summary>
        /// 女荷官AVG随机入口
        /// </summary>
        #region
        public void Load_Vs_Anto_AVG()
        {

            switch (Random.Range(1, 11))
            {
                case 1:
                    uiManager.Load_AVG(1011);
                    break;
                case 2:
                    uiManager.Load_AVG(1021);
                    break;
                case 3:
                    uiManager.Load_AVG(1031);
                    break;
                case 4:
                    uiManager.Load_AVG(1041);
                    break;
                case 5:
                    uiManager.Load_AVG(1051);
                    break;
                case 6:
                    uiManager.Load_AVG(1061);
                    break;
                case 7:
                    uiManager.Load_AVG(1071);
                    break;
                case 8:
                    uiManager.Load_AVG(1081);
                    break;
                case 9:
                    uiManager.Load_AVG(1091);
                    break;
                case 10:
                    uiManager.Load_AVG(1101);
                    break;
            }

        }
        #endregion
    }
}