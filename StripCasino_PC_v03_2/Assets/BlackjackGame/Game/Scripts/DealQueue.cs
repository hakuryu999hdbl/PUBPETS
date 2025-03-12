using UnityEngine;
using System.Collections.Generic;
namespace Blackjack_Game
{
    public static class DealQueue
    {
        private static List<GameObject> processList = new List<GameObject>();
        public static bool processing = false;
        public static int CardCount = 0;
        public delegate void voidDelegate();
        public static event voidDelegate OnFinishedDealing;

        public static void DealCard(GameObject card)
        {
            processList.Add(card);
            AudioManager.SoundPlay(1);
            processing = true;
            CardCount++;
        }

        public static void ProcessCard()
        {
            if (processList.Count > 0)
            {
                processList.RemoveAt(0);
            }
            else
                return;

            if (processList.Count == 0)
            {
                processing = false;
                Debug.Log("Processed card");
                OnFinishedDealing();
            }
        }

        public static void CheckDealing()
        {
            OnFinishedDealing();
        }
    }
}