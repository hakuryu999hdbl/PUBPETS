using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //--------语言
    public void Setlanguage(int number)
    {
        PlayerPrefs.SetInt("language", number);
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

    public AudioMixer audioMixer;
    public AudioMixer audioMixer_2;

    public void SetVolume(float value) 
    {
        audioMixer.SetFloat("MainVolume", value);
    }

    public void SetVolume_2(float value)
    {
        audioMixer_2.SetFloat("MainVolume_2", value);
    }
}
