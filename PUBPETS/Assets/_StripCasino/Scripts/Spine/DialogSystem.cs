using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
namespace Blackjack_Game
{
    public class DialogSystem : MonoBehaviour
    {
        [Header("UI组件")]
        public Text textLabel;
        public Text textLabel_2;

        public List<GameObject> NameObject = new List<GameObject>();


        private Dictionary<int, TextAsset> textAssets = new Dictionary<int, TextAsset>();


        public int index;
        public float textSpeed;
        bool textFinished;//是否完成打字
        bool cancelTyping;//取消打字
        List<string> textList = new List<string>();

        [Header("Spine动画器总控制")]
        public Spine_FrameEvents spine_FrameEvents;
        public GameObject Black_Half_AVG;//通常对话框
        public GameObject Black_Half_CG;//CG对话框

        [Header("男主自定义名称带入")]
        public Text Me;

        void SetMyName()
        {
            // 先加载存档数据
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);


            Me.text = data.saveName;
        }

        [Header("这是哪个动画需要的对话")]
        public int animation_number;

        [Header("对话，背景，角色")]
        public GameObject TextButton;

        public Image People;
        public Animator People_Anim;
        public Sprite ShopManager, Andon,Enemy,XXX, Eirik, MimicHades, Doll3, Jeweler,
                       NPC_1, NPC_2, NPC_3, NPC_4,
                      Anto_Lose,
                      Anto_01, Anto_02, Anto_03, Anto_04, Anto_05, Anto_06,
                      Anto_07, Anto_08, Anto_09, Anto_10, Anto_11, Anto_12,
                      Anto_13, Anto_14, Anto_15, Anto_16, Anto_17, Anto_18,
        Alice_Lose, Alice_01, Alice_02, Alice_03, Alice_04, Alice_05, Alice_06, Alice_07, Alice_08,
        Hetty_Lose, Hetty_01, Hetty_02, Hetty_03, Hetty_04, Hetty_05, Hetty_06, Hetty_07, Hetty_08;

        public Image Background;
        public Sprite Black,
                      BarCounter,
                      Background_DungeonEntrance, Background_DungeonCorridor, Background_Town,
                      Background_Shop;

        public GameObject Lose_Black;//失败后的黑色外圈

        [Header("自动播放")]
        public bool autoMode = false; // 是否自动播放
        public float autoDelay = 1.5f; // 每句之间的间隔秒数
        public GameObject Auto_On, Auto_Off, Auto_on, Auto_off;

        public void ToggleAutoMode()
        {
            autoMode = !autoMode;
            Debug.Log("自动模式已" + (autoMode ? "开启" : "关闭"));

            if (!autoMode)
            {
                Auto_On.SetActive(false);
                Auto_on.SetActive(false);

                Auto_Off.SetActive(true);
                Auto_off.SetActive(true);
            }
            else
            {
                Auto_On.SetActive(true);
                Auto_on.SetActive(true);

                Auto_Off.SetActive(false);
                Auto_off.SetActive(false);
            }

        }//自动播放

