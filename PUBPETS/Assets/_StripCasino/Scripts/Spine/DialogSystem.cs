using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Blackjack_Game
{
    public class DialogSystem : MonoBehaviour
    {
        [Header("UI组件")]
        public Text textLabel;

        public List<GameObject> NameObject = new List<GameObject>();


        private Dictionary<int, TextAsset> textAssets = new Dictionary<int, TextAsset>();


        public int index;
        public float textSpeed;
        bool textFinished;//是否完成打字
        bool cancelTyping;//取消打字
        List<string> textList = new List<string>();

        [Header("这是哪个动画需要的对话")]
        public int animation_number;

        [Header("对话，背景，角色")]
        public GameObject TextButton;

        public Image Anto_1,Anto_2;
        public Image ShopManager;

        public Image Background;
        public Sprite BarCounter, History_01, History_02, History_03, Shop_Background;

        private void OnEnable()
        {
            //读取textSpeed

            textSpeed = PlayerPrefs.GetFloat("TextSpeed");

            Invoke("Read", 0.1f);

        }//一开始不会产生空白，OnEnable会在Start之前，Awake之后被调用

        public void ForceEndDialogue()
        {
            // 清除当前对话状态
            textList.Clear();
            index = 0;

            // 设置 textFinished 为 true，以便退出正在进行的协程
            textFinished = true;

            // 将对话系统 UI 隐藏
            gameObject.SetActive(false);

            //Debug.Log("对话已强制结束并重置");


        }//强制关闭对话

        void Read()
        {
            // Clear the existing dictionary to avoid key conflicts
            textAssets.Clear();

            switch (PlayerPrefs.GetInt("language"))
            {
                case 0:
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Japanese/J_StartStory_01"));

                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Japanese/J_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Japanese/J_StartShop_02"));

                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Japanese/J_StartWork_01"));

                    textAssets.Add(101, Resources.Load<TextAsset>("TXT_Japanese/J_Chat_Anto_01"));
                    textAssets.Add(111, Resources.Load<TextAsset>("TXT_Japanese/J_Failure_Anto_01"));

                    textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Japanese/J_CG_01"));
                    textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Japanese/J_CG_02"));
                    break;
                case 1:
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartStory_01"));

                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartShop_02"));

                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartWork_01"));

                    textAssets.Add(101, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_Chat_Anto_01"));
                    textAssets.Add(111, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_Failure_Anto_01"));

                    textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_CG_01"));
                    textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_CG_02"));
                    break;
                case 2:
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartStory_01"));

                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartShop_02"));

                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartWork_01"));

                    textAssets.Add(101, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_Chat_Anto_01"));
                    textAssets.Add(111, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_Failure_Anto_01"));

                    textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_CG_01"));
                    textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_CG_02"));
                    break;
                case 3:
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_English/E_StartStory_01"));

                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_English/E_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_English/E_StartShop_02"));

                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_English/E_StartWork_01"));

                    textAssets.Add(101, Resources.Load<TextAsset>("TXT_English/E_Chat_Anto_01"));
                    textAssets.Add(111, Resources.Load<TextAsset>("TXT_English/E_Failure_Anto_01"));

                    textAssets.Add(1001, Resources.Load<TextAsset>("TXT_English/E_CG_01"));
                    textAssets.Add(1002, Resources.Load<TextAsset>("TXT_English/E_CG_02"));
                    break;
                case 4:
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Korean/K_StartStory_01"));

                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Korean/K_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Korean/K_StartShop_02"));

                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Korean/K_StartWork_01"));

                    textAssets.Add(101, Resources.Load<TextAsset>("TXT_Korean/K_Chat_Anto_01"));
                    textAssets.Add(111, Resources.Load<TextAsset>("TXT_Korean/K_Failure_Anto_01"));

                    textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Korean/K_CG_01"));
                    textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Korean/K_CG_02"));
                    break;
            }






            // 使用字典查找相应的 TextAsset
            if (textAssets.TryGetValue(animation_number, out TextAsset selectedText))
            {
                GetTextFormFile(selectedText);
            }
            else
            {
                Debug.LogError("No TextAsset found for animation_number: " + animation_number);
            }

            textFinished = true;
            StartCoroutine(SetTextUI());
        }

        public void ShowText()
        {
            if (textFinished && !cancelTyping)
            {
                if (index >= textList.Count) // 添加边界检查
                {
                    gameObject.SetActive(false);
                    index = 0;

                    ChangeStory();//结束位置触发
                    Debug.Log("对话已结束");
                    return;
                }

                if (gameObject.activeSelf)
                {
                    StartCoroutine(SetTextUI());
                }
            }
            else if (!textFinished)
            {
                cancelTyping = !cancelTyping;
            }

        }

        void GetTextFormFile(TextAsset file)
        {
            textList.Clear(); index = 0;//首先将列表内的字符清空

            var lineDate = file.text.Split('\n');//以回车切割每一段

            foreach (var line in lineDate)
            {
                textList.Add(line);
            }
        }

        IEnumerator SetTextUI()
        {
            if (index >= textList.Count)
            {
                Debug.LogWarning("index 超出 textList 范围");
                yield break;
            }

            textFinished = false;
            textLabel.text = "";

            //判断一整行的字符是
            Text text = textLabel;
            switch (textList[index].Trim().ToString())
            {
                //字的颜色
                case "BG":
                    text.color = Color.white;
                    CleanNameText();

                    Anto_1.color = new Color(0.5f, 0.5f, 0.5f, 1f); //变暗
                    Anto_2.color = new Color(0.5f, 0.5f, 0.5f, 1f); //变暗

                    Background.sprite = BarCounter;// 默认是酒馆背景

                    index++;
                    break;


                case "Shop":
                    text.color = Color.white;
                    CleanNameText();

                    Anto_2.gameObject.SetActive(false);

                    Background.sprite = Shop_Background;// 默认是酒馆背景

                    index++;
                    break;



                case "Me":
                    text.color = new Color(0.0f, 0.68f, 0.93f, 1.0f);//蓝色
                    CleanNameText();
                    NameObject[0].SetActive(true);

                    Anto_1.color = new Color(0.5f, 0.5f, 0.5f, 1f); //变暗
                    Anto_2.color = new Color(0.5f, 0.5f, 0.5f, 1f); //变暗
                    ShopManager.color = new Color(0.5f, 0.5f, 0.5f, 1f); //变暗

                    index++;
                    break;






                case "ShopManager":
                    text.color = new Color(0.7f, 0.75f, 0.8f, 1.0f); // 亮灰色
                    CleanNameText();
                    NameObject[4].SetActive(true);

                    ShopManager.gameObject.SetActive(true);
                    ShopManager.color = new Color(1f, 1f, 1f, 1f); //说话变亮
                    index++;
                    break;

                case "Anto_1":
                    text.color = new Color(1.0f, 0.2f, 0.5f, 1.0f); //浅红色
                    CleanNameText();
                    NameObject[1].SetActive(true);

                    Anto_1.gameObject.SetActive(true);
                    Anto_1.color = new Color(1f, 1f, 1f, 1f); //说话变亮

                    index++;
                    break;


                case "Anto_2":
                    text.color = new Color(1.0f, 0.2f, 0.5f, 1.0f); //浅红色
                    CleanNameText();
                    NameObject[1].SetActive(true);

                    Anto_2.gameObject.SetActive(true);
                    Anto_2.color = new Color(1f, 1f, 1f, 1f); //说话变亮

                    index++;
                    break;



                case "Hetty":
                    text.color = Color.white;
                    CleanNameText();
                    NameObject[2].SetActive(true);
                    index++;
                    break;
                case "Alice":
                    text.color = Color.white;
                    CleanNameText();
                    NameObject[3].SetActive(true);
                    index++;
                    break;




                case "History_01":
                    text.color = Color.white;
                    CleanNameText();
                    Background.sprite = History_01;
                    index++;
                    break;
                case "History_02":
                    text.color = Color.white;
                    Background.sprite = History_02;
                    index++;
                    break;
                case "History_03":
                    text.color = Color.white;
                    Background.sprite = History_03;
                    index++;
                    break;



                    //case "Girl":
                    //    text.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);//粉色
                    //    index++;
                    //    break;

                    // case "MAN":
                    //     text.color = new Color(0.0f, 0.68f, 0.93f, 1.0f);//蓝色(市民群众)
                    //     index++;
                    //     break;
                    // case "DarkRed":
                    //     text.color = new Color(0.8f, 0.2f, 0.2f, 1.0f); // 深红色（女特工）
                    //     index++;
                    //     break;
                    // case "LightRed":
                    //     text.color = new Color(1.0f, 0.2f, 0.5f, 1.0f); //浅红色(菲西莉亚)
                    //     index++;
                    //     break;
                    // case "Green":
                    //     text.color = new Color(0.0f, 1.0f, 0.0f, 1.0f); // 绿色（魔族女干部）
                    //     index++;
                    //     break;
                    // case "LightBlue":
                    //     text.color = new Color(0.68f, 0.85f, 0.9f, 1.0f); // 浅蓝色（艾莉丝）
                    //     index++;
                    //     break;
                    // case "Gold":
                    //     text.color = new Color(1.0f, 0.84f, 0.0f, 1.0f); // 金色（战姬大队长）
                    //     index++;
                    //     break;
                    // case "Yellow":
                    //     text.color = new Color(1.0f, 1.0f, 0.0f, 1.0f); // 黄色（莱拉）
                    //     index++;
                    //     break;
                    // case "Orange":
                    //     text.color = new Color(1.0f, 0.5f, 0.0f, 1.0f); // 橙色(母体)
                    //     index++;
                    //     break;
                    // case "Purple":
                    //     text.color = new Color(0.7f, 0.3f, 0.7f, 1.0f); // 紫色 (女记者)
                    //     index++;
                    //     break;
                    // case "Gray":
                    //     text.color = new Color(0.7f, 0.75f, 0.8f, 1.0f); // 亮灰色(牧者)
                    //     index++;
                    //     break;



                    //case "Over":
                    //    ChangeStory();//通常对话结束
                    //    index++;
                    //    break;
                    //
                    //
                    //case "ReStart":
                    //    //Spine_FrameEvents.ReStart();//教程结束回主菜单
                    //    index++;
                    //    break;





            }


            int letter = 0;
            while (!cancelTyping && letter < textList[index].Length - 1)
            {
                textLabel.text += textList[index][letter];
                letter++;
                yield return new WaitForSeconds(textSpeed);
            }

            textLabel.text = textList[index];
            cancelTyping = false;
            textFinished = true;
            index++;
        }

        public void CleanNameText()
        {
            foreach (var nameObject in NameObject)
            {
                nameObject.SetActive(false);
            }
        }

        [Header("当对话结束时需要触发的一些地方")]
        public UIManager uiManager;
        public GameObject ShopPlane;//商店本体
        //快进按钮触发在这里
        public void ChangeStory()
        {

            switch (animation_number) 
            {
                case 1:
                    uiManager.Load_AVG(100);//开启新的一天
                    break;
                case 10:              
                    uiManager.LoadingScene_BarCounter();//开启第二天经营
                    break;
                case 11:
                    ShopPlane.SetActive(true);//显示商店
                    uiManager.Close_AVG();
                    break;
                case 100:
                    uiManager.barCounterManager.StartWork();//开始经营
                    uiManager.Close_AVG();
                    break;
                case 101:
                    uiManager.LoadingScene_BJ_Mobile();//开始对战
                    break;
                case 111:
                    if (Random.Range(0, 3) == 2)
                    {
                        uiManager.Load_AVG(10);//没有遇到商人
                    }
                    else 
                    {
                        uiManager.Load_AVG(11);//遇到商人                       
                    }
                    break;
            }

           

        }


    }
}