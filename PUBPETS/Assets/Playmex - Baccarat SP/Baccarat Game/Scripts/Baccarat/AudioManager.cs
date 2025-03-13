using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Baccarat_Game
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager _Instance;

        public int maxAudioSourcePool = 15;
        public AudioClip[] audioClip;
        private List<AudioSource> AudioSourcePool;
        public AudioSource AudioSourceBGM;

        public static float MusicVolume = .6f;
        public static float SoundVolume = .6f;

        public UIStates ui;

        void Awake()
        {
            _Instance = this;

            AudioSourcePool = new List<AudioSource>();
            AudioSourceAlloc();
        }

        private void Start()
        {
            ui.musicToggle.onValueChanged.AddListener(ToggleVolumeSlider);
            ui.volumeSlider.onValueChanged.AddListener(ChangeVolume);
            ui.volumeSlider.gameObject.SetActive(false);
        }

        public void ToggleVolumeSlider(bool value)
        {
            ui.volumeSlider.gameObject.SetActive(value);
        }

        public void ChangeVolume(float value)
        {
            MusicVolume = value;
            SoundVolume = value;
            _Instance.AudioSourceBGM.volume = MusicVolume;
        }

        public void AudioSourceAlloc()
        {
            AudioSourcePool.Clear();

            for (int i = 0; i < maxAudioSourcePool; ++i)
            {
                AudioSource aS = gameObject.AddComponent<AudioSource>();
                aS.loop = false;
                AudioSourcePool.Add(aS);
            }
        }

        public AudioSource AudioSourcePop()
        {
            if (AudioSourcePool.Count <= 0) return null;

            AudioSource aS = AudioSourcePool[0];
            AudioSourcePool.RemoveAt(0);
            AudioSourcePool.Add(aS);

            return aS;
        }

        public static void SoundPlay(int iType)
        {
            if (SoundVolume == 0) return;

            AudioSource aS = _Instance.AudioSourcePop();
            aS.volume = SoundVolume;
            aS.PlayOneShot(_Instance.audioClip[iType]);
        }

        public static void SoundPlay(AudioClip clip)
        {
            if (SoundVolume == 0) return;
            if (clip == null) return;

            AudioSource aS = _Instance.AudioSourcePop();
            aS.volume = SoundVolume;
            aS.PlayOneShot(clip);
        }

        public void SoundPlayCoroutine(int iType, float fDelay)
        {
            StartCoroutine(SoundPlayIn(iType, fDelay));
        }

        public IEnumerator SoundPlayIn(int iType, float fDelay)
        {

            if (fDelay > 0.0001f) yield return new WaitForSeconds(fDelay);

            SoundPlay(iType);
        }

        public static void MusicPlay()
        {
            if (MusicVolume == 0) return;
            _Instance.AudioSourceBGM.volume = MusicVolume;
            _Instance.AudioSourceBGM.Play();
        }

        public static bool MusicIsPlaying()
        {
            return _Instance.AudioSourceBGM.isPlaying;
        }

        public static void MusicStop()
        {
            _Instance.AudioSourceBGM.Stop();
        }
    }
}
