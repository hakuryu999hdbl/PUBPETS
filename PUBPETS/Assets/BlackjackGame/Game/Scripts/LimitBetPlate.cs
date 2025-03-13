using DG.Tweening;
using UnityEngine;
using TMPro;
namespace Blackjack_Game
{
    public class LimitBetPlate : MonoBehaviour
    {
        public SpriteRenderer plate;
        public float max = 1000f;
        public float min = 1f;

        public TMP_Text minT;
        public TMP_Text maxT;

        public static LimitBetPlate _Instance;

        private void Start()
        {
            _Instance = this;
            SetLimit(min, max);
        }

        public static void SetLimit(float min, float max)
        {
            _Instance.min = min;
            _Instance.max = max;

            _Instance.maxT.text = string.Format("Max <color=red>{0}", max.ToString("0.0"));
            _Instance.minT.text = string.Format("Min <color=red>{0}", min.ToString("0.0"));
        }

        public static bool AllowLimit(float value)
        {
            if (Player.bet + value > _Instance.max)
            {
                DOTween.CompleteAll();
                Sequence sq = DOTween.Sequence();
                sq
                    .Append(_Instance.plate.transform.DOShakePosition(.3f, .01f, 15))
                    .Join(_Instance.plate.DOColor(new Color(1f, 0, 0, .2f), .4f).SetEase(Ease.OutFlash))
                    .Append(_Instance.plate.DOColor(new Color(0, 0, 0, .2f), .4f).SetEase(Ease.OutFlash));
                return false;
            }
            return true;
        }
    }
}