        private void OnEnable()
        {
            //在女荷官指名界面开始，确定出现对应女荷官，胜利后跳出Victory，失败后跳出对应失败AVG都需要记录中【VS_XX】
            //if (!string.IsNullOrEmpty(GameFlowData.nextAVGId))
            //{
            //    Debug.Log("要播放的AVG是：" + GameFlowData.nextAVGId);
            //
            //    // 播放后清空
            //    GameFlowData.nextAVGId = null;
            //}


            //读取textSpeed
            textSpeed = PlayerPrefs.GetFloat("TextSpeed");

            Invoke("Read", 0.1f);




            switch (animation_number)
            {

                case 1013:
                case 1023:
                case 1033:
                case 1043:
                case 1053:
                case 1063:
                case 1073:
                case 1083:
                case 1093:
                case 1103:

                case 2013:
                case 2023:
                case 2033:
                case 2043:
                case 2053:
                case 2063:
                case 2073:
                case 2083:
                case 2093:
                case 2103:

                case 3013:
                case 3023:
                case 3033:
                case 3043:
                case 3053:
                case 3063:
                case 3073:
                case 3083:
                case 3093:
                case 3103:
                    Black_Half_CG.SetActive(true);
                    break;

                default:
                    Black_Half_AVG.SetActive(true);
                    break;
            }



            SetMyName();//读取玩家名称



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
                    //剧情开头
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Japanese/J_StartStory_01"));

                    //开启一天工作
                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Japanese/J_StartWork_01"));

                    //离开酒馆前往商店
                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Japanese/J_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Japanese/J_StartShop_02"));

                    //离开酒馆获得配方
                    textAssets.Add(12, Resources.Load<TextAsset>("TXT_Japanese/J_StartRecipe_01"));
                    textAssets.Add(13, Resources.Load<TextAsset>("TXT_Japanese/J_StartRecipe_02"));

                    #region  安托日语


                    //安托第一幕
                    textAssets.Add(1011, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_01_1"));
                    textAssets.Add(1012, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_01_2"));
                    textAssets.Add(1013, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_01_3"));
                    textAssets.Add(1014, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_01_4"));
                    //安托第二幕
                    textAssets.Add(1021, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_02_1"));
                    textAssets.Add(1022, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_02_2"));
                    textAssets.Add(1023, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_02_3"));
                    textAssets.Add(1024, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_02_4"));
                    //安托第三幕
                    textAssets.Add(1031, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_03_1"));
                    textAssets.Add(1032, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_03_2"));
                    textAssets.Add(1033, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_03_3"));
                    textAssets.Add(1034, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_03_4"));
                    //安托第四幕
                    textAssets.Add(1041, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_04_1"));
                    textAssets.Add(1042, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_04_2"));
                    textAssets.Add(1043, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_04_3"));
                    textAssets.Add(1044, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_04_4"));
                    //安托第五幕
                    textAssets.Add(1051, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_05_1"));
                    textAssets.Add(1052, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_05_2"));
                    textAssets.Add(1053, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_05_3"));
                    textAssets.Add(1054, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_05_4"));
                    //安托第六幕
                    textAssets.Add(1061, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_06_1"));
                    textAssets.Add(1062, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_06_2"));
                    textAssets.Add(1063, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_06_3"));
                    textAssets.Add(1064, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_06_4"));
                    //安托第七幕
                    textAssets.Add(1071, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_07_1"));
                    textAssets.Add(1072, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_07_2"));
                    textAssets.Add(1073, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_07_3"));
                    textAssets.Add(1074, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_07_4"));
                    //安托第八幕
                    textAssets.Add(1081, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_08_1"));
                    textAssets.Add(1082, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_08_2"));
                    textAssets.Add(1083, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_08_3"));
                    textAssets.Add(1084, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_08_4"));
                    //安托第九幕
                    textAssets.Add(1091, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_09_1"));
                    textAssets.Add(1092, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_09_2"));
                    textAssets.Add(1093, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_09_3"));
                    textAssets.Add(1094, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_09_4"));
                    //安托第十幕
                    textAssets.Add(1101, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_10_1"));
                    textAssets.Add(1102, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_10_2"));
                    textAssets.Add(1103, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_10_3"));
                    textAssets.Add(1104, Resources.Load<TextAsset>("TXT_Japanese/Anto_J/J_Anto_CG_10_4"));

                    #endregion

                    #region  赫蒂日语

                    //赫蒂第一幕
                    textAssets.Add(2011, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_01_1"));
                    textAssets.Add(2012, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_01_2"));
                    textAssets.Add(2013, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_01_3"));
                    textAssets.Add(2014, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_01_4"));
                    //赫蒂第二幕
                    textAssets.Add(2021, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_02_1"));
                    textAssets.Add(2022, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_02_2"));
                    textAssets.Add(2023, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_02_3"));
                    textAssets.Add(2024, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_02_4"));
                    //赫蒂第三幕
                    textAssets.Add(2031, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_03_1"));
                    textAssets.Add(2032, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_03_2"));
                    textAssets.Add(2033, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_03_3"));
                    textAssets.Add(2034, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_03_4"));
                    //赫蒂第四幕
                    textAssets.Add(2041, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_04_1"));
                    textAssets.Add(2042, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_04_2"));
                    textAssets.Add(2043, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_04_3"));
                    textAssets.Add(2044, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_04_4"));
                    //赫蒂第五幕
                    textAssets.Add(2051, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_05_1"));
                    textAssets.Add(2052, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_05_2"));
                    textAssets.Add(2053, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_05_3"));
                    textAssets.Add(2054, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_05_4"));
                    //赫蒂第六幕
                    textAssets.Add(2061, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_06_1"));
                    textAssets.Add(2062, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_06_2"));
                    textAssets.Add(2063, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_06_3"));
                    textAssets.Add(2064, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_06_4"));
                    //赫蒂第七幕
                    textAssets.Add(2071, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_07_1"));
                    textAssets.Add(2072, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_07_2"));
                    textAssets.Add(2073, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_07_3"));
                    textAssets.Add(2074, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_07_4"));
                    //赫蒂第八幕
                    textAssets.Add(2081, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_08_1"));
                    textAssets.Add(2082, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_08_2"));
                    textAssets.Add(2083, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_08_3"));
                    textAssets.Add(2084, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_08_4"));
                    //赫蒂第九幕
                    textAssets.Add(2091, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_09_1"));
                    textAssets.Add(2092, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_09_2"));
                    textAssets.Add(2093, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_09_3"));
                    textAssets.Add(2094, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_09_4"));
                    //赫蒂第十幕
                    textAssets.Add(2101, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_10_1"));
                    textAssets.Add(2102, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_10_2"));
                    textAssets.Add(2103, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_10_3"));
                    textAssets.Add(2104, Resources.Load<TextAsset>("TXT_Japanese/Hetty_J/J_Hetty_CG_10_4"));

                    #endregion

                    #region   爱丽丝日语


                    //爱丽丝第一幕
                    textAssets.Add(3011, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_01_1"));
                    textAssets.Add(3012, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_01_2"));
                    textAssets.Add(3013, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_01_3"));
                    textAssets.Add(3014, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_01_4"));
                    //爱丽丝第二幕
                    textAssets.Add(3021, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_02_1"));
                    textAssets.Add(3022, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_02_2"));
                    textAssets.Add(3023, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_02_3"));
                    textAssets.Add(3024, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_02_4"));
                    //爱丽丝第三幕
                    textAssets.Add(3031, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_03_1"));
                    textAssets.Add(3032, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_03_2"));
                    textAssets.Add(3033, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_03_3"));
                    textAssets.Add(3034, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_03_4"));
                    //爱丽丝第四幕
                    textAssets.Add(3041, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_04_1"));
                    textAssets.Add(3042, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_04_2"));
                    textAssets.Add(3043, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_04_3"));
                    textAssets.Add(3044, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_04_4"));
                    //爱丽丝第五幕
                    textAssets.Add(3051, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_05_1"));
                    textAssets.Add(3052, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_05_2"));
                    textAssets.Add(3053, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_05_3"));
                    textAssets.Add(3054, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_05_4"));
                    //爱丽丝第六幕
                    textAssets.Add(3061, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_06_1"));
                    textAssets.Add(3062, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_06_2"));
                    textAssets.Add(3063, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_06_3"));
                    textAssets.Add(3064, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_06_4"));
                    //爱丽丝第七幕
                    textAssets.Add(3071, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_07_1"));
                    textAssets.Add(3072, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_07_2"));
                    textAssets.Add(3073, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_07_3"));
                    textAssets.Add(3074, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_07_4"));
                    //爱丽丝第八幕
                    textAssets.Add(3081, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_08_1"));
                    textAssets.Add(3082, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_08_2"));
                    textAssets.Add(3083, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_08_3"));
                    textAssets.Add(3084, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_08_4"));
                    //爱丽丝第九幕
                    textAssets.Add(3091, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_09_1"));
                    textAssets.Add(3092, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_09_2"));
                    textAssets.Add(3093, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_09_3"));
                    textAssets.Add(3094, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_09_4"));
                    //爱丽丝第十幕
                    textAssets.Add(3101, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_10_1"));
                    textAssets.Add(3102, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_10_2"));
                    textAssets.Add(3103, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_10_3"));
                    textAssets.Add(3104, Resources.Load<TextAsset>("TXT_Japanese/Alice_J/J_Alice_CG_10_4"));

                    #endregion

                    break;
                case 1:
                    //剧情开头
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartStory_01"));

                    //开启一天工作
                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartWork_01"));



                    //离开酒馆前往商店
                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartShop_02"));

                    //离开酒馆获得配方
                    textAssets.Add(12, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartRecipe_01"));
                    textAssets.Add(13, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_StartRecipe_02"));

                    #region  安托简中


                    //安托第一幕
                    textAssets.Add(1011, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_01_1"));
                    textAssets.Add(1012, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_01_2"));
                    textAssets.Add(1013, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_01_3"));
                    textAssets.Add(1014, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_01_4"));
                    //安托第二幕
                    textAssets.Add(1021, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_02_1"));
                    textAssets.Add(1022, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_02_2"));
                    textAssets.Add(1023, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_02_3"));
                    textAssets.Add(1024, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_02_4"));
                    //安托第三幕
                    textAssets.Add(1031, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_03_1"));
                    textAssets.Add(1032, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_03_2"));
                    textAssets.Add(1033, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_03_3"));
                    textAssets.Add(1034, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_03_4"));
                    //安托第四幕
                    textAssets.Add(1041, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_04_1"));
                    textAssets.Add(1042, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_04_2"));
                    textAssets.Add(1043, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_04_3"));
                    textAssets.Add(1044, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_04_4"));
                    //安托第五幕
                    textAssets.Add(1051, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_05_1"));
                    textAssets.Add(1052, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_05_2"));
                    textAssets.Add(1053, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_05_3"));
                    textAssets.Add(1054, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_05_4"));
                    //安托第六幕
                    textAssets.Add(1061, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_06_1"));
                    textAssets.Add(1062, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_06_2"));
                    textAssets.Add(1063, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_06_3"));
                    textAssets.Add(1064, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_06_4"));
                    //安托第七幕
                    textAssets.Add(1071, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_07_1"));
                    textAssets.Add(1072, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_07_2"));
                    textAssets.Add(1073, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_07_3"));
                    textAssets.Add(1074, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_07_4"));
                    //安托第八幕
                    textAssets.Add(1081, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_08_1"));
                    textAssets.Add(1082, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_08_2"));
                    textAssets.Add(1083, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_08_3"));
                    textAssets.Add(1084, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_08_4"));
                    //安托第九幕
                    textAssets.Add(1091, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_09_1"));
                    textAssets.Add(1092, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_09_2"));
                    textAssets.Add(1093, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_09_3"));
                    textAssets.Add(1094, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_09_4"));
                    //安托第十幕
                    textAssets.Add(1101, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_10_1"));
                    textAssets.Add(1102, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_10_2"));
                    textAssets.Add(1103, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_10_3"));
                    textAssets.Add(1104, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Anto_C1/C1_Anto_CG_10_4"));

                    #endregion

                    #region  赫蒂简中


                    //赫蒂第一幕
                    textAssets.Add(2011, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_01_1"));
                    textAssets.Add(2012, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_01_2"));
                    textAssets.Add(2013, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_01_3"));
                    textAssets.Add(2014, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_01_4"));
                    //赫蒂第二幕
                    textAssets.Add(2021, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_02_1"));
                    textAssets.Add(2022, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_02_2"));
                    textAssets.Add(2023, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_02_3"));
                    textAssets.Add(2024, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_02_4"));
                    //赫蒂第三幕
                    textAssets.Add(2031, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_03_1"));
                    textAssets.Add(2032, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_03_2"));
                    textAssets.Add(2033, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_03_3"));
                    textAssets.Add(2034, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_03_4"));
                    //赫蒂第四幕
                    textAssets.Add(2041, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_04_1"));
                    textAssets.Add(2042, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_04_2"));
                    textAssets.Add(2043, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_04_3"));
                    textAssets.Add(2044, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_04_4"));
                    //赫蒂第五幕
                    textAssets.Add(2051, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_05_1"));
                    textAssets.Add(2052, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_05_2"));
                    textAssets.Add(2053, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_05_3"));
                    textAssets.Add(2054, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_05_4"));
                    //赫蒂第六幕
                    textAssets.Add(2061, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_06_1"));
                    textAssets.Add(2062, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_06_2"));
                    textAssets.Add(2063, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_06_3"));
                    textAssets.Add(2064, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_06_4"));
                    //赫蒂第七幕
                    textAssets.Add(2071, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_07_1"));
                    textAssets.Add(2072, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_07_2"));
                    textAssets.Add(2073, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_07_3"));
                    textAssets.Add(2074, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_07_4"));
                    //赫蒂第八幕
                    textAssets.Add(2081, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_08_1"));
                    textAssets.Add(2082, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_08_2"));
                    textAssets.Add(2083, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_08_3"));
                    textAssets.Add(2084, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_08_4"));
                    //赫蒂第九幕
                    textAssets.Add(2091, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_09_1"));
                    textAssets.Add(2092, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_09_2"));
                    textAssets.Add(2093, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_09_3"));
                    textAssets.Add(2094, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_09_4"));
                    //赫蒂第十幕
                    textAssets.Add(2101, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_10_1"));
                    textAssets.Add(2102, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_10_2"));
                    textAssets.Add(2103, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_10_3"));
                    textAssets.Add(2104, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Hetty_C1/C1_Hetty_CG_10_4"));

                    #endregion

                    #region  爱丽丝简中


                    //爱丽丝第一幕
                    textAssets.Add(3011, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_01_1"));
                    textAssets.Add(3012, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_01_2"));
                    textAssets.Add(3013, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_01_3"));
                    textAssets.Add(3014, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_01_4"));
                    //爱丽丝第二幕
                    textAssets.Add(3021, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_02_1"));
                    textAssets.Add(3022, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_02_2"));
                    textAssets.Add(3023, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_02_3"));
                    textAssets.Add(3024, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_02_4"));
                    //爱丽丝第三幕
                    textAssets.Add(3031, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_03_1"));
                    textAssets.Add(3032, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_03_2"));
                    textAssets.Add(3033, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_03_3"));
                    textAssets.Add(3034, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_03_4"));
                    //爱丽丝第四幕
                    textAssets.Add(3041, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_04_1"));
                    textAssets.Add(3042, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_04_2"));
                    textAssets.Add(3043, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_04_3"));
                    textAssets.Add(3044, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_04_4"));
                    //爱丽丝第五幕
                    textAssets.Add(3051, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_05_1"));
                    textAssets.Add(3052, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_05_2"));
                    textAssets.Add(3053, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_05_3"));
                    textAssets.Add(3054, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_05_4"));
                    //爱丽丝第六幕
                    textAssets.Add(3061, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_06_1"));
                    textAssets.Add(3062, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_06_2"));
                    textAssets.Add(3063, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_06_3"));
                    textAssets.Add(3064, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_06_4"));
                    //爱丽丝第七幕
                    textAssets.Add(3071, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_07_1"));
                    textAssets.Add(3072, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_07_2"));
                    textAssets.Add(3073, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_07_3"));
                    textAssets.Add(3074, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_07_4"));
                    //爱丽丝第八幕
                    textAssets.Add(3081, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_08_1"));
                    textAssets.Add(3082, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_08_2"));
                    textAssets.Add(3083, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_08_3"));
                    textAssets.Add(3084, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_08_4"));
                    //爱丽丝第九幕
                    textAssets.Add(3091, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_09_1"));
                    textAssets.Add(3092, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_09_2"));
                    textAssets.Add(3093, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_09_3"));
                    textAssets.Add(3094, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_09_4"));
                    //爱丽丝第十幕
                    textAssets.Add(3101, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_10_1"));
                    textAssets.Add(3102, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_10_2"));
                    textAssets.Add(3103, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_10_3"));
                    textAssets.Add(3104, Resources.Load<TextAsset>("TXT_Simplified_Chinese/Alice_C1/C1_Alice_CG_10_4"));

                    #endregion


                    break;
                case 2:
                    //剧情开头
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartStory_01"));

                    //开启一天工作
                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartWork_01"));


                    //离开酒馆前往商店
                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartShop_02"));


                    //离开酒馆获得配方
                    textAssets.Add(12, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartRecipe_01"));
                    textAssets.Add(13, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_StartRecipe_02"));

                    #region  安托繁中


                    //安托第一幕
                    textAssets.Add(1011, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_01_1"));
                    textAssets.Add(1012, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_01_2"));
                    textAssets.Add(1013, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_01_3"));
                    textAssets.Add(1014, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_01_4"));
                    //安托第二幕
                    textAssets.Add(1021, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_02_1"));
                    textAssets.Add(1022, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_02_2"));
                    textAssets.Add(1023, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_02_3"));
                    textAssets.Add(1024, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_02_4"));
                    //安托第三幕
                    textAssets.Add(1031, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_03_1"));
                    textAssets.Add(1032, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_03_2"));
                    textAssets.Add(1033, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_03_3"));
                    textAssets.Add(1034, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_03_4"));
                    //安托第四幕
                    textAssets.Add(1041, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_04_1"));
                    textAssets.Add(1042, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_04_2"));
                    textAssets.Add(1043, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_04_3"));
                    textAssets.Add(1044, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_04_4"));
                    //安托第五幕
                    textAssets.Add(1051, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_05_1"));
                    textAssets.Add(1052, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_05_2"));
                    textAssets.Add(1053, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_05_3"));
                    textAssets.Add(1054, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_05_4"));
                    //安托第六幕
                    textAssets.Add(1061, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_06_1"));
                    textAssets.Add(1062, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_06_2"));
                    textAssets.Add(1063, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_06_3"));
                    textAssets.Add(1064, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_06_4"));
                    //安托第七幕
                    textAssets.Add(1071, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_07_1"));
                    textAssets.Add(1072, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_07_2"));
                    textAssets.Add(1073, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_07_3"));
                    textAssets.Add(1074, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_07_4"));
                    //安托第八幕
                    textAssets.Add(1081, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_08_1"));
                    textAssets.Add(1082, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_08_2"));
                    textAssets.Add(1083, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_08_3"));
                    textAssets.Add(1084, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_08_4"));
                    //安托第九幕
                    textAssets.Add(1091, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_09_1"));
                    textAssets.Add(1092, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_09_2"));
                    textAssets.Add(1093, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_09_3"));
                    textAssets.Add(1094, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_09_4"));
                    //安托第十幕
                    textAssets.Add(1101, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_10_1"));
                    textAssets.Add(1102, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_10_2"));
                    textAssets.Add(1103, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_10_3"));
                    textAssets.Add(1104, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Anto_C2/C2_Anto_CG_10_4"));

                    #endregion


                    #region  赫蒂繁中

                    //赫蒂第一幕
                    textAssets.Add(2011, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_01_1"));
                    textAssets.Add(2012, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_01_2"));
                    textAssets.Add(2013, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_01_3"));
                    textAssets.Add(2014, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_01_4"));
                    //赫蒂第二幕
                    textAssets.Add(2021, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_02_1"));
                    textAssets.Add(2022, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_02_2"));
                    textAssets.Add(2023, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_02_3"));
                    textAssets.Add(2024, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_02_4"));
                    //赫蒂第三幕
                    textAssets.Add(2031, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_03_1"));
                    textAssets.Add(2032, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_03_2"));
                    textAssets.Add(2033, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_03_3"));
                    textAssets.Add(2034, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_03_4"));
                    //赫蒂第四幕
                    textAssets.Add(2041, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_04_1"));
                    textAssets.Add(2042, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_04_2"));
                    textAssets.Add(2043, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_04_3"));
                    textAssets.Add(2044, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_04_4"));
                    //赫蒂第五幕
                    textAssets.Add(2051, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_05_1"));
                    textAssets.Add(2052, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_05_2"));
                    textAssets.Add(2053, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_05_3"));
                    textAssets.Add(2054, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_05_4"));
                    //赫蒂第六幕
                    textAssets.Add(2061, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_06_1"));
                    textAssets.Add(2062, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_06_2"));
                    textAssets.Add(2063, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_06_3"));
                    textAssets.Add(2064, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_06_4"));
                    //赫蒂第七幕
                    textAssets.Add(2071, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_07_1"));
                    textAssets.Add(2072, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_07_2"));
                    textAssets.Add(2073, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_07_3"));
                    textAssets.Add(2074, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_07_4"));
                    //赫蒂第八幕
                    textAssets.Add(2081, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_08_1"));
                    textAssets.Add(2082, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_08_2"));
                    textAssets.Add(2083, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_08_3"));
                    textAssets.Add(2084, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_08_4"));
                    //赫蒂第九幕
                    textAssets.Add(2091, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_09_1"));
                    textAssets.Add(2092, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_09_2"));
                    textAssets.Add(2093, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_09_3"));
                    textAssets.Add(2094, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_09_4"));
                    //赫蒂第十幕
                    textAssets.Add(2101, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_10_1"));
                    textAssets.Add(2102, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_10_2"));
                    textAssets.Add(2103, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_10_3"));
                    textAssets.Add(2104, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Hetty_C2/C2_Hetty_CG_10_4"));

                    #endregion

                    #region 爱丽丝繁中


                    //爱丽丝第一幕
                    textAssets.Add(3011, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_01_1"));
                    textAssets.Add(3012, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_01_2"));
                    textAssets.Add(3013, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_01_3"));
                    textAssets.Add(3014, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_01_4"));
                    //爱丽丝第二幕
                    textAssets.Add(3021, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_02_1"));
                    textAssets.Add(3022, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_02_2"));
                    textAssets.Add(3023, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_02_3"));
                    textAssets.Add(3024, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_02_4"));
                    //爱丽丝第三幕
                    textAssets.Add(3031, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_03_1"));
                    textAssets.Add(3032, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_03_2"));
                    textAssets.Add(3033, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_03_3"));
                    textAssets.Add(3034, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_03_4"));
                    //爱丽丝第四幕
                    textAssets.Add(3041, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_04_1"));
                    textAssets.Add(3042, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_04_2"));
                    textAssets.Add(3043, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_04_3"));
                    textAssets.Add(3044, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_04_4"));
                    //爱丽丝第五幕
                    textAssets.Add(3051, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_05_1"));
                    textAssets.Add(3052, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_05_2"));
                    textAssets.Add(3053, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_05_3"));
                    textAssets.Add(3054, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_05_4"));
                    //爱丽丝第六幕
                    textAssets.Add(3061, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_06_1"));
                    textAssets.Add(3062, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_06_2"));
                    textAssets.Add(3063, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_06_3"));
                    textAssets.Add(3064, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_06_4"));
                    //爱丽丝第七幕
                    textAssets.Add(3071, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_07_1"));
                    textAssets.Add(3072, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_07_2"));
                    textAssets.Add(3073, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_07_3"));
                    textAssets.Add(3074, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_07_4"));
                    //爱丽丝第八幕
                    textAssets.Add(3081, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_08_1"));
                    textAssets.Add(3082, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_08_2"));
                    textAssets.Add(3083, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_08_3"));
                    textAssets.Add(3084, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_08_4"));
                    //爱丽丝第九幕
                    textAssets.Add(3091, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_09_1"));
                    textAssets.Add(3092, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_09_2"));
                    textAssets.Add(3093, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_09_3"));
                    textAssets.Add(3094, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_09_4"));
                    //爱丽丝第十幕
                    textAssets.Add(3101, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_10_1"));
                    textAssets.Add(3102, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_10_2"));
                    textAssets.Add(3103, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_10_3"));
                    textAssets.Add(3104, Resources.Load<TextAsset>("TXT_Traditional_Chinese/Alice_C2/C2_Alice_CG_10_4"));

                    #endregion

                    break;
                case 3:
                    //剧情开头
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_English/E_StartStory_01"));

                    //开启一天工作
                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_English/E_StartWork_01"));


                    //离开酒馆前往商店
                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_English/E_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_English/E_StartShop_02"));


                    //离开酒馆获得配方
                    textAssets.Add(12, Resources.Load<TextAsset>("TXT_English/E_StartRecipe_01"));
                    textAssets.Add(13, Resources.Load<TextAsset>("TXT_English/E_StartRecipe_02"));



                    #region  安托英语


                    //安托第一幕
                    textAssets.Add(1011, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_01_1"));
                    textAssets.Add(1012, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_01_2"));
                    textAssets.Add(1013, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_01_3"));
                    textAssets.Add(1014, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_01_4"));
                    //安托第二幕
                    textAssets.Add(1021, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_02_1"));
                    textAssets.Add(1022, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_02_2"));
                    textAssets.Add(1023, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_02_3"));
                    textAssets.Add(1024, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_02_4"));
                    //安托第三幕
                    textAssets.Add(1031, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_03_1"));
                    textAssets.Add(1032, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_03_2"));
                    textAssets.Add(1033, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_03_3"));
                    textAssets.Add(1034, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_03_4"));
                    //安托第四幕
                    textAssets.Add(1041, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_04_1"));
                    textAssets.Add(1042, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_04_2"));
                    textAssets.Add(1043, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_04_3"));
                    textAssets.Add(1044, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_04_4"));
                    //安托第五幕
                    textAssets.Add(1051, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_05_1"));
                    textAssets.Add(1052, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_05_2"));
                    textAssets.Add(1053, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_05_3"));
                    textAssets.Add(1054, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_05_4"));
                    //安托第六幕
                    textAssets.Add(1061, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_06_1"));
                    textAssets.Add(1062, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_06_2"));
                    textAssets.Add(1063, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_06_3"));
                    textAssets.Add(1064, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_06_4"));
                    //安托第七幕
                    textAssets.Add(1071, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_07_1"));
                    textAssets.Add(1072, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_07_2"));
                    textAssets.Add(1073, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_07_3"));
                    textAssets.Add(1074, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_07_4"));
                    //安托第八幕
                    textAssets.Add(1081, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_08_1"));
                    textAssets.Add(1082, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_08_2"));
                    textAssets.Add(1083, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_08_3"));
                    textAssets.Add(1084, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_08_4"));
                    //安托第九幕
                    textAssets.Add(1091, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_09_1"));
                    textAssets.Add(1092, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_09_2"));
                    textAssets.Add(1093, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_09_3"));
                    textAssets.Add(1094, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_09_4"));
                    //安托第十幕
                    textAssets.Add(1101, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_10_1"));
                    textAssets.Add(1102, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_10_2"));
                    textAssets.Add(1103, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_10_3"));
                    textAssets.Add(1104, Resources.Load<TextAsset>("TXT_English/Anto_E/E_Anto_CG_10_4"));

                    #endregion


                    #region  赫蒂英语

                    //赫蒂第一幕
                    textAssets.Add(2011, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_01_1"));
                    textAssets.Add(2012, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_01_2"));
                    textAssets.Add(2013, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_01_3"));
                    textAssets.Add(2014, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_01_4"));
                    //赫蒂第二幕
                    textAssets.Add(2021, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_02_1"));
                    textAssets.Add(2022, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_02_2"));
                    textAssets.Add(2023, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_02_3"));
                    textAssets.Add(2024, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_02_4"));
                    //赫蒂第三幕
                    textAssets.Add(2031, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_03_1"));
                    textAssets.Add(2032, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_03_2"));
                    textAssets.Add(2033, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_03_3"));
                    textAssets.Add(2034, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_03_4"));
                    //赫蒂第四幕
                    textAssets.Add(2041, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_04_1"));
                    textAssets.Add(2042, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_04_2"));
                    textAssets.Add(2043, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_04_3"));
                    textAssets.Add(2044, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_04_4"));
                    //赫蒂第五幕
                    textAssets.Add(2051, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_05_1"));
                    textAssets.Add(2052, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_05_2"));
                    textAssets.Add(2053, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_05_3"));
                    textAssets.Add(2054, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_05_4"));
                    //赫蒂第六幕
                    textAssets.Add(2061, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_06_1"));
                    textAssets.Add(2062, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_06_2"));
                    textAssets.Add(2063, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_06_3"));
                    textAssets.Add(2064, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_06_4"));
                    //赫蒂第七幕
                    textAssets.Add(2071, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_07_1"));
                    textAssets.Add(2072, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_07_2"));
                    textAssets.Add(2073, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_07_3"));
                    textAssets.Add(2074, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_07_4"));
                    //赫蒂第八幕
                    textAssets.Add(2081, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_08_1"));
                    textAssets.Add(2082, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_08_2"));
                    textAssets.Add(2083, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_08_3"));
                    textAssets.Add(2084, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_08_4"));
                    //赫蒂第九幕
                    textAssets.Add(2091, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_09_1"));
                    textAssets.Add(2092, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_09_2"));
                    textAssets.Add(2093, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_09_3"));
                    textAssets.Add(2094, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_09_4"));
                    //赫蒂第十幕
                    textAssets.Add(2101, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_10_1"));
                    textAssets.Add(2102, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_10_2"));
                    textAssets.Add(2103, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_10_3"));
                    textAssets.Add(2104, Resources.Load<TextAsset>("TXT_English/Hetty_E/E_Hetty_CG_10_4"));

                    #endregion

                    #region    爱丽丝英语


                    //爱丽丝第一幕
                    textAssets.Add(3011, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_01_1"));
                    textAssets.Add(3012, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_01_2"));
                    textAssets.Add(3013, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_01_3"));
                    textAssets.Add(3014, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_01_4"));
                    //爱丽丝第二幕
                    textAssets.Add(3021, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_02_1"));
                    textAssets.Add(3022, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_02_2"));
                    textAssets.Add(3023, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_02_3"));
                    textAssets.Add(3024, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_02_4"));
                    //爱丽丝第三幕
                    textAssets.Add(3031, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_03_1"));
                    textAssets.Add(3032, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_03_2"));
                    textAssets.Add(3033, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_03_3"));
                    textAssets.Add(3034, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_03_4"));
                    //爱丽丝第四幕
                    textAssets.Add(3041, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_04_1"));
                    textAssets.Add(3042, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_04_2"));
                    textAssets.Add(3043, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_04_3"));
                    textAssets.Add(3044, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_04_4"));
                    //爱丽丝第五幕
                    textAssets.Add(3051, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_05_1"));
                    textAssets.Add(3052, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_05_2"));
                    textAssets.Add(3053, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_05_3"));
                    textAssets.Add(3054, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_05_4"));
                    //爱丽丝第六幕
                    textAssets.Add(3061, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_06_1"));
                    textAssets.Add(3062, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_06_2"));
                    textAssets.Add(3063, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_06_3"));
                    textAssets.Add(3064, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_06_4"));
                    //爱丽丝第七幕
                    textAssets.Add(3071, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_07_1"));
                    textAssets.Add(3072, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_07_2"));
                    textAssets.Add(3073, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_07_3"));
                    textAssets.Add(3074, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_07_4"));
                    //爱丽丝第八幕
                    textAssets.Add(3081, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_08_1"));
                    textAssets.Add(3082, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_08_2"));
                    textAssets.Add(3083, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_08_3"));
                    textAssets.Add(3084, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_08_4"));
                    //爱丽丝第九幕
                    textAssets.Add(3091, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_09_1"));
                    textAssets.Add(3092, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_09_2"));
                    textAssets.Add(3093, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_09_3"));
                    textAssets.Add(3094, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_09_4"));
                    //爱丽丝第十幕
                    textAssets.Add(3101, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_10_1"));
                    textAssets.Add(3102, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_10_2"));
                    textAssets.Add(3103, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_10_3"));
                    textAssets.Add(3104, Resources.Load<TextAsset>("TXT_English/Alice_E/E_Alice_CG_10_4"));

                    #endregion



                    break;
                case 4:
                    //剧情开头
                    textAssets.Add(1, Resources.Load<TextAsset>("TXT_Korean/K_StartStory_01"));

                    //开启一天工作
                    textAssets.Add(100, Resources.Load<TextAsset>("TXT_Korean/K_StartWork_01"));

                    //离开酒馆前往商店
                    textAssets.Add(10, Resources.Load<TextAsset>("TXT_Korean/K_StartShop_01"));
                    textAssets.Add(11, Resources.Load<TextAsset>("TXT_Korean/K_StartShop_02"));


                    //离开酒馆获得配方
                    textAssets.Add(12, Resources.Load<TextAsset>("TXT_Korean/K_StartRecipe_01"));
                    textAssets.Add(13, Resources.Load<TextAsset>("TXT_Korean/K_StartRecipe_02"));

                    #region  安托韩语


                    //安托第一幕
                    textAssets.Add(1011, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_01_1"));
                    textAssets.Add(1012, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_01_2"));
                    textAssets.Add(1013, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_01_3"));
                    textAssets.Add(1014, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_01_4"));
                    //安托第二幕
                    textAssets.Add(1021, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_02_1"));
                    textAssets.Add(1022, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_02_2"));
                    textAssets.Add(1023, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_02_3"));
                    textAssets.Add(1024, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_02_4"));
                    //安托第三幕
                    textAssets.Add(1031, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_03_1"));
                    textAssets.Add(1032, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_03_2"));
                    textAssets.Add(1033, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_03_3"));
                    textAssets.Add(1034, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_03_4"));
                    //安托第四幕
                    textAssets.Add(1041, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_04_1"));
                    textAssets.Add(1042, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_04_2"));
                    textAssets.Add(1043, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_04_3"));
                    textAssets.Add(1044, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_04_4"));
                    //安托第五幕
                    textAssets.Add(1051, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_05_1"));
                    textAssets.Add(1052, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_05_2"));
                    textAssets.Add(1053, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_05_3"));
                    textAssets.Add(1054, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_05_4"));
                    //安托第六幕
                    textAssets.Add(1061, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_06_1"));
                    textAssets.Add(1062, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_06_2"));
                    textAssets.Add(1063, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_06_3"));
                    textAssets.Add(1064, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_06_4"));
                    //安托第七幕
                    textAssets.Add(1071, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_07_1"));
                    textAssets.Add(1072, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_07_2"));
                    textAssets.Add(1073, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_07_3"));
                    textAssets.Add(1074, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_07_4"));
                    //安托第八幕
                    textAssets.Add(1081, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_08_1"));
                    textAssets.Add(1082, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_08_2"));
                    textAssets.Add(1083, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_08_3"));
                    textAssets.Add(1084, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_08_4"));
                    //安托第九幕
                    textAssets.Add(1091, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_09_1"));
                    textAssets.Add(1092, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_09_2"));
                    textAssets.Add(1093, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_09_3"));
                    textAssets.Add(1094, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_09_4"));
                    //安托第十幕
                    textAssets.Add(1101, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_10_1"));
                    textAssets.Add(1102, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_10_2"));
                    textAssets.Add(1103, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_10_3"));
                    textAssets.Add(1104, Resources.Load<TextAsset>("TXT_Korean/Anto_K/K_Anto_CG_10_4"));

                    #endregion


                    #region  赫蒂韩语

                    //赫蒂第一幕
                    textAssets.Add(2011, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_01_1"));
                    textAssets.Add(2012, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_01_2"));
                    textAssets.Add(2013, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_01_3"));
                    textAssets.Add(2014, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_01_4"));
                    //赫蒂第二幕
                    textAssets.Add(2021, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_02_1"));
                    textAssets.Add(2022, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_02_2"));
                    textAssets.Add(2023, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_02_3"));
                    textAssets.Add(2024, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_02_4"));
                    //赫蒂第三幕
                    textAssets.Add(2031, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_03_1"));
                    textAssets.Add(2032, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_03_2"));
                    textAssets.Add(2033, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_03_3"));
                    textAssets.Add(2034, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_03_4"));
                    //赫蒂第四幕
                    textAssets.Add(2041, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_04_1"));
                    textAssets.Add(2042, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_04_2"));
                    textAssets.Add(2043, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_04_3"));
                    textAssets.Add(2044, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_04_4"));
                    //赫蒂第五幕
                    textAssets.Add(2051, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_05_1"));
                    textAssets.Add(2052, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_05_2"));
                    textAssets.Add(2053, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_05_3"));
                    textAssets.Add(2054, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_05_4"));
                    //赫蒂第六幕
                    textAssets.Add(2061, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_06_1"));
                    textAssets.Add(2062, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_06_2"));
                    textAssets.Add(2063, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_06_3"));
                    textAssets.Add(2064, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_06_4"));
                    //赫蒂第七幕
                    textAssets.Add(2071, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_07_1"));
                    textAssets.Add(2072, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_07_2"));
                    textAssets.Add(2073, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_07_3"));
                    textAssets.Add(2074, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_07_4"));
                    //赫蒂第八幕
                    textAssets.Add(2081, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_08_1"));
                    textAssets.Add(2082, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_08_2"));
                    textAssets.Add(2083, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_08_3"));
                    textAssets.Add(2084, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_08_4"));
                    //赫蒂第九幕
                    textAssets.Add(2091, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_09_1"));
                    textAssets.Add(2092, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_09_2"));
                    textAssets.Add(2093, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_09_3"));
                    textAssets.Add(2094, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_09_4"));
                    //赫蒂第十幕
                    textAssets.Add(2101, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_10_1"));
                    textAssets.Add(2102, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_10_2"));
                    textAssets.Add(2103, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_10_3"));
                    textAssets.Add(2104, Resources.Load<TextAsset>("TXT_Korean/Hetty_K/K_Hetty_CG_10_4"));

                    #endregion

                    #region 爱丽丝韩语


                    //爱丽丝第一幕
                    textAssets.Add(3011, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_01_1"));
                    textAssets.Add(3012, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_01_2"));
                    textAssets.Add(3013, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_01_3"));
                    textAssets.Add(3014, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_01_4"));
                    //爱丽丝第二幕
                    textAssets.Add(3021, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_02_1"));
                    textAssets.Add(3022, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_02_2"));
                    textAssets.Add(3023, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_02_3"));
                    textAssets.Add(3024, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_02_4"));
                    //爱丽丝第三幕
                    textAssets.Add(3031, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_03_1"));
                    textAssets.Add(3032, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_03_2"));
                    textAssets.Add(3033, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_03_3"));
                    textAssets.Add(3034, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_03_4"));
                    //爱丽丝第四幕
                    textAssets.Add(3041, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_04_1"));
                    textAssets.Add(3042, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_04_2"));
                    textAssets.Add(3043, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_04_3"));
                    textAssets.Add(3044, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_04_4"));
                    //爱丽丝第五幕
                    textAssets.Add(3051, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_05_1"));
                    textAssets.Add(3052, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_05_2"));
                    textAssets.Add(3053, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_05_3"));
                    textAssets.Add(3054, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_05_4"));
                    //爱丽丝第六幕
                    textAssets.Add(3061, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_06_1"));
                    textAssets.Add(3062, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_06_2"));
                    textAssets.Add(3063, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_06_3"));
                    textAssets.Add(3064, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_06_4"));
                    //爱丽丝第七幕
                    textAssets.Add(3071, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_07_1"));
                    textAssets.Add(3072, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_07_2"));
                    textAssets.Add(3073, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_07_3"));
                    textAssets.Add(3074, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_07_4"));
                    //爱丽丝第八幕
                    textAssets.Add(3081, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_08_1"));
                    textAssets.Add(3082, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_08_2"));
                    textAssets.Add(3083, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_08_3"));
                    textAssets.Add(3084, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_08_4"));
                    //爱丽丝第九幕
                    textAssets.Add(3091, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_09_1"));
                    textAssets.Add(3092, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_09_2"));
                    textAssets.Add(3093, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_09_3"));
                    textAssets.Add(3094, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_09_4"));
                    //爱丽丝第十幕
                    textAssets.Add(3101, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_10_1"));
                    textAssets.Add(3102, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_10_2"));
                    textAssets.Add(3103, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_10_3"));
                    textAssets.Add(3104, Resources.Load<TextAsset>("TXT_Korean/Alice_K/K_Alice_CG_10_4"));

                    #endregion

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

        }//点击下一句

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
            textLabel_2.text = "";/////////////////////////////////////////

            //判断一整行的字符是
            Text text = textLabel;
            Text text_2 = textLabel_2;///////////////////////////////////////////
            switch (textList[index].Trim().ToString())
            {
                #region CG用


                //透明背景功能
                case "CG":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;
                    break;

                case "CG_Loop_moan":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(moan_Clips);
                    break;

                case "CG_Loop_moanLoud":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(moanLoud_Clips);
                    break;




                case "CG_Loop_moanTentacle":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(moanTentacle_Clips);
                    break;

                case "CG_Loop_FeraTentacle":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(FeraTentacle_Clips);

                    Debug.Log("CG_Loop_FeraTentacle");
                    break;

                case "CG_Loop_FeraTentacleLoud":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(FeraTentacleLoud_Clips);
                    break;




                case "CG_Loop_moanPoleDance":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(moanPoleDance_Clips);
                    break;

                case "CG_Loop_moanLoudPoleDance":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(moanLoudPoleDance_Clips);
                    break;

                case "CG_Loop_FaintingGasp":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;

                    //重启娇喘
                    PlayMoanLoop(FaintingGasp_Clips);
                    break;


                case "--------------------NEXT--------------------":
                    text_2.color = Color.white;

                    voiceSource.Stop();

                    Background.gameObject.SetActive(false);// 透明背景播放CG
                    index++;
                    //当前显示的Spine动画器里触发Next
                    spine_FrameEvents.TriggerNext();
                    break;

                //角色
                case "me":
                    text_2.color = new Color(0.0f, 0.68f, 0.93f, 1.0f);//蓝色

                    voiceSource.Stop();

                    index++;
                    break;
                case "npc_1":
                    text_2.color = new Color(0.7f, 0.75f, 0.8f, 1.0f); // 亮灰色

                    voiceSource.Stop();

                    index++;
                    break;
                case "npc_2":
                    text_2.color = new Color(1.0f, 0.5f, 0.0f, 1.0f); // 橙色

                    voiceSource.Stop();

                    index++;
                    break;

                case "enemy":
                    text_2.color = new Color(0.65f, 0.7f, 0.8f, 1.0f); // 灰蓝

                    voiceSource.Stop();

                    index++;
                    break;

                case "eirik":
                    text_2.color = new Color(0.75f, 0.65f, 0.55f, 1.0f); // 暖灰橙

                    voiceSource.Stop();

                    index++;
                    break;

                case "thor":
                    text_2.color = new Color(0.8f, 0.75f, 0.55f, 1.0f); // 冷金灰

                    voiceSource.Stop();

                    index++;
                    break;

                case "doll3":
                    text_2.color = new Color(0.55f, 0.7f, 0.7f, 1.0f); // 冷青灰

                    voiceSource.Stop();

                    index++;
                    break;


                case "godfather":
                    text_2.color = new Color(0.55f, 0.2f, 0.2f, 1.0f); // 深酒红

                    voiceSource.Stop();

                    index++;
                    break;

                case "husband":
                    text_2.color = new Color(0.45f, 0.55f, 0.7f, 1.0f); // 冷静蓝灰

                    voiceSource.Stop();

                    index++;
                    break;



                case "anto":
                    text_2.color = new Color(1.0f, 0.2f, 0.5f, 1.0f); //浅红色

                    // 播放一句对白（娇喘暂停，播完再恢复）
                    Girl_Voice();

                    index++;
                    break;
                case "hetty":
                    text_2.color = new Color(0.8f, 0.2f, 0.2f, 1.0f); // 深红色


                    index++;
                    break;
                case "alice":
                    text_2.color = Color.green;

                    Girl_Voice();

                    index++;
                    break;
                #endregion

                #region AVG背景




                //场景    
                case "Black":
                    case_BG();

                    Background.sprite = Black;// 过场
                    People.gameObject.SetActive(false);
                    index++;
                    break;

                case "BG_Black":
                    case_BG();

                    Background.sprite = BarCounter;// 默认是酒馆背景
                    Lose_Black.SetActive(true);//黑色外圈
                    index++;
                    break;

                case "BG":
                    Background.sprite = BarCounter;// 默认是酒馆背景
                    case_BG();
                    index++;
                    break;
                case "DungeonEntrance":
                    Background.sprite = Background_DungeonEntrance;// 地下城入口
                    case_BG();
                    index++;
                    break;
                case "DungeonCorridor":
                    Background.sprite = Background_DungeonCorridor;// 地下城
                    case_BG();
                    index++;
                    break;
                case "Town":
                    Background.sprite = Background_Town;//乡镇
                    case_BG();

                    index++;
                    break;




                case "Shop":
                    Background.sprite = Background_Shop;//夜间小路
                    case_BG();

                    index++;
                    break;


                #endregion

                #region AVG角色
                case "Me":
                    text.color = new Color(0.0f, 0.68f, 0.93f, 1.0f);//蓝色
                    CleanNameText();
                    NameObject[0].SetActive(true);

                    voiceSource.Stop();


                    if (People.IsActive())
                    {
                        People_Anim.SetBool("Dark", true);
                    }//只有这里变

                    index++;
                    break;

                case "NPC_1":
                    text.color = new Color(0.7f, 0.75f, 0.8f, 1.0f); // 亮灰色
                    CleanNameText();
                    NameObject[5].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = NPC_1;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;

                case "NPC_2":
                    text.color = new Color(1.0f, 0.5f, 0.0f, 1.0f); // 橙色
                    CleanNameText();
                    NameObject[6].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = NPC_2;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;

                case "NPC_3":
                    text.color = new Color(1.0f, 1.0f, 0.0f, 1.0f); // 黄色
                    CleanNameText();
                    NameObject[13].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = NPC_3;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;

                case "NPC_4":
                    text.color = new Color(1.0f, 0.84f, 0.0f, 1.0f); // 金色
                    CleanNameText();
                    NameObject[14].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = NPC_4;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;


                case "ShopManager":
                    text.color = new Color(0.7f, 0.75f, 0.8f, 1.0f); // 亮灰色
                    CleanNameText();
                    NameObject[4].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = ShopManager;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;

                case "Andon":
                    text.color = new Color(0.65f, 0.75f, 0.65f, 1.0f); // 灰绿
                    CleanNameText();
                    NameObject[7].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = Andon;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;
                case "Enemy":
                    text.color = new Color(0.65f, 0.7f, 0.8f, 1.0f); // 灰蓝
                    CleanNameText();
                    NameObject[8].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = Enemy;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;
                case "XXX":
                    text.color = new Color(0.55f, 0.5f, 0.7f, 1.0f); // 暗紫灰
                    CleanNameText();
                    NameObject[9].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = XXX;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;
                case "Eirik":
                    text.color = new Color(0.75f, 0.65f, 0.55f, 1.0f); // 暖灰橙
                    CleanNameText();
                    NameObject[10].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = Eirik;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;

                case "MimicHades":
                    text.color = new Color(0.45f, 0.55f, 0.45f, 1.0f); // 暗绿灰
                    CleanNameText();
                    NameObject[11].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = MimicHades;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;


                case "Doll3":
                    text.color = new Color(0.55f, 0.7f, 0.7f, 1.0f); // 冷青灰
                    CleanNameText();
                    NameObject[12].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = Doll3;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;

                case "Jeweler":
                    text.color = new Color(0.68f, 0.85f, 0.9f, 1.0f); // 浅蓝色
                    CleanNameText();
                    NameObject[15].SetActive(true);

                    voiceSource.Stop();

                    People.gameObject.SetActive(true);
                    People.sprite = Jeweler;
                    People_Anim.SetBool("Dark", false);
                    index++;
                    break;


                #endregion

                #region AVG角色_安托

                case "Anto_Lose":
                    People.sprite = Anto_Lose;
                    case_Anto();
                    index++;
                    break;


                case "Anto_01":
                    People.sprite = Anto_01;
                    case_Anto();
                    index++;
                    break;


                case "Anto_02":
                    People.sprite = Anto_02;
                    case_Anto();
                    index++;
                    break;

                case "Anto_03":
                    People.sprite = Anto_03;
                    case_Anto();
                    index++;
                    break;


                case "Anto_04":
                    People.sprite = Anto_04;
                    case_Anto();
                    index++;
                    break;


                case "Anto_05":
                    People.sprite = Anto_05;
                    case_Anto();
                    index++;
                    break;


                case "Anto_06":
                    People.sprite = Anto_06;
                    case_Anto();
                    index++;
                    break;

                case "Anto_07":
                    People.sprite = Anto_07;
                    case_Anto();
                    index++;
                    break;

                case "Anto_08":
                    People.sprite = Anto_08;
                    case_Anto();
                    index++;
                    break;

                case "Anto_09":
                    People.sprite = Anto_09;
                    case_Anto();
                    index++;
                    break;

                case "Anto_10":
                    People.sprite = Anto_10;
                    case_Anto();
                    index++;
                    break;

                case "Anto_11":
                    People.sprite = Anto_11;
                    case_Anto();
                    index++;
                    break;

                case "Anto_12":
                    People.sprite = Anto_12;
                    case_Anto();
                    index++;
                    break;


                case "Anto_13":
                    People.sprite = Anto_13;
                    case_Anto();
                    index++;
                    break;


                case "Anto_14":
                    People.sprite = Anto_14;
                    case_Anto();
                    index++;
                    break;


                case "Anto_15":
                    People.sprite = Anto_15;
                    case_Anto();
                    index++;
                    break;


                case "Anto_16":
                    People.sprite = Anto_16;
                    case_Anto();
                    break;


                case "Anto_17":
                    People.sprite = Anto_17;
                    case_Anto();
                    index++;
                    break;


                case "Anto_18":
                    People.sprite = Anto_18;
                    case_Anto();
                    index++;
                    break;
                #endregion

                #region AVG角色_赫蒂


                case "Hetty_Lose":
                    People.sprite = Hetty_Lose;
                    case_Hetty();
                    index++;
                    break;



                case "Hetty_01":
                    People.sprite = Hetty_01;
                    case_Hetty();
                    index++;
                    break;


                case "Hetty_02":
                    People.sprite = Hetty_02;
                    case_Hetty();
                    index++;
                    break;

                case "Hetty_03":
                    People.sprite = Hetty_03;
                    case_Hetty();
                    index++;
                    break;


                case "Hetty_04":
                    People.sprite = Hetty_04;
                    case_Hetty();
                    index++;
                    break;


                case "Hetty_05":
                    People.sprite = Hetty_05;
                    case_Hetty();
                    index++;
                    break;


                case "Hetty_06":
                    People.sprite = Hetty_06;
                    case_Hetty();
                    index++;
                    break;

                case "Hetty_07":
                    People.sprite = Hetty_07;
                    case_Hetty();
                    index++;
                    break;

                case "Hetty_08":
                    People.sprite = Hetty_08;
                    case_Hetty();
                    index++;
                    break;



                #endregion

                #region AVG角色_爱丽丝

                case "Alice_Lose":
                    People.sprite = Alice_Lose;
                    case_Alice();
                    index++;
                    break;


                case "Alice_01":
                    People.sprite = Alice_01;
                    case_Alice();
                    index++;
                    break;

                case "Alice_02":
                    People.sprite = Alice_02;
                    case_Alice();
                    index++;
                    break;

                case "Alice_03":
                    People.sprite = Alice_03;
                    case_Alice();
                    index++;
                    break;

                case "Alice_04":
                    People.sprite = Alice_04;
                    case_Alice();
                    index++;
                    break;

                case "Alice_05":
                    People.sprite = Alice_05;
                    case_Alice();
                    index++;
                    break;

                case "Alice_06":
                    People.sprite = Alice_06;
                    case_Alice();
                    index++;
                    break;

                case "Alice_07":
                    People.sprite = Alice_07;
                    case_Alice();
                    index++;
                    break;

                case "Alice_08":
                    People.sprite = Alice_08;
                    case_Alice();
                    index++;
                    break;

                #endregion


                #region AVG特殊功能
                case "ShowRecipe":
                    text.color = Color.white;
                    CleanNameText();

                    voiceSource.Stop();

                    OpenRecipeShop();



                    //Background.sprite = Black;// 过场
                    //People.gameObject.SetActive(false);
                    index++;
                    break;


                    #endregion



            }

            //统合整理AVG角色

            //统合整理BG
            void case_BG()
            {
                text.color = Color.white;
                CleanNameText();

                voiceSource.Stop();

                if (People.IsActive())
                {
                    People_Anim.SetBool("Dark", true);
                }

            }


            //统合整理Anto
            void case_Anto()
            {
                text.color = new Color(1.0f, 0.2f, 0.5f, 1.0f); //浅红色
                CleanNameText();
                NameObject[1].SetActive(true);

                Girl_Voice();

                People.gameObject.SetActive(true);
                People_Anim.SetBool("Dark", false);
            }

            //统合整理Hetty
            void case_Hetty()
            {
                text.color = new Color(0.8f, 0.2f, 0.2f, 1.0f); // 深红色
                CleanNameText();
                NameObject[2].SetActive(true);

                //AntoVoice();

                People.gameObject.SetActive(true);

                People_Anim.SetBool("Dark", false);
            }


            //统合整理Alice
            void case_Alice()
            {
                text.color = Color.green; //绿色
                CleanNameText();
                NameObject[3].SetActive(true);

                Girl_Voice();

                People.gameObject.SetActive(true);

                People_Anim.SetBool("Dark", false);
            }






            int letter = 0;
            while (!cancelTyping && letter < textList[index].Length - 1)
            {
                textLabel.text += textList[index][letter];
                textLabel_2.text += textList[index][letter];////////////////////////////////////////
                letter++;
                yield return new WaitForSeconds(textSpeed);
            }

            textLabel.text = textList[index];
            textLabel_2.text = textList[index];////////////////////////////////////////////////
            cancelTyping = false;
            textFinished = true;
            index++;


            // ✅ 如果开启自动播放并且还有后续文本，就自动播放下一句
            if (autoMode && index < textList.Count)
            {
                yield return new WaitForSeconds(autoDelay); // 自动延迟
                ShowText(); // 自动下一句
            }
        }

        public void CleanNameText()
        {
            foreach (var nameObject in NameObject)
            {
                nameObject.SetActive(false);
            }
        }





        /// <summary>
        /// 插入声音
        /// </summary>
        #region

        [Header("插入声音")]
        [SerializeField] AudioSource voiceSource;
        [SerializeField] List<AudioClip> Playlist;//当前播放的列表

        [SerializeField] List<AudioClip> antoPlaylist_StartStory;

        [Header("安托声音")]
        #region 安托声音

        [SerializeField] List<AudioClip> antoPlaylist_CG_01_1;
        [SerializeField] List<AudioClip> antoPlaylist_CG_01_2;
        [SerializeField] List<AudioClip> antoPlaylist_CG_01_3;
        [SerializeField] List<AudioClip> antoPlaylist_CG_01_4;

        [SerializeField] List<AudioClip> antoPlaylist_CG_02_1;
        [SerializeField] List<AudioClip> antoPlaylist_CG_02_2;
        [SerializeField] List<AudioClip> antoPlaylist_CG_02_3;
        [SerializeField] List<AudioClip> antoPlaylist_CG_02_4;

        [SerializeField] List<AudioClip> antoPlaylist_CG_03_1;
        [SerializeField] List<AudioClip> antoPlaylist_CG_03_2;
        [SerializeField] List<AudioClip> antoPlaylist_CG_03_3;
        [SerializeField] List<AudioClip> antoPlaylist_CG_03_4;
        #endregion

        [Header("爱丽丝声音")]
        #region 爱丽丝声音

        [SerializeField] List<AudioClip> alicePlaylist_CG_01_1;
        //[SerializeField] List<AudioClip> alicePlaylist_CG_01_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_01_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_01_4;

        [SerializeField] List<AudioClip> alicePlaylist_CG_02_1;
        [SerializeField] List<AudioClip> alicePlaylist_CG_02_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_02_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_02_4;


        [SerializeField] List<AudioClip> alicePlaylist_CG_03_1;
        //[SerializeField] List<AudioClip> alicePlaylist_CG_03_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_03_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_03_4;



        [SerializeField] List<AudioClip> alicePlaylist_CG_04_1;
        //[SerializeField] List<AudioClip> alicePlaylist_CG_04_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_04_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_04_4;



        [SerializeField] List<AudioClip> alicePlaylist_CG_05_1;
        //[SerializeField] List<AudioClip> alicePlaylist_CG_05_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_05_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_05_4;



        [SerializeField] List<AudioClip> alicePlaylist_CG_06_1;
        [SerializeField] List<AudioClip> alicePlaylist_CG_06_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_06_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_06_4;


        [SerializeField] List<AudioClip> alicePlaylist_CG_07_1;
        //[SerializeField] List<AudioClip> alicePlaylist_CG_07_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_07_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_07_4;


        [SerializeField] List<AudioClip> alicePlaylist_CG_08_1;
        //[SerializeField] List<AudioClip> alicePlaylist_CG_08_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_08_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_08_4;


        [SerializeField] List<AudioClip> alicePlaylist_CG_09_1;
        [SerializeField] List<AudioClip> alicePlaylist_CG_09_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_09_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_09_4;

        [SerializeField] List<AudioClip> alicePlaylist_CG_10_1;
        [SerializeField] List<AudioClip> alicePlaylist_CG_10_2;
        [SerializeField] List<AudioClip> alicePlaylist_CG_10_3;
        [SerializeField] List<AudioClip> alicePlaylist_CG_10_4;

        #endregion


        int VoiceIndex = 0;


        public void Girl_Voice()
        {

            // 1) 选择「当前台词列表」
            //List<AudioClip> newList = null;

            switch (animation_number)
            {
                case 1:
                    Playlist = antoPlaylist_StartStory;
                    break;

                #region 安托声音调用

                case 1011:
                    Playlist = antoPlaylist_CG_01_1;
                    break;
                case 1012:
                    Playlist = antoPlaylist_CG_01_2;
                    break;
                case 1013:
                    Playlist = antoPlaylist_CG_01_3;
                    break;
                case 1014:
                    Playlist = antoPlaylist_CG_01_4;
                    break;



                case 1021:
                    Playlist = antoPlaylist_CG_02_1;
                    break;
                case 1022:
                    Playlist = antoPlaylist_CG_02_2;
                    break;
                case 1023:
                    Playlist = antoPlaylist_CG_02_3;
                    break;
                case 1024:
                    Playlist = antoPlaylist_CG_02_4;
                    break;



                case 1031:
                    Playlist = antoPlaylist_CG_03_1;
                    break;
                case 1032:
                    Playlist = antoPlaylist_CG_03_2;
                    break;
                case 1033:
                    Playlist = antoPlaylist_CG_03_3;
                    break;
                case 1034:
                    Playlist = antoPlaylist_CG_03_4;
                    break;

                #endregion

                #region 爱丽丝声音调用

                case 3011:
                    Playlist = alicePlaylist_CG_01_1;
                    break;
                //case 3012:
                //    Playlist = alicePlaylist_CG_01_2;
                //    break;
                case 3013:
                    Playlist = alicePlaylist_CG_01_3;
                    break;
                case 3014:
                    Playlist = alicePlaylist_CG_01_4;
                    break;



                case 3021:
                    Playlist = alicePlaylist_CG_02_1;
                    break;
                case 3012:
                    Playlist = alicePlaylist_CG_02_2;
                    break;
                case 3023:
                    Playlist = alicePlaylist_CG_02_3;
                    break;
                case 3024:
                    Playlist = alicePlaylist_CG_02_4;
                    break;



                case 3031:
                    Playlist = alicePlaylist_CG_03_1;
                    break;
               //case 3032:
               //    Playlist = alicePlaylist_CG_03_2;
               //    break;
                case 3033:
                    Playlist = alicePlaylist_CG_03_3;
                    break;
                case 3034:
                    Playlist = alicePlaylist_CG_03_4;
                    break;



                case 3041:
                    Playlist = alicePlaylist_CG_04_1;
                    break;
               //case 3042:
               //    Playlist = alicePlaylist_CG_04_2;
               //    break;
                case 3043:
                    Playlist = alicePlaylist_CG_04_3;
                    break;
                case 3044:
                    Playlist = alicePlaylist_CG_04_4;
                    break;



                case 3051:
                    Playlist = alicePlaylist_CG_05_1;
                    break;
                //case 3052:
                //    Playlist = alicePlaylist_CG_05_2;
                //    break;
                case 3053:
                    Playlist = alicePlaylist_CG_05_3;
                    break;
                case 3054:
                    Playlist = alicePlaylist_CG_05_4;
                    break;



                case 3061:
                    Playlist = alicePlaylist_CG_06_1;
                    break;
                case 3062:
                    Playlist = alicePlaylist_CG_06_2;
                    break;
                case 3063:
                    Playlist = alicePlaylist_CG_06_3;
                    break;
                case 3064:
                    Playlist = alicePlaylist_CG_06_4;
                    break;



                case 3071:
                    Playlist = alicePlaylist_CG_07_1;
                    break;
                //case 3072:
                //    Playlist = alicePlaylist_CG_07_2;
                //    break;
                case 3073:
                    Playlist = alicePlaylist_CG_07_3;
                    break;
                case 3074:
                    Playlist = alicePlaylist_CG_07_4;
                    break;




                case 3081:
                    Playlist = alicePlaylist_CG_08_1;
                    break;
                //case 3082:
                //    Playlist = alicePlaylist_CG_08_2;
                //    break;
                case 3083:
                    Playlist = alicePlaylist_CG_08_3;
                    break;
                case 3084:
                    Playlist = alicePlaylist_CG_08_4;
                    break;



                case 3091:
                    Playlist = alicePlaylist_CG_09_1;
                    break;
                case 3092:
                    Playlist = alicePlaylist_CG_09_2;
                    break;
                case 3093:
                    Playlist = alicePlaylist_CG_09_3;
                    break;
                case 3094:
                    Playlist = alicePlaylist_CG_09_4;
                    break;




                case 3101:
                    Playlist = alicePlaylist_CG_10_1;
                    break;
                case 3102:
                    Playlist = alicePlaylist_CG_10_2;
                    break;
                case 3103:
                    Playlist = alicePlaylist_CG_10_3;
                    break;
                case 3104:
                    Playlist = alicePlaylist_CG_10_4;
                    break;

                    #endregion












            }

            // 如果列表切换了，重置索引
            //if (newList != lastPlaylist)
            //{
            //    antoPlaylist = newList;
            //    VoiceIndex = 0;
            //    lastPlaylist = newList;
            //}
            //else
            //{
            //    antoPlaylist = newList;
            //}






            if (Playlist == null || Playlist.Count == 0)
            {
                Debug.LogWarning("Anto playlist is empty.");
                return;
            }


            if (VoiceIndex >= Playlist.Count)
            {
                Debug.Log("Anto playlist finished.");
                return;
            }


            // 2) 播放台词前：暂停呻吟循环
            PauseMoanLoop();
            if (resumeCo != null) StopCoroutine(resumeCo);



            // 3) 播台词
            var clip = Playlist[VoiceIndex++];
            voiceSource.Stop();
            voiceSource.clip = clip;
            voiceSource.Play();


            // 4) 台词结束后：自动恢复呻吟循环
            if (resumeMoanAfterVoice)
                resumeCo = StartCoroutine(ResumeMoanAfterVoiceCo());

        }

        // 等 voiceSource 播完，再恢复 moanLoop
        private IEnumerator ResumeMoanAfterVoiceCo()
        {
            // 避免 pitch / 重新触发导致误判，改用 isPlaying 轮询最稳
            yield return new WaitWhile(() => voiceSource != null && voiceSource.isPlaying);
            ResumeMoanLoop();
        }

        #endregion




        /// <summary>
        /// 循环呻吟声效
        /// </summary>
        #region

        [Header("安托CG_01_娇喘音频列表")]
        public List<AudioClip> moan_Clips = new List<AudioClip>();
        [Header("安托CG_01_剧烈娇喘音频列表")]
        public List<AudioClip> moanLoud_Clips = new List<AudioClip>();
        [Header("安托CG_02_触手娇喘音频列表")]
        public List<AudioClip> moanTentacle_Clips = new List<AudioClip>();
        [Header("安托CG_02_触手口交音频列表")]
        public List<AudioClip> FeraTentacle_Clips = new List<AudioClip>();
        [Header("安托CG_02_剧烈触手口交音频列表")]
        public List<AudioClip> FeraTentacleLoud_Clips = new List<AudioClip>();
        [Header("安托CG_03_钢管舞喘息音频列表")]
        public List<AudioClip> moanPoleDance_Clips = new List<AudioClip>();
        [Header("安托CG_03_钢管舞剧烈喘息音频列表")]
        public List<AudioClip> moanLoudPoleDance_Clips = new List<AudioClip>();
        [Header("安托CG_03_昏厥喘息音频列表")]
        public List<AudioClip> FaintingGasp_Clips = new List<AudioClip>();




        [Header("爱丽丝CG_01_娇喘音频列表")]
        public List<AudioClip> ailce_01_moan_Clips = new List<AudioClip>();
        public List<AudioClip> ailce_01_moanLoad_Clips = new List<AudioClip>();
        public List<AudioClip> ailce_01_FaintingGasp_Clips = new List<AudioClip>();



        // —— 新增字段 —— //
        [Header("Loop(呻吟)播放器")]
        [SerializeField] private AudioSource moanLoop;   // 用来循环的那个
        [SerializeField] private bool resumeMoanAfterVoice = true;

        private Coroutine resumeCo;
        private List<AudioClip> lastPlaylist;            // 记录上一次的播放列表，便于换段时重置索引

        // 随机从列表里挑一个作为循环
        private void PlayMoanLoop(List<AudioClip> pool)
        {
            if (pool == null || pool.Count == 0) return;

            var clip = pool[Random.Range(0, pool.Count)];
            moanLoop.clip = clip;
            moanLoop.loop = true;
            moanLoop.Play();
        }

        private void PauseMoanLoop()
        {
            if (moanLoop != null && moanLoop.isPlaying) moanLoop.Pause();
        }

        private void ResumeMoanLoop()
        {
            if (moanLoop != null && moanLoop.clip != null) moanLoop.UnPause();
        }

        #endregion


        /// <summary>
        /// 结束对话触发
        /// </summary>
        #region


        [Header("当对话结束时需要触发的一些地方")]
        public UIManager uiManager;

        //快进按钮触发在这里
        public void ChangeStory()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            switch (animation_number)
            {
                //开场白
                case 1:

                    //三人都得记录
                    data.antoProgress = 1;
                    data.hettyProgress = 1;
                    data.aliceProgress = 1;


                    SaveManager.SaveGame(data);


                    GameFlowData.nextAVGId = "StartWork_01";//故事背景结束，开启经营AVG
                    uiManager.LoadingScene_Spine();
                    break;

                //开启一天工作
                case 100:
                    uiManager.LoadingScene_BarCounter();

                    break;

                //商人不出现
                case 10:
                case 12:
                case 13:
                    GameFlowData.nextAVGId = "StartWork_01";//开启经营AVG
                    uiManager.LoadingScene_Spine();
                    break;

                //商人出现
                case 11:                
                    uiManager.LoadingScene_Shop();
                    break;



                #region  安托

                //被安托击败
                case 1014:
                case 1024:
                case 1034:
                case 1044:
                case 1054:
                case 1064:
                case 1074:
                case 1084:
                case 1094:
                case 1104:
                    RandomToShop();
                    break;



                //击败安托
                case 1012:
                    GameFlowData.nextAVGId = "Anto_CG_01_3";//开启安托第一个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1022:
                    GameFlowData.nextAVGId = "Anto_CG_02_3";//开启安托第二个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1032:
                    GameFlowData.nextAVGId = "Anto_CG_03_3";//开启安托第三个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1042:
                    GameFlowData.nextAVGId = "Anto_CG_04_3";//开启安托第四个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1052:
                    GameFlowData.nextAVGId = "Anto_CG_05_3";//开启安托第五个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1062:
                    GameFlowData.nextAVGId = "Anto_CG_06_3";//开启安托第六个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1072:
                    GameFlowData.nextAVGId = "Anto_CG_07_3";//开启安托第七个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1082:
                    GameFlowData.nextAVGId = "Anto_CG_08_3";//开启安托第八个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1092:
                    GameFlowData.nextAVGId = "Anto_CG_09_3";//开启安托第九个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 1102:
                    GameFlowData.nextAVGId = "Anto_CG_10_3";//开启安托第十个CG
                    uiManager.LoadingScene_Spine();
                    break;



                //安托CG结束
                case 1013:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托01）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 1; 



                        data.antoProgress = 2;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1023:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托02）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 2; 



                        data.antoProgress = 3;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1033:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托03）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 3;


                        data.antoProgress = 4;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1043:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托04）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 4;


                        data.antoProgress = 5;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1053:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托05）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 5;


                        data.antoProgress = 6;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1063:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托06）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 6;


                        data.antoProgress = 7;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1073:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托07）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 7;

                        data.antoProgress = 8;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1083:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托08）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 8;


                        data.antoProgress = 9;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1093:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托09）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 9;


                        data.antoProgress = 10;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 1103:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（安托10）
                        data.lastCGGirl = 1;   // 1=安托
                        data.lastCGIndex = 10;

                        //安托已经通关   
                        data.antoProgress = 11;
                        SaveManager.SaveGame(data);

                        CheckClean();//检测三位女荷官是否通关

                        RandomToShop();
                    }
                    break;

                #endregion


                #region  赫蒂

                //被赫蒂击败
                case 2014:
                case 2024:
                case 2034:
                case 2044:
                case 2054:
                case 2064:
                case 2074:
                case 2084:
                case 2094:
                case 2104:
                    RandomToShop();
                    break;



                //击败赫蒂
                case 2012:
                    GameFlowData.nextAVGId = "Hetty_CG_01_3";//开启赫蒂第一个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2022:
                    GameFlowData.nextAVGId = "Hetty_CG_02_3";//开启赫蒂第二个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2032:
                    GameFlowData.nextAVGId = "Hetty_CG_03_3";//开启赫蒂第三个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2042:
                    GameFlowData.nextAVGId = "Hetty_CG_04_3";//开启赫蒂第四个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2052:
                    GameFlowData.nextAVGId = "Hetty_CG_05_3";//开启赫蒂第五个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2062:
                    GameFlowData.nextAVGId = "Hetty_CG_06_3";//开启赫蒂第六个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2072:
                    GameFlowData.nextAVGId = "Hetty_CG_07_3";//开启赫蒂第七个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2082:
                    GameFlowData.nextAVGId = "Hetty_CG_08_3";//开启赫蒂第八个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2092:
                    GameFlowData.nextAVGId = "Hetty_CG_09_3";//开启赫蒂第九个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 2102:
                    GameFlowData.nextAVGId = "Hetty_CG_10_3";//开启赫蒂第十个CG
                    uiManager.LoadingScene_Spine();
                    break;



                //赫蒂CG结束
                case 2013:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂01）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 1;


                        data.hettyProgress = 2;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2023:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂02）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 2;


                        data.hettyProgress = 3;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2033:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂03）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 3;


                        data.hettyProgress = 4;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2043:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂04）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 4;


                        data.hettyProgress = 5;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2053:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂05）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 5;


                        data.hettyProgress = 6;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2063:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂06）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 6;


                        data.hettyProgress = 7;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2073:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂07）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 7;


                        data.hettyProgress = 8;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2083:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂08）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 8;


                        data.hettyProgress = 9;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2093:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂09）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 9;


                        data.hettyProgress = 10;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 2103:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（赫蒂10）
                        data.lastCGGirl = 2;   // 2=赫蒂
                        data.lastCGIndex = 10;

                        //赫蒂已经通关
                        data.hettyProgress = 10;
                        SaveManager.SaveGame(data);

                        CheckClean();//检测三位女荷官是否通关

                        RandomToShop();
                    }
                    break;

                #endregion

                #region  爱丽丝

                //被爱丽丝击败
                case 3014:
                case 3024:
                case 3034:
                case 3044:
                case 3054:
                case 3064:
                case 3074:
                case 3084:
                case 3094:
                case 3104:
                    RandomToShop();
                    break;



                //击败爱丽丝
                case 3012:
                    GameFlowData.nextAVGId = "Alice_CG_01_3";//开启爱丽丝第一个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3022:
                    GameFlowData.nextAVGId = "Alice_CG_02_3";//开启爱丽丝第二个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3032:
                    GameFlowData.nextAVGId = "Alice_CG_03_3";//开启爱丽丝第三个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3042:
                    GameFlowData.nextAVGId = "Alice_CG_04_3";//开启爱丽丝第四个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3052:
                    GameFlowData.nextAVGId = "Alice_CG_05_3";//开启爱丽丝第五个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3062:
                    GameFlowData.nextAVGId = "Alice_CG_06_3";//开启爱丽丝第六个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3072:
                    GameFlowData.nextAVGId = "Alice_CG_07_3";//开启爱丽丝第七个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3082:
                    GameFlowData.nextAVGId = "Alice_CG_08_3";//开启爱丽丝第八个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3092:
                    GameFlowData.nextAVGId = "Alice_CG_09_3";//开启爱丽丝第九个CG
                    uiManager.LoadingScene_Spine();
                    break;
                case 3102:
                    GameFlowData.nextAVGId = "Alice_CG_10_3";//开启爱丽丝第十个CG
                    uiManager.LoadingScene_Spine();
                    break;



                //爱丽丝CG结束
                case 3013:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝01）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 1;


                        data.aliceProgress = 2;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3023:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝02）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 2;

                        data.aliceProgress = 3;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3033:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝03）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 3;

                        data.aliceProgress = 4;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3043:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝04）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 4;

                        data.aliceProgress = 5;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3053:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝05）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 5;

                        data.aliceProgress = 6;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3063:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝06）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 6;

                        data.aliceProgress = 7;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3073:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝07）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 7;

                        data.aliceProgress = 8;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3083:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝08）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 8;

                        data.aliceProgress = 9;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3093:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝09）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 9;

                        data.aliceProgress = 10;
                        SaveManager.SaveGame(data);

                        RandomToShop();
                    }
                    break;
                case 3103:
                    if (GameFlowData.returnPath == "cg")
                    {
                        uiManager.LoadingScene_Lobby();
                    }
                    else
                    {
                        // ✅ 记录上一局CG缩略图（爱丽丝10）
                        data.lastCGGirl = 3;   // 3=爱丽丝
                        data.lastCGIndex = 10;

                        //爱丽丝已经通关
                        data.aliceProgress = 11;
                        SaveManager.SaveGame(data);

                        CheckClean();//检测三位女荷官是否通关

                        RandomToShop();
                    }
                    break;

                #endregion




                //所有的_1都在这里触发
                default:
                    uiManager.LoadingScene_BJ_Mobile();//开始对战
                    break;

                    //CG暂时这么做
                    //case 1001:
                    //    uiManager.LoadingScene_Lobby();//回主菜单
                    //    break;
            }



        }

        void RandomToShop()
        {
            bool allUnlocked = IsAllRecipesUnlocked();

            int roll = Random.Range(0, 100);

            if (!allUnlocked)
            {
                // 还没集满酒：允许配方商店
                if (roll < 30)
                {
                    //配方商人出现
                    GameFlowData.nextAVGId = "StartRecipe";   // 30%
                }
                else if (roll < 70)
                {
                    //商人出现
                    GameFlowData.nextAVGId = "StartShop_02";  // 40%
                }
                else
                {
                    //无事发生
                    GameFlowData.nextAVGId = "StartShop_01";  // 30%
                }
            }
            else
            {

                Debug.Log("已经买齐所有酒，不需要再去买配方了");

                // 已集满酒：彻底禁止配方商店
                if (roll < 60)
                {
                    //商人出现
                    GameFlowData.nextAVGId = "StartShop_02";  // 60%
                }
                else
                {
                    //无事发生
                    GameFlowData.nextAVGId = "StartShop_01";  // 40%
                }
            }


            uiManager.LoadingScene_Spine();
        }//根据酒品来选择去不去酒品商店

        bool IsAllRecipesUnlocked()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            if (data.unlockedDrinkNames == null)
                return false;

            return data.unlockedDrinkNames.Count >= 10;//这是特殊酒总数
        }



        void CheckClean() 
        {

            //如果本局胜利时，三位女荷官都进度已满，打开【通关】记录
            // ===== 通关判定 =====
            var data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            bool allMax =
                data.antoProgress >= 11 &&
                data.hettyProgress >= 11 &&
                data.aliceProgress >= 11;

            if (allMax && !data.HasCleared)
            {
                data.HasCleared = true;



                SaveManager.SaveGame(data);

                Debug.Log("【通关达成】三位女荷官进度已满");
            }
        }


        #endregion



        /// <summary>
        /// 配方商店
        /// </summary>
        #region

        [Header("配方商店")]
        public GameObject RecipePanel;//配方商店界面

        public void BuyRecipe(int Wine_Number)
        {
            // 找到对应条目
            var item = shopItems.Find(x => x.wineNumber == Wine_Number);
            if (item == null)
            {
                Debug.LogWarning("未找到商店条目 Wine_Number=" + Wine_Number);
                return;
            }

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();

            // 已解锁就直接返回（防重复点）
            if (data.unlockedDrinkNames.Contains(item.drinkName))
            {
                Debug.Log("已解锁：" + item.drinkName);
                RefreshRecipeShopUI();
                return;
            }

            // 钱够不够：你项目现在 balance 是 SaveData 里有的
            if (data.balance < item.cost)
            {
                Debug.Log("钱不够，需：" + item.cost);
                // 可在这里播放拒绝SE / 弹提示
                AudioManager_2.SoundPlay(4);
                return;
            }

            // ✅ 扣钱：用 BalanceManager 统一做（会更新UI+写存档）
            BalanceManager.ChangeBalance(-item.cost);

            // ✅ 关键：重新 Load 一次最新存档，避免被旧 data 覆盖
            data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();



            // 解锁
            data.unlockedDrinkNames.Add(item.drinkName);

            // 存档
            SaveManager.SaveGame(data);



            // 刷商店UI + 刷解锁图标（如果你做了解锁图标列表）
            RefreshRecipeShopUI();

            AudioManager_2.SoundPlay(3); // 成功音效
            Debug.Log("购买并解锁：" + item.drinkName);

        }


        //public bool UnlockRecipe(string drinkName)
        //{
        //    SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
        //
        //    if (data.unlockedDrinkNames == null)
        //        data.unlockedDrinkNames = new List<string>();
        //
        //    if (data.unlockedDrinkNames.Contains(drinkName))
        //        return false;
        //
        //    data.unlockedDrinkNames.Add(drinkName);
        //
        //    SaveManager.SaveGame(data);              // ✅只传 data
        //    //RefreshUnlockedRecipesFromSave();
        //    return true;
        //}//存入酒品


        [Header("配方商店条目")]
        public List<RecipeShopItem> shopItems = new List<RecipeShopItem>();

        [System.Serializable]
        public class RecipeShopItem
        {
            public int wineNumber;        // 按钮传入用（1~7）
            public string drinkName;      // 存档key：例如 "魔女之吻"
            public float cost;            // 售价
            public GameObject rowGO;      // 这一行UI（整行隐藏用）
            public Text costText;     // 价格文本（可选）
        }
        public void RefreshRecipeShopUI()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();

            foreach (var item in shopItems)
            {
                bool unlocked = data.unlockedDrinkNames.Contains(item.drinkName);

                // 已解锁：整行隐藏（你要的“不显示”）
                if (item.rowGO != null)
                    item.rowGO.SetActive(!unlocked);

                // 如果要动态显示价格
               //if (item.costText != null)
               //    item.costText.text = ((int)item.cost).ToString();
            }
        }//更新酒品

        public void OpenRecipeShop()
        {
            RecipePanel.SetActive(true);
            RefreshRecipeShopUI();
        }//打开商店
        #endregion
    }
}
