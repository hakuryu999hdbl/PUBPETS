using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine_FrameEvents : MonoBehaviour
{

    public AudioManager audioManager;

    public Animator CG_01;
    public Animator CG_02;


    public void TriggerNext_1() 
    {
        audioManager.Stop();
        CG_01.SetTrigger("Next");
    }
    public void TriggerNext_2()
    {
        audioManager.Stop();
        CG_02.SetTrigger("Next");
    }
}
