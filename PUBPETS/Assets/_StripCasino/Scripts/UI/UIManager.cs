using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
namespace Blackjack_Game
{
    public class UIManager : MonoBehaviour
    {

        /// <summary>
        /// 主菜单使用UI
        /// </summary>
        #region

        private void Start()
        {
            Scene currentScene = SceneManager.GetActiveScene(); // 获取当前场景
            if (currentScene.name == "Lobby")
            {
                OnTabClick("System");//主菜单的设置中预先设置为系统版面

                allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));//键位设置

                LoadKeyBindings(); // 在游戏开始时加载键位设置


                if (PlayerPrefs.GetInt("Story") == 0)
                {
                    LoadGame.interactable = false; // 将按钮设为不可交互
                }//检测当前是否有存档


                CheckTextSpeed();//检测文字加载速度，默认为0.05f

                if (PlayerPrefs.GetFloat("TextSpeed") == 0)
                {
                    PlayerPrefs.SetFloat("TextSpeed", 0.05f);
                }//检测文字加载速度，默认为0.05f


                //if (Application.platform == RuntimePlatform.Android)
                //{
                //    Debug.Log("当前是 Android");
                //}
                //else
                //{
                //    Debug.Log("当前是 PC");
                //
                //    if (PlayerPrefs.GetInt("Setting_Windows") == 0)
                //    {
                //        isDisplayMode = true; // 全屏
                //    }//检测当前的画面设置
                //    DisplayMode();
                //    
                //    
                //    if (isDisplayMode)
                //    {
                //        if (PlayerPrefs.GetInt("Setting_ResolutionWindows") == 0)
                //        {
                //            isAllowedResizingGameWindow = true; // 全屏
                //        }//检测当前是否基于当前分辨率全屏
                //        ResizingGameWindow();
                //    
                //    }
                //    else 
                //    {
                //        if (PlayerPrefs.GetInt("Setting_WindowedCurrentResolution") == 0)
                //        {
                //            isWindowedCurrentResolution = true; // 窗口
                //        }//检测当前是否基于当前分辨率全屏
                //        WindowedCurrentResolution();
                //    }
                //    
                //    
                //    
                //    if (PlayerPrefs.GetInt("Setting_AllowBackgroundRunning") == 0)
                //    {
                //        isAllowedBackgroundRunning = true; // 允许
                //    }//检测允许游戏在后台运行
                //    AllowBackgroundRunning();
                //}


            }//主菜单的设置



            //Debug.Log("目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));
            //Debug.Log("目前储存的语言" + PlayerPrefs.GetInt("language"));//0日语 1简体中文 2繁体中文 3英语 4韩语
            //Debug.Log("目前储存的Hit按键设置" + PlayerPrefs.GetString("KeyBindings_Hit"));
            //Debug.Log("目前储存的Stand按键设置: " + PlayerPrefs.GetString("KeyBindings_Stand"));
            //Debug.Log("目前储存的DoubleDown按键设置: " + PlayerPrefs.GetString("KeyBindings_DoubleDown"));
            //Debug.Log("目前储存的Skip按键设置: " + PlayerPrefs.GetString("KeyBindings_Skip"));
            //Debug.Log("目前储存的Confirm按键设置: " + PlayerPrefs.GetString("KeyBindings_Confirm"));
            //Debug.Log("目前储存的Back按键设置: " + PlayerPrefs.GetString("KeyBindings_Back"));
            //Debug.Log("目前是否有存档" + PlayerPrefs.GetInt("Story"));//0没有  1有

            //Debug.Log("目前储存的AVG对话框文字速度" + PlayerPrefs.GetFloat("TextSpeed"));

            Debug.Log("目前储存的窗口设置" + PlayerPrefs.GetInt("Setting_Windows"));//0全屏 1窗口
            Debug.Log("目前储存的最大分辨率全屏设置" + PlayerPrefs.GetInt("Setting_ResolutionWindows"));//0当前分辨率 1非当前分辨率
            Debug.Log("目前储存的最大分辨率窗口化设置" + PlayerPrefs.GetInt("Setting_WindowedCurrentResolution"));//0当前分辨率 1非当前分辨率
            Debug.Log("目前储存的是否允许后台运行" + PlayerPrefs.GetInt("Setting_AllowBackgroundRunning"));//0允许 1不允许


