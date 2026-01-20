using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine_FrameEvents : MonoBehaviour
{

    public AudioManager audioManager;

    public Animator Anto_CG_01;
    public Animator Anto_CG_02;
    public Animator Anto_CG_03;

    public Animator Hetty_CG_01;

    public Animator Alice_CG_01;
    public Animator Alice_CG_02;
    public Animator Alice_CG_03;
    public Animator Alice_CG_04;
    public Animator Alice_CG_05;
    public Animator Alice_CG_06;
    public Animator Alice_CG_08;
    public Animator Alice_CG_09;
    public Animator Alice_CG_10;




    //当前播放的动画器
    private Animator currentAnimator;
    public GameObject AVG_CG;

    public void SetCurrentAnimator(int Number)
    {
        switch (Number)
        {

            #region 安托
            case 11:
                currentAnimator = Anto_CG_01;
                break;

            case 12:
                currentAnimator = Anto_CG_02;
                break;
            case 13:
                currentAnimator = Anto_CG_03;
                break;
            #endregion


            #region 赫蒂
            case 21:
                currentAnimator = Hetty_CG_01;
                break;

            #endregion



            #region 爱丽丝

            case 31:
                currentAnimator = Alice_CG_01;
                break;

            case 32:
                currentAnimator = Alice_CG_02;
                break;

            case 33:
                currentAnimator = Alice_CG_03;
                break;

            case 34:
                currentAnimator = Alice_CG_04;
                break;

            case 35:
                currentAnimator = Alice_CG_05;
                break;

            case 36:
                currentAnimator = Alice_CG_06;
                break;

            case 38:
                currentAnimator = Alice_CG_08;
                break;

            case 39:
                currentAnimator = Alice_CG_09;
                break;

            case 40:
                currentAnimator = Alice_CG_10;
                break;

           #endregion



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
