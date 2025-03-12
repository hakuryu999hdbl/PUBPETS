using UnityEngine;
using DG.Tweening;
namespace Blackjack_Game
{
    public class PlayerHand : Hand
    {
        private bool stand = false;
        public bool splitAceHand = false;


        [Space]
        [Header("Split")]
        public Vector3 offsetSplitPosition;
        public GameObject selectionArrow;

        override public void ResetHand()
        {
            base.ResetHand();
            transform.position = originalPosition;
            stand = false;
            splitAceHand = false;
            selectionArrow.SetActive(false);
        }

        public void MoveOnSplit()
        {
            transform.DOMove(transform.position + offsetSplitPosition, .3f).SetEase(Ease.OutSine);
        }

        public void Stand()
        {
            stand = true;
            selectionArrow.SetActive(false);
        }

        public void Split(Card card, FlipType flip)
        {
            _Cards.Add(card);
            AddCardToHand(card.gameObject);
            AnimateToPosition(card.gameObject, flip);
            selectionArrow.SetActive(true);
        }

        public Card RemoveSecondCard()
        {
            Card card = _Cards[_Cards.Count - 1];
            _Cards.Remove(card);
            CalculateScore();
            UpdateScoreView();
            UpdateNextPosition();
            MoveOnSplit();
            return card;
        }

        public bool CanHit()
        {
            return currentScore > 0 && !IsEnded();
        }

        public bool IsEnded()
        {
            if (currentScore >= 21 || stand || splitAceHand)
            {
                selectionArrow.SetActive(false);
                return true;
            }

            return false;
        }

        public int TakeValueFromCard(int index)
        {
            return _Cards[index].cardData.GetValue();
        }

        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector3(.12f, .005f, .15f));
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position + offsetSplitPosition, new Vector3(.12f, .005f, .15f));
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + cardShift, new Vector3(.12f, .005f, .15f));
        }
    }
}