            Debug.Log("目前储存的物品1" + PlayerPrefs.GetInt("Item_1"));
            Debug.Log("目前储存的物品2" + PlayerPrefs.GetInt("Item_2"));
            Debug.Log("目前储存的物品3" + PlayerPrefs.GetInt("Item_3"));
            Debug.Log("目前储存的物品4" + PlayerPrefs.GetInt("Item_4"));
            Debug.Log("目前储存的物品5" + PlayerPrefs.GetInt("Item_5"));
            Debug.Log("目前储存的物品6" + PlayerPrefs.GetInt("Item_6"));
            Debug.Log("目前储存的物品7" + PlayerPrefs.GetInt("Item_7"));
            Debug.Log("目前储存的物品8" + PlayerPrefs.GetInt("Item_8"));

            //PlayerPrefs.SetInt("Item_2", 999);

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
        /// 检测和删除存档/开始游戏选项
        /// </summary>
        #region
        public GameObject MakeSureStartNewGameMenu;//确定是否删除存档
        public Button LoadGame;

        public void StartCheckSave()
        {
            if (PlayerPrefs.GetInt("Story") == 0)
            {
                NewGame();
            }
            else
            {
                MakeSureStartNewGameMenu.SetActive(true);
            }

        }//点击新游戏按钮时

        public void NewGame()
        {
            //初始化项目
            PlayerPrefs.SetFloat("BalanceKey", 1000);

            PlayerPrefs.SetInt("Story", 0);
            LoadingScene_BarCounter();

        }//在已有存档的情况下开始新游戏



