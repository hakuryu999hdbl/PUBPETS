using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
namespace Baccarat_Game
{
    public class ChipStack : MonoBehaviour
    {

        private Vector3 intiialPosition;

        private float value = 0;

        private List<GameObject> chips;

        static float[] CHIP_VALUES = new float[] { 0.05f, 0.1f, 0.25f, 1, 5, 10, 25, 100 };
        static string[] CHIP_PREFAB_NAMES = new string[] { "chip.05", "chip.1", "chip.25", "chip1", "chip5", "chip10", "chip25", "chip100" };
        private BetSpace betSpaceParent;

        void Start()
        {
            intiialPosition = transform.position;
        }

        public void Add(float value)
        {
            SetValue(this.value + value);
        }

        public void Clear()
        {
            value = 0;

            if (chips != null)
            {
                foreach (GameObject chip in chips)
                {
                    Destroy(chip);
                }
            }

            chips = null;
        }

        public float GetValue()
        {
            return value;
        }

        public void SetValue(float value)
        {
            Clear();

            if (value <= 0)
            {
                return;
            }

            this.value = value;

            chips = new List<GameObject>();

            int currentChipIndex = CHIP_VALUES.Length - 1;

            while (value > 0)
            {
                value = Mathf.Round(value * 100f) / 100f;
                float nextValue = value - CHIP_VALUES[currentChipIndex];

                if (nextValue < 0)
                {
                    currentChipIndex--;
                    if (currentChipIndex < 0)
                    {
                        throw new Exception("Impossible value");
                    }
                    continue;
                }

                value = nextValue;

                GameObject newChip = Instantiate(Resources.Load<GameObject>(CHIP_PREFAB_NAMES[currentChipIndex]));
                newChip.transform.parent = gameObject.transform;
                newChip.transform.localPosition = new Vector3(0, 0.03f * (chips.Count + 1), 0);

                chips.Add(newChip);
            }
        }

        public float Win(float multiplier)
        {
            float winAmount = value * multiplier;

            SetValue(winAmount);

            CollectChips();

            return winAmount;
        }

        public void CollectChips()
        {
            transform.DOMove(ResultManager.GetChipWinPosition(), .4f).SetDelay(1).SetEase(Ease.InSine).OnComplete(ResetStack);
        }

        public void SetBetSpaceParent(BetSpace parent)
        {
            betSpaceParent = parent;
        }

        public void ResetBet()
        {
            //betSpaceParent.ResetLastBet();
        }

        public void ResetStack()
        {
            transform.position = intiialPosition;
            Clear();
        }
    }
}
