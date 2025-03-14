using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Trigger : MonoBehaviour
{
    public void ButtonVoice() 
    {
        AudioManager_2.SoundPlay(1);//手动SE音频替换
    }
    public void ClickVoice()
    {
        AudioManager_2.SoundPlay(4);//手动SE音频替换
    }
    public void SetActiveFalse() 
    {
        gameObject.SetActive(false);
    }
}
