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
}
