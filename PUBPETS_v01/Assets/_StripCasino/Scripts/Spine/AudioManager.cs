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

    public AudioClip Attack_hit2;

    public AudioClip Attack_pai1, Attack_pai2, Attack_whip_1, Attack_whip_2, Attack_whip_3, Attack_whip_4, Attack_whip_5,
                     Effect_tear1, Effect_falldown, Effect_tuo, Effect_zipper, SE_Clothes, Effect_camera, Effect_shower, Effect_water_buku_1, Effect_water_buku_2,
                     SE_Semen_1, SE_Semen_2, SE_Semen_3, SE_Semen_fuck_in, SE_Semen_fuck_out,
                     SE_Water, SE_Set, SE_Reba, SE_Swing, SE_Glass,
                     SE_Wings,

                     Jinye_zhajin1_fast, Jinye_zhajin1_middle, Jinye_zhajin1_one, Jinye_zhajin1_slow,
                     Jinye_yanxia, Jinv_xitian_fast1,

                     Jinye_tentacle_short, Jinye_tentacle_slow, Jinye_tentacle_middle, Jinye_tentacle_quick;

    public AudioClip SE_Rope, SE_Vibrator, SE_Chain_1, SE_Chain_2;
    [Header("叶语嫣")]
    public AudioClip Audio_03_Breath_3_Short_2;
    public AudioClip Audio_03_Breath_0, Audio_03_Breath_1, Audio_03_Breath_2, Audio_03_Breath_3, Audio_03_Breath_4, Audio_03_Breath_5;
    [Header("叶语嫣惨叫")]
    public AudioClip Audio_02_Syllabary_A;
    public AudioClip yyy_duzui1, yyy_duzui2, yyy_duzui3, yyy_duzui4,Rbq_niao_short1, Rbq_niao_short2, Rbq_niao_short3,
                     Audio_04_Scream_Strong_0, Audio_04_Scream_Strong_1, Audio_04_Scream_Strong_2, Audio_04_Scream_Strong_3, Audio_04_Scream_Strong_4, Audio_04_Scream_Strong_5, Audio_04_Scream_Strong_6, Audio_04_Scream_Strong_7;
    public AudioClip Audio_03_Voice_Struggle_1, Audio_03_Voice_Struggle_2, Audio_03_Voice_Strangle,
                     Audio_03_Resist_0, Audio_03_Resist_1, Audio_03_Resist_2, Audio_03_Resist_3, Audio_03_Resist_4, Audio_03_Resist_5,
                     Audio_03_Shame_0, Audio_03_Shame_1, Audio_03_Shame_2, Audio_03_Shame_3;
    public AudioClip Audio_04_Scream_Belly_0, Audio_04_Scream_Belly_1, Audio_04_Scream_Belly_2, Audio_04_Scream_Belly_3, Audio_04_Scream_Belly_4, Audio_04_Scream_Belly_5, Audio_04_Scream_Belly_6, Audio_04_Scream_Belly_7,
                     Audio_04_Scream_Face_0, Audio_04_Scream_Face_1, Audio_04_Scream_Face_2, Audio_04_Scream_Face_3, Audio_04_Scream_Face_4, Audio_04_Scream_Face_5,
                     Audio_04_Swoon_0, Audio_04_Swoon_1, Audio_04_Swoon_2, Audio_04_Swoon_3, Audio_04_Swoon_4, Audio_04_Swoon_5, Audio_04_Swoon_6;
    [Header("叶语嫣被强奸")]
    public AudioClip Audio_03_Cry_2_Short;
    public AudioClip Audio_03_Cry_0, Audio_03_Cry_1, Audio_03_Cry_2;

    public AudioClip Audio_01_Word_Aaaa, Audio_01_Word_Cannot, Audio_01_Word_ThankYou_1, Audio_01_Word_ThankYou_2;
    public AudioClip Audio_01_Word_ForgiveMe, Audio_01_Word_Hentai;
    public AudioClip Audio_01_Word_No_1, Audio_01_Word_No_2, Audio_01_Word_No_3, Audio_01_Word_No_4, Audio_01_Word_No_5, Audio_01_Word_No_6;

    public AudioClip Audio_01_Word_Stop_1, Audio_01_Word_Stop_2, Audio_01_Word_What, Audio_01_Word_This, Audio_01_Word_Wu;

    public AudioClip Audio_03_Fera_Semen_0, Audio_03_Fera_Semen_1, Audio_03_Fera_Swallow_0, Audio_03_Fera_0, Audio_03_Fera_1, Audio_03_Fera_2, Audio_03_Fera_3, Audio_03_Fera_4, Audio_03_Fera_5;

    public AudioClip Audio_03_H_Gasping_0, Audio_03_H_Gasping_1,
                     Audio_03_H_Gasping_Weak_0, Audio_03_H_Gasping_Weak_1,
                     Audio_03_H_Gasping_Quick_0, Audio_03_H_Gasping_Quick_1, Audio_03_H_Gasping_Quick_2,
                     Audio_03_03_H_ContinualClimax_0, Audio_03_03_H_ContinualClimax_1, Audio_03_03_H_ContinualClimax_2, Audio_03_03_H_ContinualClimax_3, Audio_03_03_H_ContinualClimax_4, Audio_03_03_H_ContinualClimax_5, Audio_03_03_H_ContinualClimax_6, Audio_03_03_H_ContinualClimax_7, Audio_03_03_H_ContinualClimax_8, Audio_03_03_H_ContinualClimax_9,
                     Audio_03_H_Gasping_MentalBreakDown_0, Audio_03_H_Gasping_MentalBreakDown_1,
                     Audio_03_H_Pain_0, Audio_03_H_Pain_1,
                     Audio_04_Scream_Weak_0, Audio_04_Scream_Weak_1, Audio_04_Scream_Weak_2, Audio_04_Scream_Weak_3, Audio_04_Scream_Weak_4, Audio_04_Scream_Weak_5;
    [Header("淫叫")]
    public AudioClip Audio_02_Connection_Gasping_Long_0;
    public AudioClip Audio_02_Connection_Gasping_0, Audio_02_Connection_Gasping_1, Audio_02_Connection_Gasping_2, Audio_02_Connection_Gasping_3, Audio_02_Connection_Gasping_4, Audio_02_Connection_Gasping_5, Audio_02_Connection_Gasping_6, Audio_02_Connection_Gasping_7,
                     Audio_02_Connection_Gasping_8, Audio_02_Connection_Gasping_9, Audio_02_Connection_Gasping_10, Audio_02_Connection_Gasping_11, Audio_02_Connection_Gasping_12, Audio_02_Connection_Gasping_13, Audio_02_Connection_Gasping_14, Audio_02_Connection_Gasping_15;
    public AudioClip Audio_02_Connection_Breather_0, Audio_02_Connection_Breather_1, Audio_02_Connection_Breather_2, Audio_02_Connection_Breather_3, Audio_02_Connection_Breather_4, Audio_02_Connection_Breather_5, Audio_02_Connection_Breather_6, Audio_02_Connection_Breather_7, Audio_02_Connection_Breather_8, Audio_02_Connection_Breather_9;
    #endregion
}
