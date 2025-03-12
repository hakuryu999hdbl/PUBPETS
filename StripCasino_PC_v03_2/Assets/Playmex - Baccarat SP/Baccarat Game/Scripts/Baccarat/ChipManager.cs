using UnityEngine;
using System.Collections;
using System;
namespace Baccarat_Game
{
    public static class ChipManager
    {

        internal static Chip selected = GameObject.Find("ChipButton_1").GetComponent<Chip>();

        internal static float GetSelectedValue()
        {
            Debug.Log("The value is: " + selected.value);
            return selected.value;
        }
    }
}