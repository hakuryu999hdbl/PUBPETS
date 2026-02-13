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

    public int DayCount;      // 当前经营天数
    public bool HasCleared;   // 是否已经通关

    public int Item_1;//紫色心情
    public int Item_2;//占卜水晶
    public int Item_3;//均衡徽章
    public int Item_4;//魔眼石
    public int Item_5;//酒瓶
    public int Item_6;//藏宝图残片
    public int Item_7;//幸运币
    public int Item_8;//透视药水
    public int Item_9;//绿色心情
    public int Item_10;//匕首
    public int Item_11;//黑棋子
    public int Item_12;//魔眼药水
    public int Item_13;//空瓶
    public int Item_14;//白棋子
    public int Item_15;//厄运币
    public int Item_16;//皇室家徽


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




    // ✅ 新增（最简单）
    public int lastCGGirl = 0; // 0 none, 1 Anto, 2 Hetty, 3 Alice
    public int lastCGIndex = 0; // 1~10

    //你已经解锁的酒类
    public List<string> unlockedDrinkNames = new List<string>(); // ✅新增

}

