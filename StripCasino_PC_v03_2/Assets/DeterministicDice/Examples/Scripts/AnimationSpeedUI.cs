using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MADD
{
    public class AnimationSpeedUI : MonoBehaviour
    {
        public TextMeshProUGUI _animationSpeedText;
        public Slider _slider;

        void Start()
        {
            _slider.value = FindObjectOfType<AnimationRecorder>()._animationSpeed;
            _animationSpeedText.text = "Animation Speed: " + _slider.value.ToString();
        }

        void FixedUpdate()
        {
            _animationSpeedText.text = "Animation Speed: " + _slider.value.ToString();
        }
    }
}
