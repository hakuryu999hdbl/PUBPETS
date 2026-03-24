using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blackjack_Game;

public class FrameEvents : MonoBehaviour
{
    void Start()
    {
        //AudioManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioManager>();
    }

    /// <summary>
    /// 声音
    /// </summary>
    #region
    [Header("声音")]
    public AudioManager AudioManager;
    public AudioSource audioS;



    //------------效果音
    public void _chip() { audioS.PlayOneShot(AudioManager.chip); }



    //衣服类
    public void _SE_Clothes() { audioS.PlayOneShot(AudioManager.SE_Clothes); }
    public void _SE_Clothes_2() { audioS.PlayOneShot(AudioManager.SE_Clothes_2); }
    public void _Effect_tuo() { audioS.PlayOneShot(AudioManager.Effect_tuo); }



    public void _Effect_zipper() { audioS.PlayOneShot(AudioManager.Effect_zipper); }
    public void _Effect_tear1() { audioS.PlayOneShot(AudioManager.Effect_tear1); }





    //触手声
    public void _Jinye_tentacle_short() 
    {

        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_short);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_short_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_short_3);
                break;
        }
        
    }



    
    public void _Jinye_tentacle_slow_one() 
    {
        switch(Random.Range(0,3)) 
        {
            case 0:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_slow_one);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_slow_one_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_slow_one_3);
                break;
        }

    }
    public void _Jinye_tentacle_middle_one()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_middle_one);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_middle_one_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_middle_one_3);
                break;
        }    
    }
    public void _Jinye_tentacle_quick_one() 
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_quick_one);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_quick_one_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.Jinye_tentacle_quick_one_3);
                break;
        }      
    }

    //产卵声

    public void _SE_Egglaying() 
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Egglaying_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Egglaying_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Egglaying_3);
                break;
        }
    }



    //拍 鞭打

    public void _Attack_pai2() { audioS.PlayOneShot(AudioManager.Attack_pai2); }


    public void _Attack_blood()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.Attack_blood1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.Attack_blood2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.Attack_blood3);
                break;
        }

    }


    public void _SE_Whip()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Whip_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Whip_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Whip_3);
                break;
        }

    }

    public void _SE_Dog()
    {
        switch (Random.Range(0, 6))//不让狗叫的太频繁
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Dog_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Dog_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Dog_3);
                break;
          
        }

    }


    //吸舔声音
    public void _Jinv_xitian_fast1()
    { 
        audioS.PlayOneShot(AudioManager.Jinv_xitian_fast1);

    }

    public void _SE_Lick() 
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Lick_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Lick_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Lick_3);
                break;
        }
    }





    public void _Jinye_yanxia() { audioS.PlayOneShot(AudioManager.Jinye_yanxia); }




    //抽插声
    public void _SE_Semen_1()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Semen_1_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Semen_1_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Semen_1_3);
                break;
            case 3:
                audioS.PlayOneShot(AudioManager.SE_Semen_1_4);
                break;
            case 4:
                audioS.PlayOneShot(AudioManager.SE_Semen_1_5);
                break;

            case 5:
                audioS.PlayOneShot(AudioManager.SE_Semen_1);
                break;
        }
    }

    //手淫声
    public void _SE_Semen_2() 
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Semen_2_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Semen_2_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Semen_2_3);
                break;
        }

    }

    //射精声
    public void _SE_Semen_3() 
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Semen_3_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Semen_3_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Semen_3_3);
                break;


            case 3:
                audioS.PlayOneShot(AudioManager.SE_Semen_3);
                break;
        }
        
    }




    public void _SE_Semen_fuck_in() { audioS.PlayOneShot(AudioManager.SE_Semen_fuck_in); }
    public void _SE_Semen_fuck_out() { audioS.PlayOneShot(AudioManager.SE_Semen_fuck_out); }



    //喷水声

    public void _SE_Squirting()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_Squirting_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_Squirting_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_Squirting_3);
                break;
        }
    }

    public void _SE_Water() 
    {
        audioS.PlayOneShot(AudioManager.SE_Water);

    }




    //抚摸声
    public void _SE_LotionGauze()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                audioS.PlayOneShot(AudioManager.SE_LotionGauze_1);
                break;
            case 1:
                audioS.PlayOneShot(AudioManager.SE_LotionGauze_2);
                break;
            case 2:
                audioS.PlayOneShot(AudioManager.SE_LotionGauze_3);
                break;
        }

    }






    //------------安托声音

    public void _Anto_Game_034() { DialogSystem._instance.PlaySpineEventVoice(AudioManager.Anto_Game_034);}
    public void _Anto_Game_035() { DialogSystem._instance.PlaySpineEventVoice(AudioManager.Anto_Game_035); }
    public void _Anto_Game_036() { DialogSystem._instance.PlaySpineEventVoice(AudioManager.Anto_Game_036); }

    //------------爱丽丝声音

    public void _Alice_Game_062() { DialogSystem._instance.PlaySpineEventVoice(AudioManager.Alice_Game_062); }
    public void _Alice_Game_063() { DialogSystem._instance.PlaySpineEventVoice(AudioManager.Alice_Game_063); }
    public void _Alice_Game_066() { DialogSystem._instance.PlaySpineEventVoice(AudioManager.Alice_Game_066); }

    #endregion
}
