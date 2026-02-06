using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameFlowData
{
    public static string nextAVGId = null;     //当前选中的AVG名称
    public static string returnPath = null;      // 回来的路径 ("cg" / null 等)
    public static string CurrentPlayer = null;   //目前使用的是哪个存档



    public static void EnsureInit()
    {
        if (string.IsNullOrEmpty(CurrentPlayer))
        {
            CurrentPlayer = PlayerPrefs.GetString("LastPlayer", "");
        }

        if (string.IsNullOrEmpty(nextAVGId))
        {
            nextAVGId = PlayerPrefs.GetString("NextAVGId", "");
        }
    }
}