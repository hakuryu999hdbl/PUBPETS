using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveName = "CurrentPlayer"; // 存档名
    public string saveTime;                  // 存档时间（字符串）

    public float balance;

    public int antoProgress;
    public int hettyProgress;
    public int aliceProgress;

    public int[] items = new int[8]; // Item_1 ~ Item_8


    // ✅ 加上这个构造函数 ↓↓↓↓↓↓↓↓↓
    public SaveData(string name)
    {
        saveName = name;
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    // ✅ 如果你也调用过 new SaveData() 这种无参数形式，也要保留这个：
    public SaveData()
    {
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
