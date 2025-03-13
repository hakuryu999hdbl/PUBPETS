using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
namespace Baccarat_Game
{
    public class TableResetManager : MonoBehaviour
    {

        private static TableResetManager _Instance;
        private static List<GameObject> cards;

        [HideInInspector]
        public delegate void voidEvent();
        public static voidEvent Done;

        public Transform discardPile;
        void Start()
        {
            _Instance = this;
            cards = new List<GameObject>();
        }

        public static void AddCard(GameObject card)
        {
            cards.Add(card);
        }

        public void Cleanup()
        {
            int cardCount = cards.Count;
            for (int i = 0; i < cardCount; i++)
                cards[i].GetComponentInChildren<Card>().MoveTo(discardPile.position);

            cards = new List<GameObject>();
            Done();
        }
    }
}
