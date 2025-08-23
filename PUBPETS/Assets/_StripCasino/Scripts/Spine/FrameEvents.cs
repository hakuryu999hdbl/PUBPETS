using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void _SE_Clothes() { audioS.PlayOneShot(AudioManager.SE_Clothes); }
    public void _Jinye_tentacle_short() { audioS.PlayOneShot(AudioManager.Jinye_tentacle_short); }

    public void _Jinye_tentacle_slow_one() { audioS.PlayOneShot(AudioManager.Jinye_tentacle_slow_one); }

    public void _Jinye_tentacle_middle_one() { audioS.PlayOneShot(AudioManager.Jinye_tentacle_middle_one); }
    public void _Jinye_tentacle_quick_one() { audioS.PlayOneShot(AudioManager.Jinye_tentacle_quick_one); }

    public void _Attack_pai2() { audioS.PlayOneShot(AudioManager.Attack_pai2); }

    public void _Jinv_xitian_fast1() { audioS.PlayOneShot(AudioManager.Jinv_xitian_fast1); }

    public void _SE_Semen_1() { audioS.PlayOneShot(AudioManager.SE_Semen_1); }
    public void _SE_Semen_2() { audioS.PlayOneShot(AudioManager.SE_Semen_2); }
    public void _SE_Semen_3() { audioS.PlayOneShot(AudioManager.SE_Semen_3); }
    public void _SE_Semen_fuck_in() { audioS.PlayOneShot(AudioManager.SE_Semen_fuck_in); }
    public void _SE_Semen_fuck_out() { audioS.PlayOneShot(AudioManager.SE_Semen_fuck_out); }

    public void _SE_Water() { audioS.PlayOneShot(AudioManager.SE_Water); }



    //------------安托声音

    public void _Anto_Game_034() { audioS.PlayOneShot(AudioManager.Anto_Game_034); }
    public void _Anto_Game_035() { audioS.PlayOneShot(AudioManager.Anto_Game_035); }
    public void _Anto_Game_036() { audioS.PlayOneShot(AudioManager.Anto_Game_036); }

    #endregion
}
