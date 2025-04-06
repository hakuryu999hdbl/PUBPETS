using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

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
}
