using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

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

            //if (PlayerPrefs.GetFloat("TextSpeed") == 0)
            //{
            //    PlayerPrefs.SetFloat("TextSpeed", 0.05f);
            //}//检测文字加载速度，默认为0.05f


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
    }

    public Image Title_Setting_System, Title_Setting_Audio, Title_Setting_Display, Title_Setting_Operation;
    public Sprite Bar_Show, Bar_Hidden;
    public GameObject System, Audio, Display, Operation;

    // Method to call when a tab is clicked
    public void OnTabClick(string tabName)
    {
        AudioManager_2.SoundPlay(4);//手动SE音频替换

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
    /// 检测和删除存档/跳转场景
    /// </summary>
    #region
    public GameObject MakeSureStartNewGameMenu;//确定是否删除存档
    public Button LoadGame;
    public GameObject LoadingImage;
    public void StartCheckSave() 
    {
        if (PlayerPrefs.GetInt("Story")==0) 
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
        PlayerPrefs.SetInt("Story", 1);//记录

        LoadingGame();

    }//在已有存档的情况下开始新游戏

    public void LoadingGame() 
    {
        LoadingImage.SetActive(true);
        SceneManager.LoadScene("BJ_Mobile");
    }//继续游戏

    public void SceneToCG() 
    {
        LoadingImage.SetActive(true);
        SceneManager.LoadScene("Spine"); 
    }//去CG场景

    public void ReStart_DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("删除存档");

        LoadingImage.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }//删除存档

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
            keybindText_Hit.text = "U";
            PlayerPrefs.SetString("KeyBindings_Hit", "U");
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
            keybindText_DoubleDown.text = "O";
            PlayerPrefs.SetString("KeyBindings_DoubleDown", "O");
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
        keybindText_Hit.text = "U";
        PlayerPrefs.SetString("KeyBindings_Hit", "U");

        keybindText_Stand.text = "S";
        PlayerPrefs.SetString("KeyBindings_Stand", "S");

        keybindText_DoubleDown.text = "O";
        PlayerPrefs.SetString("KeyBindings_DoubleDown", "O");

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
    /// 跳转网页
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
    #endregion

    /// <summary>
    /// 頁面設置
    /// </summary>
    #region
    [Header("画面显示方法")]
    public GameObject DisplayMode_1;
    public GameObject DisplayMode_2;

    public bool isDisplayMode = false;

    public void _DisplayMode()
    {
        isDisplayMode = !isDisplayMode;

        if (isDisplayMode)
        {
            DisplayMode_1.SetActive(false);
            DisplayMode_2.SetActive(true);
        }
        else 
        {
            DisplayMode_1.SetActive(true);
            DisplayMode_2.SetActive(false);
        }

    }

    [Header("自由改变游戏画面大小")]
    public GameObject AllowedResizingGameWindow_1;
    public GameObject AllowedResizingGameWindow_2;

    public bool isAllowedResizingGameWindow = false;

    public void _ResizingGameWindow()
    {
        isAllowedResizingGameWindow = !isAllowedResizingGameWindow;

        if (isAllowedResizingGameWindow)
        {
            AllowedResizingGameWindow_1.SetActive(false);
            AllowedResizingGameWindow_2.SetActive(true);
        }
        else
        {
            AllowedResizingGameWindow_1.SetActive(true);
            AllowedResizingGameWindow_2.SetActive(false);
        }

    }

    [Header("保持窗口处于最上方")]
    public GameObject KeepWindowTop_1;
    public GameObject KeepWindowTop_2;

    public bool isKeepWindowTop = false;

    public void _KeepWindowTop()
    {
        isKeepWindowTop = !isKeepWindowTop;

        if (isKeepWindowTop)
        {
            KeepWindowTop_1.SetActive(false);
            KeepWindowTop_2.SetActive(true);
        }
        else
        {
            KeepWindowTop_1.SetActive(true);
            KeepWindowTop_2.SetActive(false);
        }

    }


    [Header("允许后台运行")]
    public GameObject AllowedBackgroundRunning_1;
    public GameObject AllowedBackgroundRunning_2;

    public bool isAllowedBackgroundRunning = false;

    public void _AllowBackgroundRunning()
    {
        isAllowedBackgroundRunning = !isAllowedBackgroundRunning;

        if (isAllowedBackgroundRunning)
        {
            AllowedBackgroundRunning_1.SetActive(false);
            AllowedBackgroundRunning_2.SetActive(true);
        }
        else
        {
            AllowedBackgroundRunning_1.SetActive(true);
            AllowedBackgroundRunning_2.SetActive(false);
        }

    }

    #endregion
}
