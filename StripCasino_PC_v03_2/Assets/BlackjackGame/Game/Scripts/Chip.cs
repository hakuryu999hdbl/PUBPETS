using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace Blackjack_Game
{
    public class Chip : MonoBehaviour, IPointerEnterHandler
    {
        public float value;
        public GameObject rim;

        private Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = transform.localPosition;
        }

        public void OnClick()
        {
            if (!LimitBetPlate.AllowLimit(value))
            {
                transform.DOComplete();
                transform.DOShakePosition(.2f, 4f, 20, 0);
                return;
            }

            if (ResultManager.betsEnabled && BalanceManager.GetBalance() >= value)
            {
                Player.bet += value;
                BalanceManager.ChangeBalance(-value);
                BetHistoryManager._Instance.Add(value);
                ChipManager.AddToStack(StackType.Standard, value);

                PlaySelectAnimation();

                ChipManager.SelectChip(this);
            }
            else
            {
                transform.DOComplete();
                transform.DOShakePosition(.2f, 4f, 20, 0);
            }
        }

        public void PlaySelectAnimation()
        {
            AudioManager.SoundPlay(0);
            transform.DOKill();
            transform.localPosition = originalPosition;
            transform.DOScale(.9f, .8f).SetEase(Ease.OutElastic).From();
        }

        public void Deselected()
        {
            rim.SetActive(false);
        }
        public void Selected()
        {
            rim.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOComplete();
            transform.DOShakePosition(.2f, 2.2f, 10, 0);
        }
    }
}
