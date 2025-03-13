using UnityEngine;
using DG.Tweening;
namespace Baccarat_Game
{
    public class Card : MonoBehaviour
    {

        public CardData cardData;

        private Animator animator;
        private MeshRenderer renderCard;

        private void Start()
        {
            animator = GetComponent<Animator>();
            renderCard = GetComponent<MeshRenderer>();
        }

        public void MoveTo(Vector3 target)
        {
            DOTween.Sequence()
                    .Append(transform.DOMove(target, 1))
                    .Join((transform.DORotate(new Vector3(0, 0, 180), .5f))).OnComplete(() =>
                    {
                        Destroy(gameObject);
                    });
        }
    }
}
