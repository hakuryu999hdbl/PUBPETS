using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Blackjack_Game
{
    public class TableResetManager : MonoBehaviour
    {

        private static TableResetManager _Instance;

        private List<GameObject> cards;

        public Transform discardPoint;
        public Transform drawPoint;
        public Transform chipCollectPoint;

        void Awake()
        {
            _Instance = this;
            cards = new List<GameObject>();
        }

        public static void AddCard(GameObject card)
        {
            _Instance.cards.Add(card);
        }

        public static Transform GetDrawPoint()
        {
            return _Instance.drawPoint;
        }

        public static Vector3 GetChipCollectPoint()
        {
            return _Instance.chipCollectPoint.position;
        }

        public void Cleanup()
        {
            StartCoroutine(ClearTable());
        }

        private IEnumerator ClearTable()
        {
            int cardCount = cards.Count;
            for (int i = 0; i < cardCount; i++)
            {
                cards[i].GetComponentInChildren<Card>().MoveTo(discardPoint.position + Vector3.up * i * 0.005f);
                yield return new WaitForSeconds(.05f);
            }

            cards.Clear();
        }
    }
}
