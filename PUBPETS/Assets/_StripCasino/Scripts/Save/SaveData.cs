using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string slotName;// 存档名
    public string saveName = "CurrentPlayer"; // 主人公名字
    public string saveTime;                  // 存档时间（字符串）

    public float balance;

    public int antoProgress;
    public int hettyProgress;
    public int aliceProgress;



    public int Item_1;
    public int Item_2;
    public int Item_3;
    public int Item_4;
    public int Item_5;
    public int Item_6;
    public int Item_7;
    public int Item_8;


    // ✅ 加上这个构造函数 ↓↓↓↓↓↓↓↓↓
    public SaveData(string name)
    {
        saveName = name;
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //存酒
        unlockedDrinkNames = new List<string>();
    }

    // ✅ 如果你也调用过 new SaveData() 这种无参数形式，也要保留这个：
    public SaveData()
    {
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }




    // ✅ 新增（最简单）
    public int lastCGGirl = 0; // 0 none, 1 Anto, 2 Hetty, 3 Alice
    public int lastCGIndex = 0; // 1~10


    //你已经解锁的酒类
    public List<string> unlockedDrinkNames = new List<string>(); // ✅新增



}