        public void ReStart_DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("删除存档");

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
            SceneManager.LoadScene("BJ_Mobile");

        }
        public void LoadingScene_Lobby()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);
            SceneManager.LoadScene("Lobby");

        }
        public void LoadingScene_BarCounter()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);
            SceneManager.LoadScene("BarCounter");

        }
        public void LoadingScene_Spine()
        {
            Time.timeScale = 1f;
            LoadingImage.SetActive(true);
            SceneManager.LoadScene("Spine");

        }

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
        public AudioMixer audioMixer;//效果音
        public AudioMixer audioMixer_2;//BGM

        public void SetVolume(float value)
        {
            audioMixer.SetFloat("MainVolume", value);
        }

        public void SetVolume_2(float value)
        {
            audioMixer_2.SetFloat("MainVolume_2", value);
        }

        public void SetMasterVolume(float value)
        {
            audioMixer.SetFloat("MainVolume", value); // 确保在效果音混音器中存在名为MasterVolume的参数
            audioMixer_2.SetFloat("MainVolume_2", value); // 确保在BGM混音器中存在名为MasterVolume的参数
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
        /// 跳转网页/退出游戏
        /// </summary>
        #region
        public void OpenURL_Patreon()
        {
            Application.OpenURL("https://www.patreon.com/c/NEKOUJI/posts");
        }

        public void OpenURL_Discord()
        {
            Application.OpenURL("https://discord.com/channels/1342112706274267249/1342112706274267252");
        }

        public void OpenURL_Steam()
        {
            Application.OpenURL("https://store.steampowered.com/");
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
        public GameObject DisplayMode_1;
        public GameObject DisplayMode_2;

        public bool isDisplayMode = true;//是否全屏


        public void _DisplayMode()
        {
            isDisplayMode = !isDisplayMode;
            DisplayMode();
        }
        void DisplayMode()
        {
            if (isDisplayMode)
            {

                DisplayMode_1.SetActive(true);
                DisplayMode_2.SetActive(false);

                //Screen.SetResolution(1280, 720, true);//设置1280*720的全屏
                Screen.fullScreen = true;  //设置成全屏
                PlayerPrefs.SetInt("Setting_Windows", 0);

                AllowedResizingGameWindow.SetActive(true);//只有在全屏模式下可以选分辨率全屏
                WindowedCurrent.SetActive(false);//只有在全屏模式下可以选分辨率窗口化
            }
            else
            {
                DisplayMode_1.SetActive(false);
                DisplayMode_2.SetActive(true);


                //Screen.SetResolution(1280, 720, false);//设置为1280 * 720不全屏
                Screen.fullScreen = false;  //退出全屏 
                PlayerPrefs.SetInt("Setting_Windows", 1);

                AllowedResizingGameWindow.SetActive(false);//只有在全屏模式下可以选分辨率全屏
                WindowedCurrent.SetActive(true);//只有在全屏模式下可以选分辨率窗口化
            }

        }
        [Header("设置当前分辨率全屏")]
        public GameObject AllowedResizingGameWindow;//只有在全屏模式下可以选

        public GameObject AllowedResizingGameWindow_1;
        public GameObject AllowedResizingGameWindow_2;

        public bool isAllowedResizingGameWindow = true;

        public void _ResizingGameWindow()
        {
            isAllowedResizingGameWindow = !isAllowedResizingGameWindow;

            ResizingGameWindow();
        }

        void ResizingGameWindow()
        {
            if (isAllowedResizingGameWindow)
            {
                AllowedResizingGameWindow_1.SetActive(true);
                AllowedResizingGameWindow_2.SetActive(false);

                Resolution[] resolutions = Screen.resolutions;//获取设置当前屏幕分辩率全屏
                Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);//设置当前分辨率
                Screen.fullScreen = true;  //设置成全屏

                PlayerPrefs.SetInt("Setting_ResolutionWindows", 0);
            }
            else
            {
                AllowedResizingGameWindow_1.SetActive(false);
                AllowedResizingGameWindow_2.SetActive(true);

                Screen.SetResolution(1280, 720, true);//设置1280*720的全屏

                PlayerPrefs.SetInt("Setting_ResolutionWindows", 1);
            }

        }

        [Header("设置当前分辨率窗口化")]
        public GameObject WindowedCurrent;

        public GameObject WindowedCurrentResolution_1;
        public GameObject WindowedCurrentResolution_2;

        public bool isWindowedCurrentResolution = false;

        public void _WindowedCurrentResolution()
        {
            isWindowedCurrentResolution = !isWindowedCurrentResolution;

            WindowedCurrentResolution();
        }
        public void WindowedCurrentResolution()
        {

            if (isWindowedCurrentResolution)
            {
                WindowedCurrentResolution_1.SetActive(true);
                WindowedCurrentResolution_2.SetActive(false);

                Resolution[] resolutions = Screen.resolutions;//获取设置当前屏幕分辩率全屏
                Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);//设置当前分辨率
                Screen.fullScreen = false;  //设置成窗口化

                PlayerPrefs.SetInt("Setting_WindowedCurrentResolution", 0);
            }
            else
            {
                WindowedCurrentResolution_1.SetActive(false);
                WindowedCurrentResolution_2.SetActive(true);

                Screen.SetResolution(1280, 720, false);//设置1280*720的全屏

                PlayerPrefs.SetInt("Setting_WindowedCurrentResolution", 1);
            }

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
        /// AVG画面
        /// </summary>
        #region
        [Header("AVG画面")]
        public DialogSystem dialog;
        public GameObject AVG;

        public BarCounterManager barCounterManager;


        public void Load_AVG(int Number)
        {

            dialog.animation_number = Number;
            dialog.gameObject.SetActive(true);
            AVG.SetActive(true);
        }
        public void Close_AVG() 
        {
            dialog.gameObject.SetActive(false);
            AVG.SetActive(false);
        }

        public bool GameOver = false;//赌局没有结束
        //退出赌局退出商店
        public void Leave()
        {
            Debug.Log("点击离开");
            if (!GameOver)
            {
                Load_AVG(111);//输给安托，离开赌局
                GameOver = true;//再次遇到就是开启商店
            }
            else 
            {
                //离开商店，回家等第二天
                LoadingScene_BarCounter();//开启第二天经营
             
            }
        }
        #endregion
    }
}
