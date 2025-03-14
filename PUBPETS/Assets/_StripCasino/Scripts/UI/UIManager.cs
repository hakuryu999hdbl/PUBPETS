using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    /// <summary>
    /// 主菜单使用UI
    /// </summary>
    #region

    private void Start()
    {
        OnTabClick("System");
    }

    public Image Title_Setting_System, Title_Setting_Audio, Title_Setting_Display, Title_Setting_Operation;
    public Sprite Bar_Show, Bar_Hidden;
    public GameObject System,Audio,Display,Operation;

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

    //--------语言
    public void Setlanguage(int number)
    {
        AudioManager_2.SoundPlay(4);//手动SE音频替换

        PlayerPrefs.SetInt("language", number);//0日语 1简体中文 2繁体中文 3英语 4韩语
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //--------删除存档
    public void ReStart_DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("删除存档");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    /// <summary>
    /// 既存UI的利用
    /// </summary>
    #region



    [Header("既存UI的利用")]

    public GameObject volumeSlider_1; // 在Inspector中设置这个变量为你的Slider对象
    public bool isOn = false;
    public void OnMusicToggleChanged()
    {
        isOn = !isOn;

        // 在这里添加更多逻辑来处理音乐的开/关
        if (isOn)
        {
            // 代码来开启音乐
            volumeSlider_1.SetActive(true);
        }
        else
        {
            // 代码来关闭音乐
            volumeSlider_1.SetActive(false);
        }
    }


    public GameObject volumeSlider_2; // 在Inspector中设置这个变量为你的Slider对象
    public bool isOn_2 = false;
    public void OnMusicToggleChanged_2()
    {
        isOn_2 = !isOn_2;

        // 在这里添加更多逻辑来处理音乐的开/关
        if (isOn_2)
        {
            // 代码来开启音乐
            volumeSlider_2.SetActive(true);
        }
        else
        {
            // 代码来关闭音乐
            volumeSlider_2.SetActive(false);
        }
    }

    #endregion

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
    }
}
