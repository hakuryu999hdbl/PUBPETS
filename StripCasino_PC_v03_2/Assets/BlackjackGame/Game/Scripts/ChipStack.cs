using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
namespace Blackjack_Game
{
    public class ChipStack : MonoBehaviour
    {
        public StackType type;

        private Vector3 initialPosition;
        private float value = 0;
        private List<GameObject> chips;

        public TMPro.TMP_Text betValuePanel;

        private static readonly float[] CHIP_VALUES = { 0.5f, 1, 5, 10, 25, 100 };

        private bool IsWinnningStack = false;
        private bool IsPushStack = false;
        private bool IsBlackjack = false;

        private void Awake()
        {
            initialPosition = transform.localPosition;
        }

        void Start()
        {
            betValuePanel = GetComponentInChildren<TMPro.TMP_Text>();
            betValuePanel.text = "";
            ChipManager.RegisterStack(this);
        }

        public void Add(float value)
        {
            SetValue(this.value + value);
        }

        public void Remove(float value)
        {
            SetValue(this.value - value);
        }

        public void Clear()
        {
            value = 0;
            betValuePanel.text = "";

            if (chips != null)
            {
                foreach (GameObject chip in chips)
                {
                    Destroy(chip);
                }
            }

            chips = null;
            IsWinnningStack = false;
            IsBlackjack = false;
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
                value = 0;
                return;
            }

            betValuePanel.text = value.ToString();
            this.value = value;

            chips = new List<GameObject>();

            int currentChipIndex = CHIP_VALUES.Length - 1;

            while (value > 0)
            {
                //value = Mathf.Round(value * 100f) / 100f;
                float nextValue = value - CHIP_VALUES[currentChipIndex];

                if (nextValue < 0)
                {
                    currentChipIndex--;
                    if (currentChipIndex < 0)
                    {
                        value = 0;
                    }

                    continue;
                }

                value = nextValue;

                GameObject newChip = ChipManager.InstanciateChip(currentChipIndex);
                newChip.transform.parent = gameObject.transform;
                newChip.transform.localPosition = new Vector3(0, 0.005f * (chips.Count + 1), 0);
                newChip.transform.localRotation = Quaternion.Euler(90, 0, 0);

                chips.Add(newChip);
            }
        }

        public float GetWin()
        {
            float winAmount;

            if (IsWinnningStack)
            {
                winAmount = value * (IsBlackjack ? 2.5f : 2);
            }
            else if (IsPushStack)
            {
                winAmount = value;
            }
            else
            {
                Clear();
                return 0;
            }


            SetValue(winAmount);
            CollectChips();

            return winAmount;
        }

        public void SetWinningState(bool won)
        {
            IsWinnningStack = won;
        }

        public void SetPushState(bool push)
        {
            IsPushStack = push;
        }

        public void SetBlackjackState()
        {
            IsBlackjack = true;
        }

        public void CollectChips()
        {
            transform.DOMove(TableResetManager.GetChipCollectPoint(), .4f).SetDelay(2).SetEase(Ease.InSine)
                .OnComplete(ResetStack);
        }

        public void ResetStack()
        {
            transform.localPosition = initialPosition;
            Clear();
        }
    }
}