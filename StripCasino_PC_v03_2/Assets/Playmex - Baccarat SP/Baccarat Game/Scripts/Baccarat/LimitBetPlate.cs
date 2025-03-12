using DG.Tweening;
using UnityEngine;
using TMPro;
namespace Baccarat_Game
{
    public class LimitBetPlate : MonoBehaviour
    {
        public SpriteRenderer plate;
        public float max = 1000f;
        public float min = 1f;

        public TMP_Text minT;
        public TMP_Text maxT;

        public static LimitBetPlate Instance;

        private void Start()
        {
            Instance = this;
            SetLimit(min, max);
        }

        public static void SetLimit(float min, float max)
        {
            Instance.min = min;
            Instance.max = max;

            Instance.maxT.text = string.Format("<color=red>{0}", max.ToString("0.0"));
            Instance.minT.text = string.Format("<color=red>{0}", min.ToString("0.0"));
        }

        public static bool AllowLimit(float value)
        {
            if (Player.totalBet + value > Instance.max)
            {
                DOTween.Sequence().Append(Instance.plate.DOFade(0, 0)).Append(Instance.plate.DOFade(1, .3f).SetEase(Ease.OutBounce)).Append(Instance.plate.DOFade(0, .3f));
                return false;
            }
            return true;
        }
    }
}
