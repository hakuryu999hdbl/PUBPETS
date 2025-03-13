using UnityEngine;
using System.Collections.Generic;
namespace Baccarat_Game
{
    public enum Flip { FlipUp, Horizontal_FlipUp, FlipDown, Reveal }

    public static class DealQueue
    {
        private static List<GameObject> processList = new List<GameObject>();
        public static bool processing = false;

        public delegate void voidDelegate();
        public static event voidDelegate OnFinishedDealing;

        public static void DealCard(GameObject card)
        {
            processList.Add(card);
            processing = true;
        }

        public static void ProcessCard()
        {
            if (processList.Count > 0)
                processList.RemoveAt(0);
            else
                return;

            if (processList.Count == 0)
            {
                processing = false;
                OnFinishedDealing();
            }
        }
    }
}
