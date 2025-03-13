using DG.Tweening;
using UnityEngine;
namespace Blackjack_Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuPanels : MonoBehaviour
    {
        private void OnEnable()
        {
            CanvasGroup canvasG = GetComponent<CanvasGroup>();
            transform.DOKill();
            canvasG.DOKill();

            transform.localPosition = Vector3.zero;
            canvasG.alpha = 1;
            Sequence sq = DOTween.Sequence();
            sq
                .Append(transform.DOLocalMoveY(-6f, .4f).From())
                .Join(canvasG.DOFade(0, .2f).From())
                .SetDelay(.3f)
                .SetEase(Ease.OutSine);
        }

        private void OnDisable()
        {
            transform.DOKill(true);
            transform.localPosition = Vector2.zero;
        }
    }
}