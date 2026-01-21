using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Blackjack_Game
{
    public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance;



    [Header("台词音源")]
    public AudioSource dialogueSource;

    [Header("开局垃圾话")]
    public List<AudioClip> startClips;

    [Header("赢局垃圾话")]
    public List<AudioClip> playerWinClips;

    [Header("输局垃圾话")]
    public List<AudioClip> playerLoseClips;

    [Header("大注垃圾话")]
    public List<AudioClip> bigDealClips;

    [Header("小注垃圾话")]
    public List<AudioClip> smallDealClips;




    public void PlayVoice(VoiceType type)
    {
        // 垃圾话一定会打断娇喘
        PauseMoanLoop();

        AudioClip clip = GetRandomClip(type);
        if (clip == null || dialogueSource == null) return;

        dialogueSource.Stop();
        dialogueSource.clip = clip;
        dialogueSource.loop = false;
        dialogueSource.Play();

        // 播完后恢复娇喘
        //StartCoroutine(ResumeMoanAfterDialogue(clip.length));
    }


    AudioClip GetRandomClip(VoiceType type)
    {
        List<AudioClip> list = null;

        switch (type)
        {
            case VoiceType.Start: list = startClips; break;
            case VoiceType.PlayerWin: list = playerWinClips; break;
            case VoiceType.PlayerLose: list = playerLoseClips; break;
            case VoiceType.BigDeal: list = bigDealClips; break;
            case VoiceType.SmallDeal: list = smallDealClips; break;
        }

        if (list == null || list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }





























    [Header("娇喘音源")]
    public AudioSource moanLoopSource;

    [Header("娇喘音频列表")]
    public List<AudioClip> moanClips = new List<AudioClip>();

    private int voiceInterruptCount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;


        //VoiceManager.instance.StartMoanLoop();//启动娇喘

        //StartMoanLoop();
    }

    public bool CanScream = false;


    public void StartMoanLoop()
    {

        CanScream = false;//TODO:我先暂时去掉娇喘，这个娇喘本身是用在CG里的，不是对局中，暂时去掉

        if (CanScream)
        {


            if (moanClips.Count == 0 || moanLoopSource == null) return;

            // 如果当前已经在播放，避免重复
            if (moanLoopSource.isPlaying) return;

            // 随机选择一个娇喘语音
            int randomIndex = Random.Range(0, moanClips.Count);
            moanLoopSource.clip = moanClips[randomIndex];
            moanLoopSource.loop = true;
            moanLoopSource.Play();

        }
    }

    public void StopMoanLoop()
    {
        moanLoopSource.Stop();
        voiceInterruptCount = 0;
    }

    public void PauseMoanLoop()
    {
        if (moanLoopSource.isPlaying)
        {
            moanLoopSource.Pause();
        }
        voiceInterruptCount++;
    }

    public void ResumeMoanLoop()
    {
        voiceInterruptCount--;
        if (voiceInterruptCount <= 0)
        {
            voiceInterruptCount = 0;
            if (!moanLoopSource.isPlaying && moanLoopSource.clip != null)
                moanLoopSource.UnPause();
        }
    }
}
}