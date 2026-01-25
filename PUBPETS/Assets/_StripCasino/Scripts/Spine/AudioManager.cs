using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{


    public static AudioManager instance { get; private set; }
    public AudioSource audioS;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //audioS = GetComponent<AudioSource>();
    }

    public void AudioPlay(AudioClip clip)
    {
        if (audioS == null)
        {
            Debug.LogWarning("AudioSource is missing or destroyed.");
            return;  // 避免继续尝试播放已经销毁的音源
        }


        audioS.PlayOneShot(clip);
    }

    public void Stop()
    {
        audioS.Stop();
    }

    /// <summary>
    /// 声音
    /// </summary>
    #region

    [Header("效果音")]

    public AudioClip chip;

    public AudioClip SE_Clothes, SE_Clothes_2;
    public AudioClip Effect_tear1, Effect_tuo, Effect_zipper;

    public AudioClip Jinye_tentacle_short, Jinye_tentacle_short_2, Jinye_tentacle_short_3;
    public AudioClip Jinye_tentacle_slow_one, Jinye_tentacle_slow_one_2, Jinye_tentacle_slow_one_3;
    public AudioClip Jinye_tentacle_middle_one, Jinye_tentacle_middle_one_2, Jinye_tentacle_middle_one_3;
    public AudioClip Jinye_tentacle_quick_one, Jinye_tentacle_quick_one_2, Jinye_tentacle_quick_one_3;

    public AudioClip SE_Egglaying_1, SE_Egglaying_2, SE_Egglaying_3;


    public AudioClip Attack_pai2;

    public AudioClip SE_Whip_1, SE_Whip_2, SE_Whip_3;


    public AudioClip Jinv_xitian_fast1;

    public AudioClip SE_Lick_1, SE_Lick_2, SE_Lick_3;

    public AudioClip Jinye_yanxia;


    public AudioClip SE_Semen_1_1, SE_Semen_1_2, SE_Semen_1_3, SE_Semen_1_4, SE_Semen_1_5,
                     SE_Semen_2_1, SE_Semen_2_2, SE_Semen_2_3,
                     SE_Semen_3_1, SE_Semen_3_2, SE_Semen_3_3,

                     SE_Semen_fuck_in, SE_Semen_fuck_out;

    public AudioClip SE_Water;
    public AudioClip SE_Squirting_1, SE_Squirting_2, SE_Squirting_3;


    public AudioClip SE_LotionGauze_1, SE_LotionGauze_2, SE_LotionGauze_3;


    public AudioClip Anto_Game_034, Anto_Game_035, Anto_Game_036;

    #endregion
}
