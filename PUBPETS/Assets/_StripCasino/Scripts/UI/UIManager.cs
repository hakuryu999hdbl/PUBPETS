using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
namespace Blackjack_Game
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        void Awake()
        {
            instance = this;
        }



        /// <summary>
        /// 主菜单使用UI
        /// </summary>
        #region
        [Header("主菜单使用UI")]
        public GameObject CG_Thumbnail_Menu;
        private void Start()
        {
            Scene currentScene = SceneManager.GetActiveScene(); // 获取当前场景
            if (currentScene.name == "Lobby")
            {
                OnTabClick("System");//主菜单的设置中预先设置为系统版面

                allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));//键位设置

                LoadKeyBindings(); // 在游戏开始时加载键位设置



                CheckAndShowAntoCG();//根据当前存档来解锁CG进度

                CheckTextSpeed();//检测文字加载速度，默认为0.05f

                if (PlayerPrefs.GetFloat("TextSpeed") == 0)
                {
                    PlayerPrefs.SetFloat("TextSpeed", 0.05f);
                }//检测文字加载速度，默认为0.05f

                if (GameFlowData.returnPath == "cg")
                {
                    CG_Thumbnail_Menu.SetActive(true);
                    GameFlowData.returnPath = null;
                }//观赏完CG回主菜单































                if (Application.platform == RuntimePlatform.Android)
                {
                    Debug.Log("当前是 Android");
                }
                else
                {
                    Debug.Log("当前是 PC");

                    StartSetDisplayMode();//根据存档设置对应屏幕以及分辨率

                    GetResolutionIndex_Text();//设置屏幕分辨率文字

                    if (PlayerPrefs.GetInt("Setting_AllowBackgroundRunning") == 0)
                    {
                        isAllowedBackgroundRunning = true; // 允许

                    }//检测允许游戏在后台运行

                    AllowBackgroundRunning();
                }


            }//主菜单的设置

            Debug.Log("目前储存的语言" + PlayerPrefs.GetInt("language"));//0日语 1简体中文 2繁体中文 3英语 4韩语

            //Debug.Log("目前储存的Hit按键设置" + PlayerPrefs.GetString("KeyBindings_Hit"));
            //Debug.Log("目前储存的Stand按键设置: " + PlayerPrefs.GetString("KeyBindings_Stand"));
            //Debug.Log("目前储存的DoubleDown按键设置: " + PlayerPrefs.GetString("KeyBindings_DoubleDown"));
            //Debug.Log("目前储存的Skip按键设置: " + PlayerPrefs.GetString("KeyBindings_Skip"));
            //Debug.Log("目前储存的Confirm按键设置: " + PlayerPrefs.GetString("KeyBindings_Confirm"));
            //Debug.Log("目前储存的Back按键设置: " + PlayerPrefs.GetString("KeyBindings_Back"));


            Debug.Log("目前储存的AVG对话框文字速度" + PlayerPrefs.GetFloat("TextSpeed"));

            // Debug.Log("目前储存的窗口设置" + PlayerPrefs.GetInt("Setting_Windows"));//0全屏 1窗口
            // Debug.Log("目前储存的是否允许后台运行" + PlayerPrefs.GetInt("Setting_AllowBackgroundRunning"));//0允许 1不允许


            Debug.Log("UIManager.要播放的AVG是：" + GameFlowData.nextAVGId);


            //ーーーーーーーーーーーーーーーーーーーーー手动测试AVGーーーーーーーーーーーーーーーーーーーーーーーー
            //GameFlowData.nextAVGId = "Hetty_CG_10_3";//获胜失败和CG使用
            //GameFlowData.nextAVGId = "VSHetty";
            //BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
            //BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
            //BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
            
            //Load_AVG(2104);//VS对应女荷官使用
           


            if (currentScene.name == "Spine")
            {
                BGM.instance.Stop();


                //播放AV媒介
                switch (GameFlowData.nextAVGId)
                {

                    case "StartStory_01":
                        Load_AVG(1);//开始剧情
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;

                    case "StartWork_01":
                        Load_AVG(100);//开始工作
                        BGM.instance.AudioPlayBackgroundMusic(11);//女荷官指名AVG音乐

                        OnEnterTavern();//每次进入酒店经营AVG界面刷新

                        break;

                    case "StartShop_01":
                        Load_AVG(10);//开始商家界面1（商人不出现）
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;
                    case "StartShop_02":
                        Load_AVG(11);//开始商家界面2（商人出现）
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;


                    case "StartRecipe":

                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            Load_AVG(12);//神秘人购买配方
                        }
                        else
                        {
                            Load_AVG(13);//珠宝商购买配方
                        }

                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;


                    #region  Anto
                    case "VSAnto":
                        Load_Vs_Anto_AVG();//对决安托[所有Anto_CG_XX_1开端]
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_Failure":
                        Load_Anto_Lose_AVG();//输给安托
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;

                    case "Anto_CG_01_2":
                        Load_AVG(1012);//开启安托第一个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_01_3":
                        Load_AVG(1013);//开启安托第一个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(11);
                        BGM.instance.AudioPlayBackgroundMusic(5);//CG地下城入口
                        break;

                    case "Anto_CG_02_2":
                        Load_AVG(1022);//开启安托第二个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_02_3":
                        Load_AVG(1023);//开启安托第二个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(12);
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;

                    case "Anto_CG_03_2":
                        Load_AVG(1032);//开启安托第三个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_03_3":
                        Load_AVG(1033);//开启安托第三个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(13);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Anto_CG_04_2":
                        Load_AVG(1042);//开启安托第四个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_04_3":
                        Load_AVG(1043);//开启安托第四个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(14);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Anto_CG_05_2":
                        Load_AVG(1052);//开启安托第五个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_05_3":
                        Load_AVG(1053);//开启安托第五个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(15);
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;

                    case "Anto_CG_06_2":
                        Load_AVG(1062);//开启安托第六个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_06_3":
                        Load_AVG(1063);//开启安托第六个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(16);
                        BGM.instance.AudioPlayBackgroundMusic(5);//CG地下城入口
                        break;

                    case "Anto_CG_07_2":
                        Load_AVG(1072);//开启安托第七个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_07_3":
                        Load_AVG(1073);//开启安托第七个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(17);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Anto_CG_08_2":
                        Load_AVG(1082);//开启安托第八个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_08_3":
                        Load_AVG(1083);//开启安托第八个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(18);
                        BGM.instance.AudioPlayBackgroundMusic(5);//CG地下城入口
                        break;

                    case "Anto_CG_09_2":
                        Load_AVG(1092);//开启安托第九个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_09_3":
                        Load_AVG(1093);//开启安托第九个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(19);
                        BGM.instance.AudioPlayBackgroundMusic(5);//CG地下城入口
                        break;

                    case "Anto_CG_10_2":
                        Load_AVG(1102);//开启安托第十个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(3);//安托AVG音乐
                        break;
                    case "Anto_CG_10_3":
                        Load_AVG(1103);//开启安托第十个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(20);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;
                    #endregion

                    #region  Alice
                    case "VSAlice":
                        Load_Vs_Alice_AVG();//对决爱丽丝[所有Alice_CG_XX_1开端]
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_Failure":
                        Load_Alice_Lose_AVG();//输给爱丽丝
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;

                    case "Alice_CG_01_2":
                        Load_AVG(3012);//开启爱丽丝第一个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_01_3":
                        Load_AVG(3013);//开启爱丽丝第一个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(31);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_02_2":
                        Load_AVG(3022);//开启爱丽丝第二个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_02_3":
                        Load_AVG(3023);//开启爱丽丝第二个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(32);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_03_2":
                        Load_AVG(3032);//开启爱丽丝第三个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_03_3":
                        Load_AVG(3033);//开启爱丽丝第三个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(33);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_04_2":
                        Load_AVG(3042);//开启爱丽丝第四个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_04_3":
                        Load_AVG(3043);//开启爱丽丝第四个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(34);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_05_2":
                        Load_AVG(3052);//开启爱丽丝第五个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_05_3":
                        Load_AVG(3053);//开启爱丽丝第五个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(35);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_06_2":
                        Load_AVG(3062);//开启爱丽丝第六个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_06_3":
                        Load_AVG(3063);//开启爱丽丝第六个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(36);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_07_2":
                        Load_AVG(3072);//开启爱丽丝第七个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_07_3":
                        Load_AVG(3073);//开启爱丽丝第七个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(37);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_08_2":
                        Load_AVG(3082);//开启爱丽丝第八个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_08_3":
                        Load_AVG(3083);//开启爱丽丝第八个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(38);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_09_2":
                        Load_AVG(3092);//开启爱丽丝第九个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_09_3":
                        Load_AVG(3093);//开启爱丽丝第九个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(39);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Alice_CG_10_2":
                        Load_AVG(3102);//开启爱丽丝第十个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(9);//爱丽丝AVG音乐
                        break;
                    case "Alice_CG_10_3":
                        Load_AVG(3103);//开启爱丽丝第十个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(40);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;
                    #endregion

                    #region  Hetty
                    case "VSHetty":
                        Load_Vs_Hetty_AVG();//对决赫蒂[所有Hetty_CG_XX_1开端]
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_Failure":
                        Load_Hetty_Lose_AVG();//输给赫蒂
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;

                    case "Hetty_CG_01_2":
                        Load_AVG(2012);//开启赫蒂第一个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_01_3":
                        Load_AVG(2013);//开启赫蒂第一个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(21);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Hetty_CG_02_2":
                        Load_AVG(2022);//开启赫蒂第二个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_02_3":
                        Load_AVG(2023);//开启赫蒂第二个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(22);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Hetty_CG_03_2":
                        Load_AVG(2032);//开启赫蒂第三个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_03_3":
                        Load_AVG(2033);//开启赫蒂第三个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(23);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Hetty_CG_04_2":
                        Load_AVG(2042);//开启赫蒂第四个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_04_3":
                        Load_AVG(2043);//开启赫蒂第四个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(24);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Hetty_CG_05_2":
                        Load_AVG(2052);//开启赫蒂第五个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_05_3":
                        Load_AVG(2053);//开启赫蒂第五个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(25);
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;

                    case "Hetty_CG_06_2":
                        Load_AVG(2062);//开启赫蒂第六个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_06_3":
                        Load_AVG(2063);//开启赫蒂第六个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(26);
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;

                    case "Hetty_CG_07_2":
                        Load_AVG(2072);//开启赫蒂第七个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_07_3":
                        Load_AVG(2073);//开启赫蒂第七个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(27);
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;

                    case "Hetty_CG_08_2":
                        Load_AVG(2082);//开启赫蒂第八个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_08_3":
                        Load_AVG(2083);//开启赫蒂第八个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(28);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;

                    case "Hetty_CG_09_2":
                        Load_AVG(2092);//开启赫蒂第九个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_09_3":
                        Load_AVG(2093);//开启赫蒂第九个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(29);
                        BGM.instance.AudioPlayBackgroundMusic(6);//CG地下城环境音
                        break;

                    case "Hetty_CG_10_2":
                        Load_AVG(2102);//开启赫蒂第十个CG前端AVG
                        BGM.instance.AudioPlayBackgroundMusic(7);//赫蒂AVG音乐
                        break;
                    case "Hetty_CG_10_3":
                        Load_AVG(2103);//开启赫蒂第十个CG
                        dialog.spine_FrameEvents.SetCurrentAnimator(30);
                        BGM.instance.AudioPlayBackgroundMusic(4);//CG酒馆环境音
                        break;
                        #endregion
                }

            }


            // 读取本地保存的音量数值并应用
            if (PlayerPrefs.HasKey("SE_Volume"))
            {
                float v = PlayerPrefs.GetFloat("SE_Volume");
                audioMixer.SetFloat("MainVolume", v);
                Debug.Log("读取到SE音量: " + v);
            }
            if (PlayerPrefs.HasKey("BGM_Volume"))
            {
                float v = PlayerPrefs.GetFloat("BGM_Volume");
                audioMixer_2.SetFloat("MainVolume_2", v);
                Debug.Log("读取到BGM音量: " + v);
            }
            if (PlayerPrefs.HasKey("Voice_Volume"))
            {
                float v = PlayerPrefs.GetFloat("Voice_Volume");
                audioMixer_3.SetFloat("MainVolume_3", v);
                Debug.Log("读取到Voice音量: " + v);
            }

            slider_SE.value = PlayerPrefs.GetFloat("SE_Volume"); // 让滑杆跳到保存的位置
            slider_BGM.value = PlayerPrefs.GetFloat("BGM_Volume"); // 让滑杆跳到保存的位置
            slider_Voice.value = PlayerPrefs.GetFloat("Voice_Volume"); // 让滑杆跳到保存的位置



            if (SceneManager.GetActiveScene().name != "BarCounter")
            {
                WaitToNormal();//开头加载就鼠标变化
            }




        }

















        public Image Title_Setting_System, Title_Setting_Audio, Title_Setting_Display, Title_Setting_Operation;
        public Sprite Bar_Show, Bar_Hidden;
        public GameObject System, Audio, Display, Operation;

        // Method to call when a tab is clicked
        public void OnTabClick(string tabName)
        {
            AudioManager_2.SoundPlay(5);//手动SE音频替换

            Title_Setting_System.sprite = Bar_Hidden;
            Title_Setting_Audio.sprite = Bar_Hidden;
            Title_Setting_Display.sprite = Bar_Hidden;
            Title_Setting_Operation.sprite = Bar_Hidden;

            System.SetActive(false);
            Audio.SetActive(false);
            Display.SetActive(false);
            Operation.SetActive(false);

            switch (tabName)
            {
                case "System":
                    Title_Setting_System.sprite = Bar_Show;
                    System.SetActive(true);
                    break;
                case "Audio":
                    Title_Setting_Audio.sprite = Bar_Show;
                    Audio.SetActive(true);
                    break;
                case "Display":
                    Title_Setting_Display.sprite = Bar_Show;
                    Display.SetActive(true);
                    break;
                case "Operation":
                    Title_Setting_Operation.sprite = Bar_Show;
                    Operation.SetActive(true);
                    break;
                default:
                    break;
            }

        }

        //衣服显示变化
        public Image Menu_Anto, Menu_Hetty, Menu_Alice;
        public Sprite[] AntoSprites;
        public Sprite[] HettySprites;
        public Sprite[] AliceSprites;

        #endregion



        /// <summary>
        /// 存档统合
        /// </summary>
        #region

        [Header("存档界面UI")]
        public InputField nameInputField; // 绑定在 Inspector 里

        public SaveSlotUI saveSlotUI_1, saveSlotUI_2, saveSlotUI_3;
        SaveSlotUI CurrentSaveSlotUI;
        public void CurrentSaveSlotUI_Is(int Number)
        {
            switch (Number)
            {

                case 1:
                    CurrentSaveSlotUI = saveSlotUI_1;
                    break;
                case 2:
                    CurrentSaveSlotUI = saveSlotUI_2;
                    break;
                case 3:
                    CurrentSaveSlotUI = saveSlotUI_3;
                    break;
            }
        }//确定当前选中脚本
        public void MakeSureDeleteDateMenu_Delete()
        {
            if (CurrentSaveSlotUI != null)
            {
                CurrentSaveSlotUI.OnDeleteClicked();
            }
        }//删除当前选中存档


        public GameObject SaveNameMenu;//输入酒保名称菜单

        public void OnConfirmNameInput()
        {
            if (CurrentSaveSlotUI != null)
            {
                CurrentSaveSlotUI.saveName = nameInputField.text.Trim();

                // 新建存档
                SaveData newData = new SaveData(CurrentSaveSlotUI.slotName);

                newData.slotName = CurrentSaveSlotUI.slotName;//记住档的名字
                newData.saveName = CurrentSaveSlotUI.saveName;//记主人公的名字
                newData.balance = 1000;//初始给与1000


                SaveManager.SaveGame(newData);

                CurrentSaveSlotUI.Refresh();//更新当前存档内容
            }

        }//玩家确定这个存档名称








        #endregion



        /// <summary>
        /// CG解锁进度
        /// </summary>
        #region

        [Header("当前已解锁CG数量")]
        public int unlockedCount;
        public int pageCount;

        public GameObject Thumbnail_Anto_01;
        public GameObject Thumbnail_Anto_02;
        public GameObject Thumbnail_Anto_03;
        public GameObject Thumbnail_Anto_04;
        public GameObject Thumbnail_Anto_05;
        public GameObject Thumbnail_Anto_06;
        public GameObject Thumbnail_Anto_07;
        public GameObject Thumbnail_Anto_08;
        public GameObject Thumbnail_Anto_09;
        public GameObject Thumbnail_Anto_10;

        public GameObject Thumbnail_Hetty_01;
        public GameObject Thumbnail_Hetty_02;
        public GameObject Thumbnail_Hetty_03;
        public GameObject Thumbnail_Hetty_04;
        public GameObject Thumbnail_Hetty_05;
        public GameObject Thumbnail_Hetty_06;
        public GameObject Thumbnail_Hetty_07;
        public GameObject Thumbnail_Hetty_08;
        public GameObject Thumbnail_Hetty_09;
        public GameObject Thumbnail_Hetty_10;

        public GameObject Thumbnail_Alice_01;
        public GameObject Thumbnail_Alice_02;
        public GameObject Thumbnail_Alice_03;
        public GameObject Thumbnail_Alice_04;
        public GameObject Thumbnail_Alice_05;
        public GameObject Thumbnail_Alice_06;
        public GameObject Thumbnail_Alice_07;
        public GameObject Thumbnail_Alice_08;
        public GameObject Thumbnail_Alice_09;
        public GameObject Thumbnail_Alice_10;

        public void CheckAndShowAntoCG()
        {
            int maxAntoProgress = 0;
            int maxHettyProgress = 0;
            int maxAliceProgress = 0;

            // 检查三个存档中antoProgress最大值
            for (int i = 1; i <= 3; i++)
            {
                string slotName = "CurrentPlayer" + i;
                if (!string.IsNullOrEmpty(slotName) && SaveManager.Exists(slotName))
                {
                    SaveData data = SaveManager.LoadGame(slotName);
                    if (data.antoProgress > maxAntoProgress)
                        maxAntoProgress = data.antoProgress;
                    if (data.hettyProgress > maxHettyProgress)
                        maxHettyProgress = data.hettyProgress;
                    if (data.aliceProgress > maxAliceProgress)
                        maxAliceProgress = data.aliceProgress;
                }
            }

            // 控制显示
            Thumbnail_Anto_01.SetActive(maxAntoProgress >= 2);
            Thumbnail_Anto_02.SetActive(maxAntoProgress >= 3);
            Thumbnail_Anto_03.SetActive(maxAntoProgress >= 4);
            Thumbnail_Anto_04.SetActive(maxAntoProgress >= 5);
            Thumbnail_Anto_05.SetActive(maxAntoProgress >= 6);
            Thumbnail_Anto_06.SetActive(maxAntoProgress >= 7);
            Thumbnail_Anto_07.SetActive(maxAntoProgress >= 8);
            Thumbnail_Anto_08.SetActive(maxAntoProgress >= 9);
            Thumbnail_Anto_09.SetActive(maxAntoProgress >= 10);
            Thumbnail_Anto_10.SetActive(maxAntoProgress >= 11);//确实需要11这个状态来代表最后一个CG已近解锁


            Thumbnail_Hetty_01.SetActive(maxHettyProgress >= 2);
            Thumbnail_Hetty_02.SetActive(maxHettyProgress >= 3);
            Thumbnail_Hetty_03.SetActive(maxHettyProgress >= 4);
            Thumbnail_Hetty_04.SetActive(maxHettyProgress >= 5);
            Thumbnail_Hetty_05.SetActive(maxHettyProgress >= 6);
            Thumbnail_Hetty_06.SetActive(maxHettyProgress >= 7);
            Thumbnail_Hetty_07.SetActive(maxHettyProgress >= 8);
            Thumbnail_Hetty_08.SetActive(maxHettyProgress >= 9);
            Thumbnail_Hetty_09.SetActive(maxHettyProgress >= 10);
            Thumbnail_Hetty_10.SetActive(maxHettyProgress >= 11);//确实需要11这个状态来代表最后一个CG已近解锁


            Thumbnail_Alice_01.SetActive(maxAliceProgress >= 2);
            Thumbnail_Alice_02.SetActive(maxAliceProgress >= 3);
            Thumbnail_Alice_03.SetActive(maxAliceProgress >= 4);
            Thumbnail_Alice_04.SetActive(maxAliceProgress >= 5);
            Thumbnail_Alice_05.SetActive(maxAliceProgress >= 6);
            Thumbnail_Alice_06.SetActive(maxAliceProgress >= 7);
            Thumbnail_Alice_07.SetActive(maxAliceProgress >= 8);
            Thumbnail_Alice_08.SetActive(maxAliceProgress >= 9);
            Thumbnail_Alice_09.SetActive(maxAliceProgress >= 10);
            Thumbnail_Alice_10.SetActive(maxAliceProgress >= 11);//确实需要11这个状态来代表最后一个CG已近解锁



            //计算已经解锁CG数量,判断出现多少页
            #region
            unlockedCount =
    CountUnlockedThumbnails(
        Thumbnail_Anto_01, Thumbnail_Anto_02, Thumbnail_Anto_03, Thumbnail_Anto_04, Thumbnail_Anto_05,
        Thumbnail_Anto_06, Thumbnail_Anto_07, Thumbnail_Anto_08, Thumbnail_Anto_09, Thumbnail_Anto_10,

        Thumbnail_Hetty_01, Thumbnail_Hetty_02, Thumbnail_Hetty_03, Thumbnail_Hetty_04, Thumbnail_Hetty_05,
        Thumbnail_Hetty_06, Thumbnail_Hetty_07, Thumbnail_Hetty_08, Thumbnail_Hetty_09, Thumbnail_Hetty_10,

        Thumbnail_Alice_01, Thumbnail_Alice_02, Thumbnail_Alice_03, Thumbnail_Alice_04, Thumbnail_Alice_05,
        Thumbnail_Alice_06, Thumbnail_Alice_07, Thumbnail_Alice_08, Thumbnail_Alice_09, Thumbnail_Alice_10
    );

           
            #endregion



            //设置主菜单衣服显示
            if (maxAntoProgress < 3) { Menu_Anto.sprite = AntoSprites[0]; }
            else if (maxAntoProgress >= 3 && maxAntoProgress < 10) { Menu_Anto.sprite = AntoSprites[1]; }
            else { Menu_Anto.sprite = AntoSprites[2]; }

            if (maxHettyProgress < 3) { Menu_Hetty.sprite = HettySprites[0]; }
            else if (maxHettyProgress >= 3 && maxHettyProgress < 10) { Menu_Hetty.sprite = HettySprites[1]; }
            else { Menu_Hetty.sprite = HettySprites[2]; }

            if (maxAliceProgress < 3) { Menu_Alice.sprite = AliceSprites[0]; }
            else if (maxAliceProgress >= 3 && maxAliceProgress < 10) { Menu_Alice.sprite = AliceSprites[1]; }
            else { Menu_Alice.sprite = AliceSprites[2]; }

        }//读取当前最大CG解锁进度





        int CountUnlockedThumbnails(params GameObject[] thumbnails)
        {
            int count = 0;
            foreach (var go in thumbnails)
            {
                if (go != null && go.activeSelf)
                    count++;
            }
            return count;
        }//计算解锁数量

        #endregion


        /// <summary>
        /// CG鉴赏上下翻页
        /// </summary>
        #region

        [Header("要移动的UI物体")]
        public RectTransform target;

        [Header("上下按钮（要隐藏/显示）")]
        public GameObject upButton;
        public GameObject downButton;

        [Header("每次移动步长")]
        public float step = 239f;

        [Header("Y范围（到顶/到底就隐藏按钮）")]
        public float topLimit = -23f;     // 最上不能超过这个值
        public float bottomLimit = 1889f; // 最下不能超过这个值





        public void MoveUp()
        {
            SetY(target.anchoredPosition.y - step);
        }

        public void MoveDown()
        {

            SetY(target.anchoredPosition.y + step);

        }

        private void SetY(float y)
        {
            y = Mathf.Clamp(y, topLimit, bottomLimit);
            var p = target.anchoredPosition;
            p.y = y;
            target.anchoredPosition = p;

            RefreshButtons();
        }

        private void RefreshButtons()
        {
            float y = target.anchoredPosition.y;
            
            // 到顶：隐藏上键；否则显示
            if (downButton) downButton.SetActive(y < bottomLimit);
            
            // 到底：隐藏下键；否则显示
            if (upButton) upButton.SetActive(y > topLimit);

            // if (unlockedCount <= 6)
            // {
            //     // 只有一页，两个按钮都不显示
            //     if (upButton) upButton.SetActive(false);
            //     if (downButton) downButton.SetActive(false);
            //     return;
            // }
            //
            // pageCount = Mathf.CeilToInt(unlockedCount / 6f);
            // pageCount+=2;
            //
            // int currentPage = Mathf.RoundToInt(
            //     (target.anchoredPosition.y - topLimit) / step
            // );
            //
            // // 上一页是否存在
            // if (upButton)
            //     upButton.SetActive(currentPage > 0);
            //
            // // 下一页是否存在
            // if (downButton)
            //     downButton.SetActive(currentPage < pageCount - 1);


        }




        #endregion

        /// <summary>
        /// 语言设置/文字加载速度
        /// </summary>
        #region
        public void Setlanguage(int number)
        {
            AudioManager_2.SoundPlay(4);//手动SE音频替换

            PlayerPrefs.SetInt("language", number);//0日语 1简体中文 2繁体中文 3英语 4韩语
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }



        public Slider textSpeedSlider; // 引用UI中的Slider组件

        void CheckTextSpeed()
        {
            // 初始化Slider的值
            float textSpeed = PlayerPrefs.GetFloat("TextSpeed", 0.05f); // 如果没有找到，使用默认值0.05f
            textSpeedSlider.value = textSpeed; // 设置Slider的值
            textSpeedSlider.onValueChanged.AddListener(SetTextSpeed); // 为Slider添加值改变监听事件
        }

        public void SetTextSpeed(float value)
        {
            PlayerPrefs.SetFloat("TextSpeed", value);

            //Debug.Log("目前储存的AVG对话框文字速度" + PlayerPrefs.GetFloat("TextSpeed"));
        }
        #endregion




        /// <summary>
        /// 恢复默认设置
        /// </summary>
        #region

        public void ReStart_DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("恢复默认设置");

            LoadingImage.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }//删除存档

        #endregion




        /// <summary>
        /// 跳转场景
        /// </summary>
        #region
        [Header("加载场景")]
        public GameObject LoadingImage;
        public void LoadingScene_BJ_Mobile()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);

            Invoke("LoadBJ_Mobile", 1f);
            SetWait();
        }
        void LoadBJ_Mobile()
        {
            SceneManager.LoadScene("BJ_Mobile");
        }


        public void LoadingScene_Lobby()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);

            Invoke("LoadLobby", 1f);
            SetWait();
        }
        void LoadLobby()
        {
            SceneManager.LoadScene("Lobby");
        }


        public void LoadingScene_BarCounter()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);

            Invoke("LoadBarCounter", 1f);
            SetWait();
        }
        void LoadBarCounter()
        {
            SceneManager.LoadScene("BarCounter");
        }


        public void LoadingScene_Spine()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);

            Invoke("LoadSpine", 1f);
            SetWait();
        }
        void LoadSpine()
        {
            SceneManager.LoadScene("Spine");
        }


        public void LoadingScene_Shop()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);

            Invoke("LoadShop", 1f);
            SetWait();
        }
        void LoadShop()
        {
            SceneManager.LoadScene("Shop");
        }



        public void OpenSaveURL()
        {
            Application.OpenURL(Application.persistentDataPath);
        }//打开存档位置文件夹

        #endregion


        /// <summary>
        /// 暂停菜单
        /// </summary>
        #region
        [Header("暂停菜单")]
        public GameObject PauseMenu;
        public void OpenPauseMenu()
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
        }
        public void ClosePauseMenu()
        {
            Time.timeScale = 1f;
            PauseMenu.SetActive(false);
        }
        #endregion

        /// <summary>
        /// 声音控制
        /// </summary>
        #region
        [Header("声音利用")]
        public AudioMixer audioMixer;//SE 效果音
        public AudioMixer audioMixer_2;//BGM
        public AudioMixer audioMixer_3;//Voice

        //public Slider slider_Main;
        public Slider slider_SE;
        public Slider slider_BGM;
        public Slider slider_Voice;

        public void SetVolume(float value)
        {
            audioMixer.SetFloat("MainVolume", value);
            PlayerPrefs.SetFloat("SE_Volume", value);
        }

        public void SetVolume_2(float value)
        {
            audioMixer_2.SetFloat("MainVolume_2", value);
            PlayerPrefs.SetFloat("BGM_Volume", value);
        }

        public void SetVolume_3(float value)
        {
            audioMixer_3.SetFloat("MainVolume_3", value);
            PlayerPrefs.SetFloat("Voice_Volume", value);
        }

        public void SetMasterVolume(float value)
        {
            audioMixer.SetFloat("MainVolume", value); // 确保在效果音混音器中存在名为MasterVolume的参数
            audioMixer_2.SetFloat("MainVolume_2", value); // 确保在BGM混音器中存在名为MasterVolume的参数
            audioMixer_3.SetFloat("MainVolume_3", value); // 确保在Voice混音器中存在名为MasterVolume的参数

            PlayerPrefs.SetFloat("SE_Volume", value);
            PlayerPrefs.SetFloat("BGM_Volume", value);
            PlayerPrefs.SetFloat("Voice_Volume", value);

        }//主音频

        #endregion



        /// <summary>
        /// 按键设置
        /// </summary>
        #region
        [Header("按键设置")]
        private bool isWaitingForKey = false; // 是否正在等待按键输入
        private string ButtonName;
        public Text
            keybindText_Hit,
            keybindText_Stand,
            keybindText_DoubleDown,
            keybindText_Skip,
            keybindText_Confirm,
            keybindText_Back; // UI中显示按键的文本组件

        private KeyCode[] allKeyCodes;

        void Update()
        {
            if (isWaitingForKey)
            {
                foreach (KeyCode kcode in allKeyCodes)
                {
                    if (Input.GetKeyDown(kcode) && kcode != KeyCode.Mouse0 && kcode != KeyCode.Mouse1 && kcode != KeyCode.Mouse2)
                    {


                        SetKeybind(kcode);
                        isWaitingForKey = false;
                        break;
                    }
                }
            }

        }//检测是否输入

        void LoadKeyBindings()
        {
            // Hit
            if (PlayerPrefs.HasKey("KeyBindings_Hit"))
            {
                keybindText_Hit.text = PlayerPrefs.GetString("KeyBindings_Hit");
            }
            else
            {
                keybindText_Hit.text = "A";
                PlayerPrefs.SetString("KeyBindings_Hit", "A");
            }

            // Stand
            if (PlayerPrefs.HasKey("KeyBindings_Stand"))
            {
                keybindText_Stand.text = PlayerPrefs.GetString("KeyBindings_Stand");
            }
            else
            {
                keybindText_Stand.text = "S";
                PlayerPrefs.SetString("KeyBindings_Stand", "S");
            }

            // DoubleDown
            if (PlayerPrefs.HasKey("KeyBindings_DoubleDown"))
            {
                keybindText_DoubleDown.text = PlayerPrefs.GetString("KeyBindings_DoubleDown");
            }
            else
            {
                keybindText_DoubleDown.text = "D";
                PlayerPrefs.SetString("KeyBindings_DoubleDown", "D");
            }

            // Skip
            if (PlayerPrefs.HasKey("KeyBindings_Skip"))
            {
                keybindText_Skip.text = PlayerPrefs.GetString("KeyBindings_Skip");
            }
            else
            {
                keybindText_Skip.text = "LeftShift";
                PlayerPrefs.SetString("KeyBindings_Skip", "LeftShift");
            }

            // Confirm
            if (PlayerPrefs.HasKey("KeyBindings_Confirm"))
            {
                keybindText_Confirm.text = PlayerPrefs.GetString("KeyBindings_Confirm");
            }
            else
            {
                keybindText_Confirm.text = "Return";
                PlayerPrefs.SetString("KeyBindings_Confirm", "Return");
            }

            // Back
            if (PlayerPrefs.HasKey("KeyBindings_Back"))
            {
                keybindText_Back.text = PlayerPrefs.GetString("KeyBindings_Back");
            }
            else
            {
                keybindText_Back.text = "Backspace";
                PlayerPrefs.SetString("KeyBindings_Back", "Backspace");
            }

        }//如果没有设置过，那么读取默认

        public void StartKeybindChange(string buttonName)
        {
            if (!isWaitingForKey)
            {


                isWaitingForKey = true; // 开始等待按键输入
                ButtonName = buttonName;//记住目前选中的按钮

                //本地化按下显示
                int languageIndex = PlayerPrefs.GetInt("language");
                string pressKeyText = pressKeyTranslations[languageIndex];

                switch (ButtonName)
                {
                    case "Hit":
                        keybindText_Hit.text = pressKeyText; // 提示用户按下一个键             
                        break;
                    case "Stand":
                        keybindText_Stand.text = pressKeyText; // 提示用户按下一个键               
                        break;
                    case "DoubleDown":
                        keybindText_DoubleDown.text = pressKeyText; // 提示用户按下一个键
                        break;
                    case "Skip":
                        keybindText_Skip.text = pressKeyText; // 提示用户按下一个键
                        break;
                    case "Confirm":
                        keybindText_Confirm.text = pressKeyText; // 提示用户按下一个键
                        break;
                    case "Back":
                        keybindText_Back.text = pressKeyText; // 提示用户按下一个键
                        break;
                }

            }
            else
            {
                AudioManager_2.SoundPlay(5);//手动SE音频替换
            }


        }//哪个键位需要输入

        void SetKeybind(KeyCode newKey)
        {


            switch (ButtonName)
            {
                case "Hit":
                    keybindText_Hit.text = "" + newKey;
                    PlayerPrefs.SetString("KeyBindings_Hit", keybindText_Hit.text);
                    break;
                case "Stand":
                    keybindText_Stand.text = "" + newKey;
                    PlayerPrefs.SetString("KeyBindings_Stand", keybindText_Stand.text);
                    break;
                case "DoubleDown":
                    keybindText_DoubleDown.text = "" + newKey;
                    PlayerPrefs.SetString("KeyBindings_DoubleDown", keybindText_DoubleDown.text);
                    break;
                case "Skip":
                    keybindText_Skip.text = "" + newKey;
                    PlayerPrefs.SetString("KeyBindings_Skip", keybindText_Skip.text);
                    break;
                case "Confirm":
                    keybindText_Confirm.text = "" + newKey;
                    PlayerPrefs.SetString("KeyBindings_Confirm", keybindText_Confirm.text);
                    break;
                case "Back":
                    keybindText_Back.text = "" + newKey;
                    PlayerPrefs.SetString("KeyBindings_Back", keybindText_Back.text);
                    break;
            }

        }//输入并储存

        public void ResetButton()
        {
            keybindText_Hit.text = "A";
            PlayerPrefs.SetString("KeyBindings_Hit", "A");

            keybindText_Stand.text = "S";
            PlayerPrefs.SetString("KeyBindings_Stand", "S");

            keybindText_DoubleDown.text = "D";
            PlayerPrefs.SetString("KeyBindings_DoubleDown", "D");

            keybindText_Skip.text = "LeftShift";
            PlayerPrefs.SetString("KeyBindings_Skip", "LeftShift");

            keybindText_Confirm.text = "Return";
            PlayerPrefs.SetString("KeyBindings_Confirm", "Return");

            keybindText_Back.text = "Backspace";
            PlayerPrefs.SetString("KeyBindings_Back", "Backspace");
        }

        private Dictionary<int, string> pressKeyTranslations = new Dictionary<int, string>()
{
    {0, "キー押下"},   // 日语
    {1, "按下按钮"},                 // 简体中文
    {2, "按下按鈕"},                 // 繁体中文
    {3, "Press Key"},               // 英语
    {4, "키를 누르세요"}              // 韩语
};//本地化字典

        #endregion

        /// <summary>
        /// 鼠标嵌套设置
        /// </summary>
        #region
        [Header("Cursor Textures")]
        public Texture2D normalCursor;
        public Texture2D waitCursor;

        //开场的加载鼠标变化
        void WaitToNormal()
        {
            SetWait();
            Invoke(nameof(SetNormal), 1f);
        }


        public void SetNormal()
        {
            Cursor.SetCursor(
                normalCursor,
                new Vector2(0, 0),   // 左上角点击
                CursorMode.Auto
            );
        }

        public void SetWait()
        {
            Cursor.SetCursor(
                waitCursor,
                new Vector2(waitCursor.width / 2, waitCursor.height / 2),
                CursorMode.Auto
            );
        }

        #endregion


        /// <summary>
        /// 跳转网页/退出游戏
        /// </summary>
        #region
        public void OpenURL_Patreon()
        {
            Application.OpenURL("https://www.patreon.com/c/NEKOUJI/posts");
        }

        public void OpenURL_Discord()
        {
            Application.OpenURL("https://discord.gg/uCsSTPmMjV");
            Application.OpenURL("https://discord.gg/bc49G5Xcq9");
        }

        public void OpenURL_Steam()
        {
            Application.OpenURL("https://store.steampowered.com/");
        }

        public void OpenURL_Ci_en()
        {
            Application.OpenURL("https://ci-en.dlsite.com/creator/23364");
        }

        public void OpenURL_YYY()
        {
            Application.OpenURL("https://x.com/Detective_ye");
        }

        public void OpenURL_NEKOUJI()
        {
            Application.OpenURL("https://x.com/nekoujistudio");
        }

        public void ExitGame()
        {
            Debug.Log("Exiting game...");

            Application.Quit();
        }

        #endregion



        /// <summary>
        /// 頁面設置UI显示
        /// </summary>
        #region
        [Header("画面显示方法")]
        public GameObject DisplayMode_1;//全屏
        public GameObject DisplayMode_2;//窗口



        void StartSetDisplayMode()
        {
            bool fullscreen = PlayerPrefs.GetInt("DisplayMode", 1) == 1;
            int resIndex = PlayerPrefs.GetInt("ResolutionIndex", 2); // 默认1080p

            currentMode = fullscreen ? DisplayMode.Fullscreen : DisplayMode.Windowed;

            var res = supportedResolutions[resIndex];
            Screen.SetResolution(res.x, res.y, fullscreen);

        }//开始设置屏幕分辨率




        enum DisplayMode
        {
            Fullscreen,
            Windowed
        }

        DisplayMode currentMode;
        Resolution currentResolution;



        public void SetFullScreenOrWindowed()
        {
            if (currentMode == DisplayMode.Fullscreen)
            {
                SetDisplayMode(false);
            }
            else
            {
                SetDisplayMode(true);
            }
        }//设置屏幕模式活扣

        public void SetDisplayMode(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
            currentMode = fullscreen ? DisplayMode.Fullscreen : DisplayMode.Windowed;

            PlayerPrefs.SetInt("DisplayMode", fullscreen ? 1 : 0);



            //修改显示
            if (currentMode == DisplayMode.Fullscreen)
            {
                DisplayMode_1.SetActive(true);
                DisplayMode_2.SetActive(false);
            }
            else
            {
                DisplayMode_1.SetActive(false);
                DisplayMode_2.SetActive(true);
            }


        }//设置全屏或者窗口化

        Vector2Int[] supportedResolutions =
{
    new Vector2Int(3840, 2160),
    new Vector2Int(2560, 1440),
    new Vector2Int(1920, 1080),
    new Vector2Int(1600, 900),
    new Vector2Int(1280, 720),
};

        public void SetResolutionByIndex(int index)
        {
            var res = supportedResolutions[index];

            Screen.SetResolution(
                res.x,
                res.y,
                currentMode == DisplayMode.Fullscreen
            );

            // if (index == 0)
            // {
            //     //默认的就是基于当前屏幕分辨率
            //     InitResolutions();
            // }
            // else
            // {
            //     var res = supportedResolutions[index];
            //
            //     Screen.SetResolution(
            //         res.x,
            //         res.y,
            //         currentMode == DisplayMode.Fullscreen
            //     );
            // }

            PlayerPrefs.SetInt("ResolutionIndex", index);

            //设置屏幕分辨率文字
            GetResolutionIndex_Text();

        }//设置当前屏幕模式的分辨率



       // public void InitResolutions()
       // {
       //
       //     Resolution[] resolutions = Screen.resolutions;//获取设置当前屏幕分辩率
       //     Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);//设置当前分辨率
       //
       //     //设置屏幕分辨率文字
       //     GetResolutionIndex_Text();
       //
       // }//设置当前屏幕分辩率


        public Text ResolutionsText;

        public void GetResolutionIndex_Text()
        {
            //设置屏幕分辨率文字
            int index = PlayerPrefs.GetInt("ResolutionIndex");
            ResolutionsText.text = GetResolutionLabel(index).ToString();

        }//读取分辨率数字



        string GetResolutionLabel(int index)
        {
            var r = supportedResolutions[index];

            return $"{r.x}×{r.y}";
        }



        [Header("允许后台运行")]
        public GameObject AllowedBackgroundRunning_1;
        public GameObject AllowedBackgroundRunning_2;

        public bool isAllowedBackgroundRunning = true;//默认允许

        public void _AllowBackgroundRunning()
        {
            isAllowedBackgroundRunning = !isAllowedBackgroundRunning;

            AllowBackgroundRunning();

        }

        void AllowBackgroundRunning()
        {

            if (isAllowedBackgroundRunning)
            {

                AllowedBackgroundRunning_1.SetActive(true);
                AllowedBackgroundRunning_2.SetActive(false);

                Application.runInBackground = true; // 允许游戏在后台运行
                PlayerPrefs.SetInt("Setting_AllowBackgroundRunning", 0);

            }
            else
            {
                AllowedBackgroundRunning_1.SetActive(false);
                AllowedBackgroundRunning_2.SetActive(true);

                Application.runInBackground = false; // 不允许游戏在后台运行
                PlayerPrefs.SetInt("Setting_AllowBackgroundRunning", 1);

            }

        }




        #endregion



        /// <summary>
        /// 通关前记录天数
        /// </summary>
        #region
        [Header("显示是第几日")]
        public Text Day;
        void OnEnterTavern()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            if (!data.HasCleared)
            {
                data.DayCount++;
                SaveManager.SaveGame(data);


                Debug.Log("当前天数：" + data.DayCount);

                //显示现在是第X天
                switch (PlayerPrefs.GetInt("language"))
                {
                    case 0:
                        // 日语
                        Day.text = data.DayCount + "日目";
                        break;

                    case 1:
                        // 简体中文
                        Day.text = "第" + data.DayCount + "天";
                        break;

                    case 2:
                        // 繁体中文
                        Day.text = "第" + data.DayCount + "天";
                        break;

                    case 3:
                        // 英语
                        Day.text = "Day " + data.DayCount;
                        break;

                    case 4:
                        // 韩语
                        Day.text = data.DayCount + "일째";
                        break;
                }

                Day.color = new Color(1f, 0f, 0.831f, 1f); //粉色


                Invoke(nameof(ShowDay), 0.5f);
            }

         
        }

        void ShowDay()
        {
            Day.gameObject.SetActive(true);
        }
        #endregion


        /// <summary>
        /// AVG画面
        /// </summary>
        #region
        [Header("AVG画面")]
        public DialogSystem dialog;
        public GameObject AVG;





        public void Load_AVG(int Number)
        {

            dialog.animation_number = Number;
            dialog.gameObject.SetActive(true);
            AVG.SetActive(true);
        }



        public bool GameOver = false;//赌局没有结束
        //退出赌局
        public void Leave()
        {
            Debug.Log("点击离开");

            switch (GameFlowData.nextAVGId)
            {
                case "VSAnto":
                    GameFlowData.nextAVGId = "Anto_Failure";//输给安托，离开赌局
                    break;
                case "VSHetty":
                    GameFlowData.nextAVGId = "Hetty_Failure";//输给赫蒂，离开赌局
                    break;
                case "VSAlice":
                    GameFlowData.nextAVGId = "Alice_Failure";//输给爱丽丝，离开赌局
                    break;
            }



            LoadingScene_Spine();
        }

        //获得胜利开启CG
        public void Load_Win_AVG()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            switch (GameFlowData.nextAVGId)
            {
                case "VSAnto":
                    switch (data.antoProgress)
                    {
                        case 1:
                            GameFlowData.nextAVGId = "Anto_CG_01_2";//开启安托第一个CG前端AVG
                            break;
                        case 2:
                            GameFlowData.nextAVGId = "Anto_CG_02_2";//开启安托第二个CG前端AVG
                            break;
                        case 3:
                            GameFlowData.nextAVGId = "Anto_CG_03_2";//开启安托第三个CG前端AVG
                            break;
                        case 4:
                            GameFlowData.nextAVGId = "Anto_CG_04_2";//开启安托第四个CG前端AVG
                            break;
                        case 5:
                            GameFlowData.nextAVGId = "Anto_CG_05_2";//开启安托第五个CG前端AVG
                            break;
                        case 6:
                            GameFlowData.nextAVGId = "Anto_CG_06_2";//开启安托第六个CG前端AVG
                            break;
                        case 7:
                            GameFlowData.nextAVGId = "Anto_CG_07_2";//开启安托第七个CG前端AVG
                            break;
                        case 8:
                            GameFlowData.nextAVGId = "Anto_CG_08_2";//开启安托第八个CG前端AVG
                            break;
                        case 9:
                            GameFlowData.nextAVGId = "Anto_CG_09_2";//开启安托第九个CG前端AVG
                            break;
                        case 10:
                            GameFlowData.nextAVGId = "Anto_CG_10_2";//开启安托第十个CG前端AVG
                            break;
                    }
                    break;

                case "VSHetty":
                    switch (data.hettyProgress)
                    {
                        case 1:
                            GameFlowData.nextAVGId = "Hetty_CG_01_2";//开启赫蒂第一个CG前端AVG
                            break;
                        case 2:
                            GameFlowData.nextAVGId = "Hetty_CG_02_2";//开启赫蒂第二个CG前端AVG
                            break;
                        case 3:
                            GameFlowData.nextAVGId = "Hetty_CG_03_2";//开启赫蒂第三个CG前端AVG
                            break;
                        case 4:
                            GameFlowData.nextAVGId = "Hetty_CG_04_2";//开启赫蒂第四个CG前端AVG
                            break;
                        case 5:
                            GameFlowData.nextAVGId = "Hetty_CG_05_2";//开启赫蒂第五个CG前端AVG
                            break;
                        case 6:
                            GameFlowData.nextAVGId = "Hetty_CG_06_2";//开启赫蒂第六个CG前端AVG
                            break;
                        case 7:
                            GameFlowData.nextAVGId = "Hetty_CG_07_2";//开启赫蒂第七个CG前端AVG
                            break;
                        case 8:
                            GameFlowData.nextAVGId = "Hetty_CG_08_2";//开启赫蒂第八个CG前端AVG
                            break;
                        case 9:
                            GameFlowData.nextAVGId = "Hetty_CG_09_2";//开启赫蒂第九个CG前端AVG
                            break;
                        case 10:
                            GameFlowData.nextAVGId = "Hetty_CG_10_2";//开启赫蒂第十个CG前端AVG
                            break;
                    }
                    break;

                case "VSAlice":
                    switch (data.aliceProgress)
                    {
                        case 1:
                            GameFlowData.nextAVGId = "Alice_CG_01_2";//开启爱丽丝第一个CG前端AVG
                            break;
                        case 2:
                            GameFlowData.nextAVGId = "Alice_CG_02_2";//开启爱丽丝第二个CG前端AVG
                            break;
                        case 3:
                            GameFlowData.nextAVGId = "Alice_CG_03_2";//开启爱丽丝第三个CG前端AVG
                            break;
                        case 4:
                            GameFlowData.nextAVGId = "Alice_CG_04_2";//开启爱丽丝第四个CG前端AVG
                            break;
                        case 5:
                            GameFlowData.nextAVGId = "Alice_CG_05_2";//开启爱丽丝第五个CG前端AVG
                            break;
                        case 6:
                            GameFlowData.nextAVGId = "Alice_CG_06_2";//开启爱丽丝第六个CG前端AVG
                            break;
                        case 7:
                            GameFlowData.nextAVGId = "Alice_CG_07_2";//开启爱丽丝第七个CG前端AVG
                            break;
                        case 8:
                            GameFlowData.nextAVGId = "Alice_CG_08_2";//开启爱丽丝第八个CG前端AVG
                            break;
                        case 9:
                            GameFlowData.nextAVGId = "Alice_CG_09_2";//开启爱丽丝第九个CG前端AVG
                            break;
                        case 10:
                            GameFlowData.nextAVGId = "Alice_CG_10_2";//开启爱丽丝第十个CG前端AVG
                            break;
                    }
                    break;


            }





            LoadingScene_Spine();
        }//这是剧情内部打开CG路径



        public void Load_Vs_Anto_AVG()
        {

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            int Story_Anto = data.antoProgress;
            Debug.Log("目前储存的关卡进度_安托" + Story_Anto);
            if (Story_Anto <= 0)
            {
                data.antoProgress = 1;
                SaveManager.SaveGame(data);
            }


            switch (data.antoProgress)
            {
                case 1:
                    Load_AVG(1011);//安托一
                    break;
                case 2:
                    Load_AVG(1021);//安托二
                    break;
                case 3:
                    Load_AVG(1031);//安托三
                    break;
                case 4:
                    Load_AVG(1041);//安托四
                    break;
                case 5:
                    Load_AVG(1051);//安托五
                    break;
                case 6:
                    Load_AVG(1061);//安托六
                    break;
                case 7:
                    Load_AVG(1071);//安托七
                    break;
                case 8:
                    Load_AVG(1081);//安托八
                    break;
                case 9:
                    Load_AVG(1091);//安托九
                    break;
                case 10:
                case 11://确实需要11这个状态来代表最后一个CG已近解锁
                    Load_AVG(1101);//安托十
                    break;
            }

        }//女荷官指名界面选择（只留下VS_XXX，在下一个场景根据AVG解锁）
        public void Load_Anto_Lose_AVG()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            switch (data.antoProgress)
            {
                case 1:
                    Load_AVG(1014);//安托一
                    break;
                case 2:
                    Load_AVG(1024);//安托二
                    break;
                case 3:
                    Load_AVG(1034);//安托三
                    break;
                case 4:
                    Load_AVG(1044);//安托四
                    break;
                case 5:
                    Load_AVG(1054);//安托五
                    break;
                case 6:
                    Load_AVG(1064);//安托六
                    break;
                case 7:
                    Load_AVG(1074);//安托七
                    break;
                case 8:
                    Load_AVG(1084);//安托八
                    break;
                case 9:
                    Load_AVG(1094);//安托九
                    break;
                case 10:
                case 11://确实需要11这个状态来代表最后一个CG已近解锁
                    Load_AVG(1104);//安托十
                    break;
            }
        }


        public void Load_Vs_Alice_AVG()
        {

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            int Story_Alice = data.aliceProgress;
            Debug.Log("目前储存的关卡进度_爱丽丝" + Story_Alice);
            if (Story_Alice <= 0)
            {
                data.aliceProgress = 1;
                SaveManager.SaveGame(data);
            }


            switch (data.aliceProgress)
            {
                case 1:
                    Load_AVG(3011);//爱丽丝一
                    break;
                case 2:
                    Load_AVG(3021);//爱丽丝二
                    break;
                case 3:
                    Load_AVG(3031);//爱丽丝三
                    break;
                case 4:
                    Load_AVG(3041);//爱丽丝四
                    break;
                case 5:
                    Load_AVG(3051);//爱丽丝五
                    break;
                case 6:
                    Load_AVG(3061);//爱丽丝六
                    break;
                case 7:
                    Load_AVG(3071);//爱丽丝七
                    break;
                case 8:
                    Load_AVG(3081);//爱丽丝八
                    break;
                case 9:
                    Load_AVG(3091);//爱丽丝九
                    break;
                case 10:
                case 11://确实需要11这个状态来代表最后一个CG已近解锁
                    Load_AVG(3101);//爱丽丝十
                    break;
            }

        }//女荷官指名界面选择（只留下VS_XXX，在下一个场景根据AVG解锁）
        public void Load_Alice_Lose_AVG()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            switch (data.aliceProgress)
            {
                case 1:
                    Load_AVG(3014);//爱丽丝一
                    break;
                case 2:
                    Load_AVG(3024);//爱丽丝二
                    break;
                case 3:
                    Load_AVG(3034);//爱丽丝三
                    break;
                case 4:
                    Load_AVG(3044);//爱丽丝四
                    break;
                case 5:
                    Load_AVG(3054);//爱丽丝五
                    break;
                case 6:
                    Load_AVG(3064);//爱丽丝六
                    break;
                case 7:
                    Load_AVG(3074);//爱丽丝七
                    break;
                case 8:
                    Load_AVG(3084);//爱丽丝八
                    break;
                case 9:
                    Load_AVG(3094);//爱丽丝九
                    break;
                case 10:
                case 11://确实需要11这个状态来代表最后一个CG已近解锁
                    Load_AVG(3104);//爱丽丝十
                    break;
            }
        }


        public void Load_Vs_Hetty_AVG()
        {

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            int Story_Hetty = data.hettyProgress;
            Debug.Log("目前储存的关卡进度_赫蒂" + Story_Hetty);
            if (Story_Hetty <= 0)
            {
                data.hettyProgress = 1;
                SaveManager.SaveGame(data);
            }


            switch (data.hettyProgress)
            {
                case 1:
                    Load_AVG(2011);//赫蒂一
                    break;
                case 2:
                    Load_AVG(2021);//赫蒂二
                    break;
                case 3:
                    Load_AVG(2031);//赫蒂三
                    break;
                case 4:
                    Load_AVG(2041);//赫蒂四
                    break;
                case 5:
                    Load_AVG(2051);//赫蒂五
                    break;
                case 6:
                    Load_AVG(2061);//赫蒂六
                    break;
                case 7:
                    Load_AVG(2071);//赫蒂七
                    break;
                case 8:
                    Load_AVG(2081);//赫蒂八
                    break;
                case 9:
                    Load_AVG(2091);//赫蒂九
                    break;
                case 10:
                case 11://确实需要11这个状态来代表最后一个CG已近解锁
                    Load_AVG(2101);//赫蒂十
                    break;
            }




        }//女荷官指名界面选择（只留下VS_XXX，在下一个场景根据AVG解锁）
        public void Load_Hetty_Lose_AVG()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            switch (data.hettyProgress)
            {
                case 1:
                    Load_AVG(2014);//赫蒂一
                    break;
                case 2:
                    Load_AVG(2024);//赫蒂二
                    break;
                case 3:
                    Load_AVG(2034);//赫蒂三
                    break;
                case 4:
                    Load_AVG(2044);//赫蒂四
                    break;
                case 5:
                    Load_AVG(2054);//赫蒂五
                    break;
                case 6:
                    Load_AVG(2064);//赫蒂六
                    break;
                case 7:
                    Load_AVG(2074);//赫蒂七
                    break;
                case 8:
                    Load_AVG(2084);//赫蒂八
                    break;
                case 9:
                    Load_AVG(2094);//赫蒂九
                    break;
                case 10:
                case 11://确实需要11这个状态来代表最后一个CG已近解锁
                    Load_AVG(2104);//赫蒂十
                    break;
            }
        }



        public void CG_Thumbnail_RePlay(int CG_Number)
        {
            switch (CG_Number)
            {



                case 11:
                    GameFlowData.nextAVGId = "Anto_CG_01_2";//开启安托第一个CG前端AVG
                    break;
                case 12:
                    GameFlowData.nextAVGId = "Anto_CG_02_2";//开启安托第二个CG前端AVG
                    break;
                case 13:
                    GameFlowData.nextAVGId = "Anto_CG_03_2";//开启安托第三个CG前端AVG
                    break;
                case 14:
                    GameFlowData.nextAVGId = "Anto_CG_04_2";//开启安托第四个CG前端AVG
                    break;
                case 15:
                    GameFlowData.nextAVGId = "Anto_CG_05_2";//开启安托第五个CG前端AVG
                    break;
                case 16:
                    GameFlowData.nextAVGId = "Anto_CG_06_2";//开启安托第六个CG前端AVG
                    break;
                case 17:
                    GameFlowData.nextAVGId = "Anto_CG_07_2";//开启安托第七个CG前端AVG
                    break;
                case 18:
                    GameFlowData.nextAVGId = "Anto_CG_08_2";//开启安托第八个CG前端AVG
                    break;
                case 19:
                    GameFlowData.nextAVGId = "Anto_CG_09_2";//开启安托第九个CG前端AVG
                    break;
                case 20:
                    GameFlowData.nextAVGId = "Anto_CG_10_2";//开启安托第十个CG前端AVG
                    break;



                case 21:
                    GameFlowData.nextAVGId = "Hetty_CG_01_2";//开启赫蒂第一个CG前端AVG
                    break;
                case 22:
                    GameFlowData.nextAVGId = "Hetty_CG_02_2";//开启赫蒂第二个CG前端AVG
                    break;
                case 23:
                    GameFlowData.nextAVGId = "Hetty_CG_03_2";//开启赫蒂第三个CG前端AVG
                    break;
                case 24:
                    GameFlowData.nextAVGId = "Hetty_CG_04_2";//开启赫蒂第四个CG前端AVG
                    break;
                case 25:
                    GameFlowData.nextAVGId = "Hetty_CG_05_2";//开启赫蒂第五个CG前端AVG
                    break;
                case 26:
                    GameFlowData.nextAVGId = "Hetty_CG_06_2";//开启赫蒂第六个CG前端AVG
                    break;
                case 27:
                    GameFlowData.nextAVGId = "Hetty_CG_07_2";//开启赫蒂第七个CG前端AVG
                    break;
                case 28:
                    GameFlowData.nextAVGId = "Hetty_CG_08_2";//开启赫蒂第八个CG前端AVG
                    break;
                case 29:
                    GameFlowData.nextAVGId = "Hetty_CG_09_2";//开启赫蒂第九个CG前端AVG
                    break;
                case 30:
                    GameFlowData.nextAVGId = "Hetty_CG_10_2";//开启赫蒂第十个CG前端AVG
                    break;



                case 31:
                    GameFlowData.nextAVGId = "Alice_CG_01_2";//开启爱丽丝第一个CG前端AVG
                    break;
                case 32:
                    GameFlowData.nextAVGId = "Alice_CG_02_2";//开启爱丽丝第二个CG前端AVG
                    break;
                case 33:
                    GameFlowData.nextAVGId = "Alice_CG_03_2";//开启爱丽丝第三个CG前端AVG
                    break;
                case 34:
                    GameFlowData.nextAVGId = "Alice_CG_04_2";//开启爱丽丝第四个CG前端AVG
                    break;
                case 35:
                    GameFlowData.nextAVGId = "Alice_CG_05_2";//开启爱丽丝第五个CG前端AVG
                    break;
                case 36:
                    GameFlowData.nextAVGId = "Alice_CG_06_2";//开启爱丽丝第六个CG前端AVG
                    break;
                case 37:
                    GameFlowData.nextAVGId = "Alice_CG_07_2";//开启爱丽丝第七个CG前端AVG
                    break;
                case 38:
                    GameFlowData.nextAVGId = "Alice_CG_08_2";//开启爱丽丝第八个CG前端AVG
                    break;
                case 39:
                    GameFlowData.nextAVGId = "Alice_CG_09_2";//开启爱丽丝第九个CG前端AVG
                    break;
                case 40:
                    GameFlowData.nextAVGId = "Alice_CG_10_2";//开启爱丽丝第十个CG前端AVG
                    break;
            }

            GameFlowData.returnPath = "cg";//这个是CG鉴赏路径

            LoadingScene_Spine();

        }//主界面菜单打开CG


        public void DeveloperMode(int Story)
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            data.antoProgress = Story;
            SaveManager.SaveGame(data);

            //PlayerPrefs.SetInt("Story", Story);
            //Debug.Log("目前存档进度为" + PlayerPrefs.GetInt("Story"));


        }//作者模式切换进度


        public void ShopToBar()
        {
            GameFlowData.nextAVGId = "StartWork_01";//开启经营AVG
            LoadingScene_Spine();
        }//商店界面回到经营界面

        public void BarToShop()
        {
            GameFlowData.nextAVGId = "StartShop_02";//商人出现
            LoadingScene_Spine();
        }
        public void BarToRecipe()
        {
            GameFlowData.nextAVGId = "StartRecipe";//酒品商店
            LoadingScene_Spine();
        }
        #endregion

        /// <summary>
        /// 作弊按钮
        /// </summary>
        #region
        public void CheatButton()
        {

            Thumbnail_Anto_01.SetActive(true);
            Thumbnail_Anto_02.SetActive(true);
            Thumbnail_Anto_03.SetActive(true);
            Thumbnail_Anto_04.SetActive(true);
            Thumbnail_Anto_05.SetActive(true);
            Thumbnail_Anto_06.SetActive(true);
            Thumbnail_Anto_07.SetActive(true);
            Thumbnail_Anto_08.SetActive(true);
            Thumbnail_Anto_09.SetActive(true);
            Thumbnail_Anto_10.SetActive(true);

            Thumbnail_Hetty_01.SetActive(true);
            Thumbnail_Hetty_02.SetActive(true);
            Thumbnail_Hetty_03.SetActive(true);
            Thumbnail_Hetty_04.SetActive(true);
            Thumbnail_Hetty_05.SetActive(true);
            Thumbnail_Hetty_06.SetActive(true);
            Thumbnail_Hetty_07.SetActive(true);
            Thumbnail_Hetty_08.SetActive(true);
            Thumbnail_Hetty_09.SetActive(true);
            Thumbnail_Hetty_10.SetActive(true);


            Thumbnail_Alice_01.SetActive(true);
            Thumbnail_Alice_02.SetActive(true);
            Thumbnail_Alice_03.SetActive(true);
            Thumbnail_Alice_04.SetActive(true);
            Thumbnail_Alice_05.SetActive(true);
            Thumbnail_Alice_06.SetActive(true);
            Thumbnail_Alice_07.SetActive(true);
            Thumbnail_Alice_08.SetActive(true);
            Thumbnail_Alice_09.SetActive(true);
            Thumbnail_Alice_10.SetActive(true);




            CG_Thumbnail_Menu.SetActive(true);
        }

        #endregion



    }
}
