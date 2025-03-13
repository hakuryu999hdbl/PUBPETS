using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AudioManager_2 : MonoBehaviour
    {
        public static AudioManager_2 _Instance;

        public AudioClip[] audioClip;

        public AudioSource AudioSource;

        void Start()
        {
            _Instance = this;

           
        }


        public static void SoundPlay(int iType)
        {

            AudioSource aS = _Instance.GetComponent<AudioSource>();
            aS.PlayOneShot(_Instance.audioClip[iType]);
        }
    }
