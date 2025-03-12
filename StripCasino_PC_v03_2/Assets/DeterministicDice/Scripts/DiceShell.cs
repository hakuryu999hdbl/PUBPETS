using UnityEngine;

namespace MADD
{
    public class DiceShell : MonoBehaviour
    {
        [HideInInspector]
        public bool isColliding;

        [HideInInspector]
        public bool isNotMoving = false;

        private AudioSource _audioSource;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Reset()
        {
            // ResetTexturetoUnlit();
            isColliding = false;
            isNotMoving = false;
        }

        #region audio utils

        public void PlayCollisionSound()
        {
            if (!_audioSource.isPlaying)
            {
                // variation for the picth
                int pitchShift = Random.Range(0, 2);
                float newPitch = Mathf.Pow(2, pitchShift / 12);
                _audioSource.pitch = newPitch;
                _audioSource.Play();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            isColliding = true;
        }

        private void OnCollisionStay(Collision collision)
        {
            isColliding = false;
        }

        private void OnCollisionExit(Collision collision)
        {
            isColliding = false;
        }
        #endregion
    }
}
