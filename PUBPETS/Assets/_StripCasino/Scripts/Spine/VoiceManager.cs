using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance;

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
