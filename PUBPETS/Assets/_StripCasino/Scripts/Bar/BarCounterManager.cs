using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using static System.Net.WebRequestMethods;

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


            StartWork();//设定为先开始

            Dealer_Progress();//读取女荷官进度





            RefreshUnlockedRecipesFromSave(); // 读取你已经解锁酒类
            RefreshUnlockedDrinkIcons();// 图标显示对应酒类


    


           

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

            //展示挡板
            Block_Panel.SetActive(true);
            UIManager.instance.SetWait();
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

            //根据存档显示爱丽丝和赫蒂
            Set_Alice_Hetty();

            timeRunning = false;

            //继续
            Time.timeScale = 1;


            //选择女荷官界面BGM
            BGM.instance.Stop();
            BGM.instance.AudioPlayBackgroundMusic(11);//暂时通过这个改变音乐

            StopWorkButton.SetActive(false);
        }



        /// <summary>
        /// 321倒计时
        /// </summary>
        #region
        [Header("透明挡板")]
        public GameObject Block_Panel;//在调酒期间，开始记数的时候把透明挡板展开防止误触

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
            //隐藏挡板
            Block_Panel.SetActive(false);
            UIManager.instance.SetNormal();

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
            if (availableSkins.Count == 0)
                availableSkins = new List<Sprite>(GuestSkin); // 所有皮肤用完后重置池子

            Sprite newSprite = GetRandomSprite(availableSkins);

            switch (LeaveGuestNumber)
            {
                case 1: Guest_1.sprite = newSprite; break;
                case 2: Guest_2.sprite = newSprite; break;
                case 3: Guest_3.sprite = newSprite; break;
                case 4: Guest_4.sprite = newSprite; break;
                case 5: Guest_5.sprite = newSprite; break;
            }

            LeaveGuestNumber++;
            if (LeaveGuestNumber > 5)
                LeaveGuestNumber = 1;
        }


        //public void ChangeLeaveGuestSkin()
        //{
        //    List<Sprite> tempList = new List<Sprite>(GuestSkin);
        //    Sprite newSprite = GetRandomSprite(tempList);
        //
        //    switch (LeaveGuestNumber)
        //    {
        //        case 1:
        //            Guest_1.sprite = newSprite;
        //            break;
        //        case 2:
        //            Guest_2.sprite = newSprite;
        //            break;
        //        case 3:
        //            Guest_3.sprite = newSprite;
        //            break;
        //        case 4:
        //            Guest_4.sprite = newSprite;
        //            break;
        //        case 5:
        //            Guest_5.sprite = newSprite;
        //            break;
        //    }
        //
        //    // 下一位客人将离开
        //    LeaveGuestNumber++;
        //
        //    // 超出就从1重新开始（循环）
        //    if (LeaveGuestNumber > 5)
        //        LeaveGuestNumber = 1;
        //
        //    //StartDialog();//随机抽取对话
        //}

        private Sprite GetRandomSprite(List<Sprite> pool)
        {
            if (pool.Count == 0) return null;

            int index = Random.Range(0, pool.Count);
            Sprite chosen = pool[index];
            pool.RemoveAt(index); // 避免重复
            return chosen;
        }

        private List<Sprite> availableSkins;//31个皮肤的列表，从中随机抽取未抽取过的

        public void InitAllGuestSkin()
        {
            availableSkins = new List<Sprite>(GuestSkin); // 只初始化一次
            Guest_1.sprite = GetRandomSprite(availableSkins);
            Guest_2.sprite = GetRandomSprite(availableSkins);
            Guest_3.sprite = GetRandomSprite(availableSkins);
            Guest_4.sprite = GetRandomSprite(availableSkins);
            Guest_5.sprite = GetRandomSprite(availableSkins);
        }
        //public void InitAllGuestSkin()
        //{
        //    List<Sprite> tempList = new List<Sprite>(GuestSkin); // 克隆可用皮肤列表
        //
        //
        //    Guest_1.sprite = GetRandomSprite(tempList);
        //    Guest_2.sprite = GetRandomSprite(tempList);
        //    Guest_3.sprite = GetRandomSprite(tempList);
        //    Guest_4.sprite = GetRandomSprite(tempList);
        //    Guest_5.sprite = GetRandomSprite(tempList);
        //} // 给 5 位客人各分配一个不重复皮肤


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
        public List<SpecialDrink> specialDrinks;//目前全部的特调酒
        public List<SpecialDrink> unlockedSpecialDrinks = new List<SpecialDrink>();//目前你这个存档已经解锁的酒品


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

        private int currentRandomCount = 0;   // 当前随机酒配方长度(点了一串太过便宜不太好)


        public void GenerateNewCustomer()
        {
            currentIndex = 0;
            currentSpecial = null;

            // 50% 概率是随机饮品，50% 概率是特调饮品
            //isSpecial = Random.Range(0f, 1f) < 0.5f;


            //当玩家的特殊酒越多，出现特殊酒的几率也就越大

            int unlockedCount = unlockedSpecialDrinks.Count;
            int totalCount = specialDrinks.Count;


            //如果没有解锁任何特调饮品，那么不允许出现 isSpecial 
            if (unlockedSpecialDrinks.Count <= 0)
            {
                isSpecial = false;
            }
            else
            {
                //当玩家的特殊酒越多，出现特殊酒的几率也就越大
                // 1种=40%，全解锁=80%
                float specialChance = 0.4f + 0.4f * (unlockedCount - 1) / (float)(totalCount - 1);
                specialChance = Mathf.Clamp01(specialChance);

                isSpecial = Random.value < specialChance;
            }




            if (isSpecial && unlockedSpecialDrinks.Count > 0)
            {
                //currentSpecial = specialDrinks[Random.Range(0, specialDrinks.Count)];
                //currentRecipe = new List<string>(currentSpecial.recipe);

                currentSpecial = unlockedSpecialDrinks[Random.Range(0, unlockedSpecialDrinks.Count)];
                currentRecipe = new List<string>(currentSpecial.recipe);


                Debug.Log("顾客点了：" + currentSpecial.name);

                switch (currentSpecial.name)
                {
                    case "龙焰酒":
                        currentDisplayedDialogue = Diagol[1];
                        break;
                    case "魔女之吻":
                        currentDisplayedDialogue = Diagol[2];
                        break;
                    case "精灵树蜂蜜酒":
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
                    case "太阳果皮酒":
                        currentDisplayedDialogue = Diagol[8];
                        break;
                    case "森之果酒":
                        currentDisplayedDialogue = Diagol[9];
                        break;
                    case "宵之玫瑰酒":
                        currentDisplayedDialogue = Diagol[10];
                        break;
                }
                currentDisplayedDialogue.SetActive(true);

            }
            else
            {
                int count = Random.Range(2, 6); 
                
                currentRandomCount = count;   // ✅ 记录普通酒的复杂度

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
                            case "龙焰酒":
                                ShakerPlane.SetTrigger("Wine_1");
                                break;
                            case "魔女之吻":
                                ShakerPlane.SetTrigger("Wine_2");
                                break;
                            case "精灵树蜂蜜酒":
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
                            case "太阳果皮酒":
                                ShakerPlane.SetTrigger("Wine_8");
                                break;
                            case "森之果酒":
                                ShakerPlane.SetTrigger("Wine_9");
                                break;
                            case "宵之玫瑰酒":
                                ShakerPlane.SetTrigger("Wine_10");
                                break;
                        }
                    }
                    else
                    {
                        switch (Random.Range(11, 21))
                        {

                            case 11:
                                ShakerPlane.SetTrigger("Wine_11");
                                break;
                            case 12:
                                ShakerPlane.SetTrigger("Wine_12");
                                break;
                            case 13:
                                ShakerPlane.SetTrigger("Wine_13");
                                break;
                            case 14:
                                ShakerPlane.SetTrigger("Wine_14");
                                break;
                            case 15:
                                ShakerPlane.SetTrigger("Wine_15");
                                break;
                            case 16:
                                ShakerPlane.SetTrigger("Wine_16");
                                break;
                            case 17:
                                ShakerPlane.SetTrigger("Wine_17");
                                break;
                            case 18:
                                ShakerPlane.SetTrigger("Wine_18");
                                break;
                            case 19:
                                ShakerPlane.SetTrigger("Wine_19");
                                break;
                            case 20:
                                ShakerPlane.SetTrigger("Wine_20");
                                break;

                        }
                    }

                    timeRunning = false;//暂时暂停计时

                    //展示挡板
                    Block_Panel.SetActive(true);
                    UIManager.instance.SetWait();
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

            int reward;

            if (currentSpecial != null)
            {
                reward = currentSpecial.price; // 特调固定收益
            }
            else
            {
                // ✅ 随机酒：按复杂度小幅提升
                int c = Mathf.Clamp(currentRandomCount, 2, 5);

                int baseMin = 60;
                int baseMax = 80;
                int stepBonus = (c - 2) * 15;   // 2步=0, 3步=+15, 4步=+30, 5步=+45

                reward = Random.Range(baseMin + stepBonus, baseMax + stepBonus + 1);
            }

            BalanceManager.ChangeBalance(reward);
            AddGuest(reward);//营收记录
            startText.gameObject.SetActive(true);
            startText.text = reward.ToString();//营收数字显示

            GenerateNewCustomer();
            Guest_Move();//下一位客人

            AudioManager_2.SoundPlay(3);//手动SE音频替换

            timeRunning = true;//继续计时

            //隐藏挡板
            Block_Panel.SetActive(false);
            UIManager.instance.SetNormal();

            //普通酒复杂度清零
            currentRandomCount = 0;
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


        [Header("已解锁特调酒图标（按 specialDrinks 顺序对齐）")]
        public List<GameObject> unlockedDrinkIcons; // size=7，对应 Wine_1~Wine_7
        public GameObject unlockedDrinkList;//显示和不显示这个列表

        public void RefreshUnlockedRecipesFromSave()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            // 防御：避免 null
            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();

            unlockedSpecialDrinks = specialDrinks
                .Where(d => data.unlockedDrinkNames.Contains(d.name))
                .ToList();

            Debug.Log($"已解锁特调数量：{unlockedSpecialDrinks.Count}");
        }//存档内已经解锁的酒类

        public void RefreshUnlockedDrinkIcons()
        {
            // 先全部隐藏
            for (int i = 0; i < unlockedDrinkIcons.Count; i++)
                unlockedDrinkIcons[i].SetActive(false);

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();

            // 点亮已解锁
            for (int i = 0; i < specialDrinks.Count; i++)
            {
                var drink = specialDrinks[i];
                if (data.unlockedDrinkNames.Contains(drink.name))
                {
                    if (i >= 0 && i < unlockedDrinkIcons.Count)
                        unlockedDrinkIcons[i].SetActive(true);
                    else
                        Debug.LogWarning($"图标列表数量不足：special index={i}");
                }
            }
        }//显示存档内已经有的酒类



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

            //隐藏列表
            unlockedDrinkList.SetActive(false);


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
        [Header("女荷官AVG随机入口")]
        public GameObject Alice;
        public GameObject Hetty;
        public void Set_Alice_Hetty()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.antoProgress >= 3)
            {
                Alice.SetActive(true);
                Hetty.SetActive(true);
                NoAVG_Object[1].SetActive(false);//爱丽丝未解锁
                NoAVG_Object[2].SetActive(false);//赫蒂未解锁
            }
        }



        public GameObject SelectStage_Anto;//通关之后，玩家选择
        public GameObject SelectStage_Hetty;//通关之后，玩家选择
        public GameObject SelectStage_Alice;//通关之后，玩家选择


        public void Load_Vs_Anto_AVG()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.antoProgress>=11)
            {
                SelectStage_Anto.SetActive(true);//通关后选择关卡
            }
            else
            {
                GameFlowData.nextAVGId = "VSAnto";//确定女荷官
                uiManager.LoadingScene_Spine();
            }
         

        }
        public void Menu_SelectStage_Anto(int Lv) 
        {
            switch (Lv)
            {
                case 1:
                    GameFlowData.nextAVGId = "Anto_CG_01_1";//开启安托第一个CG前端AVG
                    break;
                case 2:
                    GameFlowData.nextAVGId = "Anto_CG_02_1";//开启安托第二个CG前端AVG
                    break;
                case 3:
                    GameFlowData.nextAVGId = "Anto_CG_03_1";//开启安托第三个CG前端AVG
                    break;
                case 4:
                    GameFlowData.nextAVGId = "Anto_CG_04_1";//开启安托第四个CG前端AVG
                    break;
                case 5:
                    GameFlowData.nextAVGId = "Anto_CG_05_1";//开启安托第五个CG前端AVG
                    break;
                case 6:
                    GameFlowData.nextAVGId = "Anto_CG_06_1";//开启安托第六个CG前端AVG
                    break;
                case 7:
                    GameFlowData.nextAVGId = "Anto_CG_07_1";//开启安托第七个CG前端AVG
                    break;
                case 8:
                    GameFlowData.nextAVGId = "Anto_CG_08_1";//开启安托第八个CG前端AVG
                    break;
                case 9:
                    GameFlowData.nextAVGId = "Anto_CG_09_1";//开启安托第九个CG前端AVG
                    break;
                case 10:
                    GameFlowData.nextAVGId = "Anto_CG_10_1";//开启安托第十个CG前端AVG
                    break;
            }

            uiManager.LoadingScene_Spine();
        }//通关后的关卡选择界面
        public void Load_Vs_Hetty_AVG()
        {

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.hettyProgress >= 11)
            {
                SelectStage_Hetty.SetActive(true);//通关后选择关卡
            }
            else
            {
                GameFlowData.nextAVGId = "VSHetty";//开启开头剧情介绍
                uiManager.LoadingScene_Spine();
            }

        }
        public void Menu_SelectStage_Hetty(int Lv)
        {
            switch (Lv)
            {
                case 1:
                    GameFlowData.nextAVGId = "Hetty_CG_01_1";//开启赫蒂第一个CG前端AVG
                    break;
                case 2:
                    GameFlowData.nextAVGId = "Hetty_CG_02_1";//开启赫蒂第二个CG前端AVG
                    break;
                case 3:
                    GameFlowData.nextAVGId = "Hetty_CG_03_1";//开启赫蒂第三个CG前端AVG
                    break;
                case 4:
                    GameFlowData.nextAVGId = "Hetty_CG_04_1";//开启赫蒂第四个CG前端AVG
                    break;
                case 5:
                    GameFlowData.nextAVGId = "Hetty_CG_05_1";//开启赫蒂第五个CG前端AVG
                    break;
                case 6:
                    GameFlowData.nextAVGId = "Hetty_CG_06_1";//开启赫蒂第六个CG前端AVG
                    break;
                case 7:
                    GameFlowData.nextAVGId = "Hetty_CG_07_1";//开启赫蒂第七个CG前端AVG
                    break;
                case 8:
                    GameFlowData.nextAVGId = "Hetty_CG_08_1";//开启赫蒂第八个CG前端AVG
                    break;
                case 9:
                    GameFlowData.nextAVGId = "Hetty_CG_09_1";//开启赫蒂第九个CG前端AVG
                    break;
                case 10:
                    GameFlowData.nextAVGId = "Hetty_CG_10_1";//开启赫蒂第十个CG前端AVG
                    break;
            }

            uiManager.LoadingScene_Spine();
        }//通关后的关卡选择界面
        public void Load_Vs_Alice_AVG()
        {

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.aliceProgress >= 11)
            {
                SelectStage_Alice.SetActive(true);//通关后选择关卡
            }
            else
            {
                GameFlowData.nextAVGId = "VSAlice";//确定女荷官
                uiManager.LoadingScene_Spine();
            }


        }
        public void Menu_SelectStage_Alice(int Lv)
        {
            switch (Lv)
            {
                case 1:
                    GameFlowData.nextAVGId = "Alice_CG_01_1";//开启爱丽丝第一个CG前端AVG
                    break;
                case 2:
                    GameFlowData.nextAVGId = "Alice_CG_02_1";//开启爱丽丝第二个CG前端AVG
                    break;
                case 3:
                    GameFlowData.nextAVGId = "Alice_CG_03_1";//开启爱丽丝第三个CG前端AVG
                    break;
                case 4:
                    GameFlowData.nextAVGId = "Alice_CG_04_1";//开启爱丽丝第四个CG前端AVG
                    break;
                case 5:
                    GameFlowData.nextAVGId = "Alice_CG_05_1";//开启爱丽丝第五个CG前端AVG
                    break;
                case 6:
                    GameFlowData.nextAVGId = "Alice_CG_06_1";//开启爱丽丝第六个CG前端AVG
                    break;
                case 7:
                    GameFlowData.nextAVGId = "Alice_CG_07_1";//开启爱丽丝第七个CG前端AVG
                    break;
                case 8:
                    GameFlowData.nextAVGId = "Alice_CG_08_1";//开启爱丽丝第八个CG前端AVG
                    break;
                case 9:
                    GameFlowData.nextAVGId = "Alice_CG_09_1";//开启爱丽丝第九个CG前端AVG
                    break;
                case 10:
                    GameFlowData.nextAVGId = "Alice_CG_10_1";//开启爱丽丝第十个CG前端AVG
                    break;
            }

            uiManager.LoadingScene_Spine();
        }//通关后的关卡选择界面

        public void Hide()
        {
            AudioManager_2.SoundPlay(4);
        }//未解锁的角色通用
        #endregion


        /// <summary>
        /// 女荷官按钮上显示当前进度
        /// </summary>
        #region

        public Image FillImage_Anto, FillImage_Hetty, FillImage_Alice;

        public void Dealer_Progress()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            FillImage_Anto.fillAmount = data.antoProgress / 11f;
            FillImage_Hetty.fillAmount = data.hettyProgress / 11f;
            FillImage_Alice.fillAmount = data.aliceProgress / 11f;


        }
        #endregion


       
    }
}