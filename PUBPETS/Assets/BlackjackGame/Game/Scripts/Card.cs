using UnityEngine;
using DG.Tweening;
namespace Blackjack_Game
{
    public enum FlipType { FlipUp, FlipDown }

    public class Card : MonoBehaviour
    {

        public CardData cardData;

        [HideInInspector]
        public FlipType side = FlipType.FlipDown;

        public delegate void CardReveal();

        public void Reveal(CardReveal revealCallback)
        {
            side = FlipType.FlipUp;
            DOTween.Sequence()
                    .Append(transform.DOLocalJump(transform.localPosition, .03f, 1, .6f))
                    .Join((transform.DOLocalRotate(new Vector3(0, 180, 0), .6f))).OnComplete(() =>
                   {
                        revealCallback();
                        DealQueue.CheckDealing();
                    });
        }

        //手动修改，卡牌从正面翻回背面
        public delegate void CardConceal();
        public void Conceal(CardConceal concealCallback)
        {
            side = FlipType.FlipDown;
            DOTween.Sequence()
                .Append(transform.DOLocalJump(transform.localPosition, .03f, 1, .6f))
                .Join(transform.DOLocalRotate(new Vector3(180, 180, 0), .6f))
                .OnComplete(() =>
                {
                    concealCallback();
                    DealQueue.CheckDealing();
                });
        }

        public void MoveTo(Vector3 target)
        {
            DOTween.Sequence()
                    .Append(transform.DOJump(target, .1f, 1, 1).SetEase(Ease.InOutSine))
                    .Join((transform.DORotate(new Vector3(0, 180, 180), .3f))).OnComplete(() =>
                    {
                        Destroy(gameObject, .3f);
                    });
        }
    }
}
