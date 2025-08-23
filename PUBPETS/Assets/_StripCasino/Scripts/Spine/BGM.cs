using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM instance { get; private set; }
    public AudioSource audioS;


    [Header("背景音乐")]
    public List<AudioClip> BackgroundMusicList;// 使用List来存储多个音乐

    public bool isPlaying;
    public int WhichMusic;//0无指定，随机

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //audioS = GetComponent<AudioSource>();

        if (WhichMusic == -1)
        {
            AudioPlayBackgroundMusic(-1);//随机播放一首
        }
        else
        {
            AudioPlayBackgroundMusic(WhichMusic);
        }

    }

  
    //Lobby和BJ场景是预先赋值完毕,酒馆经营场景和AVG都是后来脚本设置

    public void AudioPlayBackgroundMusic(int BGMNumber)
    {
        if (!isPlaying && BackgroundMusicList.Count > 0)
        {

            if (BGMNumber < 0)
            {
                // 从列表中随机选择一首音乐
                audioS.clip = BackgroundMusicList[Random.Range(0, BackgroundMusicList.Count)];
            }
            else
            {
                audioS.clip = BackgroundMusicList[BGMNumber];
            }//如果是小于0，那么随机播放，如果大于0，那么指定该序号播放


            // 将音频片段赋值给AudioSource的clip，并播放
            audioS.loop = true;  // 确保启用了循环播放
            audioS.Play();
            isPlaying = true;
        }

    }

    public void Stop()
    {
        audioS.Stop();
        isPlaying = false;
    }
}
