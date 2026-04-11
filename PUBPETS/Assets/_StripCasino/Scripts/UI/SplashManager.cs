using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{

    public AudioSource audioS;

    public AudioClip NEKOUJI, FTgirl;


    void PlayNEKOUJI() 
    {
        audioS.PlayOneShot(NEKOUJI);
    }
    void PlayFTgirl()
    {
        audioS.PlayOneShot(FTgirl);
    }

    void LoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }





    private void Start()
    {
        InitLanguageOnce();//开始的时候检测本地语言并设置，第二次打开不再设置
    }


    void InitLanguageOnce()
    {
        // 是否已经做过语言初始化
        if (PlayerPrefs.HasKey("language_initialized"))
            return;

        int lang = DetectSystemLanguage();

        PlayerPrefs.SetInt("language", lang);
        PlayerPrefs.SetInt("language_initialized", 1);
        PlayerPrefs.Save();
    }

    int DetectSystemLanguage()
    {
        SystemLanguage sys = Application.systemLanguage;

        switch (sys)
        {
            case SystemLanguage.Japanese:
                return 0;

            case SystemLanguage.ChineseSimplified:
                return 1;

            case SystemLanguage.ChineseTraditional:
                return 2;

            case SystemLanguage.Korean:
                return 4;

            default:
                return 3; // 默认英语
        }
    }

}
