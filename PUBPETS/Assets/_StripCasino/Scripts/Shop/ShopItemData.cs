using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShopItemData
{
    public string itemName;
    public string itemKey; // 对应 PlayerPrefs 的存储键（例如 "Item_1"）

    public Sprite icon;
    public float basePrice;
    public float priceMultiplier = 1.2f;
    public GameObject diagols;

    [HideInInspector]
    public float currentPrice;

    public void ResetPrice()
    {
        currentPrice = basePrice;
    }

    public void IncreasePrice()
    {
        currentPrice = Mathf.CeilToInt(currentPrice * priceMultiplier);
    }
}