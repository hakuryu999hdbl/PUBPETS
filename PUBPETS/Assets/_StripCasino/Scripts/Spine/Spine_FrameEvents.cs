using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine_FrameEvents : MonoBehaviour
{

    public AudioManager audioManager;

    public Animator Anto_CG_01;
    public Animator Anto_CG_02;
    public Animator Anto_CG_03;

    public Animator Alice_CG_01;
    public Animator Alice_CG_02;
    public Animator Alice_CG_03;

    //当前播放的动画器
    private Animator currentAnimator;
    public GameObject AVG_CG;

    public void SetCurrentAnimator(int Number) 
    {
        switch (Number) 
        {
            case 11:
                currentAnimator = Anto_CG_01;
                break;

            case 12:
                currentAnimator = Anto_CG_02;
                break;
            case 13:
                currentAnimator = Anto_CG_03;
                break;

            case 31:
                currentAnimator = Alice_CG_01;
                break;

            case 32:
                currentAnimator = Alice_CG_02;
                break;

            case 33:
                currentAnimator = Alice_CG_03;
                break;
        }

        currentAnimator.gameObject.SetActive(true);
        AVG_CG.SetActive(true);
    }



    public void TriggerNext() 
    {
        audioManager.Stop();
        currentAnimator.SetTrigger("Next");
    }

}
