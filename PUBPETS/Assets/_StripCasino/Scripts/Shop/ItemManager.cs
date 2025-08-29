using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public GameObject Item_1, Item_2, Item_3, Item_4, Item_5, Item_6, Item_7, Item_8;
    public Text Item_1_Number, Item_2_Number, Item_3_Number, Item_4_Number, Item_5_Number, Item_6_Number, Item_7_Number, Item_8_Number;

    void OnEnable() => UpdateInventoryUI();   // 进入界面时自动刷新
    public void UpdateInventoryUI()
    {
        SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

        Set(Item_1, Item_1_Number, data.Item_1);
        Set(Item_2, Item_2_Number, data.Item_2);
        Set(Item_3, Item_3_Number, data.Item_3);
        Set(Item_4, Item_4_Number, data.Item_4);
        Set(Item_5, Item_5_Number, data.Item_5);
        Set(Item_6, Item_6_Number, data.Item_6);
        Set(Item_7, Item_7_Number, data.Item_7);
        Set(Item_8, Item_8_Number, data.Item_8);
    }

    private void Set(GameObject icon, Text label, int count)
    {
        if (label) label.text = Mathf.Clamp(count, 0, 99).ToString();  // 需要 99 封顶就保留
        if (icon) icon.SetActive(count > 0);
    }
}
