using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MADD
{
    public class DiceConfiguratorUI : MonoBehaviour
    {
        public GameObject _configuratorPrefab;
        public List<DiceSetup> _dice = new List<DiceSetup>();

        public void AddConfig()
        {
            DiceSetup setup = new DiceSetup();
            _dice.Add(setup);
            int index = _dice.Count - 1;
            GameObject go = Instantiate(_configuratorPrefab, transform);
            TMP_Dropdown[] dropdowns = go.GetComponentsInChildren<TMP_Dropdown>();
            // configure dice type
            dropdowns[0]
                .onValueChanged.AddListener(
                    (value) =>
                    {
                        dropdowns[1].ClearOptions();

                        int faces = SetDiceType(index, value);

                        List<string> options = new List<string>();
                        for (int i = 0; i < faces; i++)
                        {
                            if (i == 0)
                                options.Add("Any");
                            options.Add((i + 1).ToString());
                        }

                        dropdowns[1].AddOptions(options);
                    }
                );

            // and the initial values
            dropdowns[1].ClearOptions();

            int faces = SetDiceType(index, 0);

            List<string> options = new List<string>();
            for (int i = 0; i < faces; i++)
            {
                if (i == 0)
                    options.Add("Any");
                options.Add((i + 1).ToString());
            }

            dropdowns[1].AddOptions(options);

            // configure expected result
            dropdowns[1]
                .onValueChanged.AddListener(
                    (value) =>
                    {
                        SetDiceResult(index, value);
                    }
                );
        }

        public int SetDiceType(int index, int type)
        {
            switch (type)
            {
                case 0:
                    _dice[index].faces = 4;
                    break;
                case 1:
                    _dice[index].faces = 6;
                    break;
                case 2:
                    _dice[index].faces = 8;
                    break;
                case 3:
                    _dice[index].faces = 10;
                    break;
                case 4:
                    _dice[index].faces = 12;
                    break;
                case 5:
                    _dice[index].faces = 20;
                    break;
            }

            return _dice[index].faces;
        }

        public void SetDiceResult(int index, int result)
        {
            if (result == 0)
                result = -1;

            _dice[index].result = result;
        }

        public void ThrowDice()
        {
            DiceManager manager = FindObjectOfType<DiceManager>();
            manager._diceSetup = _dice;
            manager.Roll();
        }
    }
}
