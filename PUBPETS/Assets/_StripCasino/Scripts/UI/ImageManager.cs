using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{

    public GameObject CG_Panel;
    public Image ShowImage;

    private List<Sprite> currentList; // 当前选中的CG序列
    private int currentIndex = 0;     // 当前显示第几张

    public void ChangeImage(string characterName, int cgIndex)
    {
        CG_Panel.SetActive(true);

        currentList = GetSprites(characterName, cgIndex);

        if (currentList == null || currentList.Count == 0)
        {
            Debug.LogError("CG列表为空");
            return;
        }

        currentIndex = 0;
        ShowImage.sprite = currentList[currentIndex];
    }

    public void NextImage()
    {
        if (currentList == null || currentList.Count == 0) return;

        currentIndex++;

        if (currentIndex >= currentList.Count)
            currentIndex = 0; // 循环

        ShowImage.sprite = currentList[currentIndex];
    }
    public void PrevImage()
    {
        if (currentList == null || currentList.Count == 0) return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = currentList.Count - 1; // 循环

        ShowImage.sprite = currentList[currentIndex];
    }


    public void CloseCG()
    {
        CG_Panel.SetActive(false);
        currentList = null;
        currentIndex = 0;
    }





    // Anto 系列
    public List<Sprite> Thumbnail_Anto_01 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_02 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_03 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_04 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_05 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_06 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_07 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_08 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_09 = new List<Sprite>();
    public List<Sprite> Thumbnail_Anto_10 = new List<Sprite>();

    // Hetty 系列
    public List<Sprite> Thumbnail_Hetty_01 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_02 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_03 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_04 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_05 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_06 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_07 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_08 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_09 = new List<Sprite>();
    public List<Sprite> Thumbnail_Hetty_10 = new List<Sprite>();

    // Alice 系列
    public List<Sprite> Thumbnail_Alice_01 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_02 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_03 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_04 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_05 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_06 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_07 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_08 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_09 = new List<Sprite>();
    public List<Sprite> Thumbnail_Alice_10 = new List<Sprite>();

    // --- 接口套入部分 ---


  




    /// <summary>
    /// 根据角色名和编号获取对应的 Sprite 列表
    /// 接口用法示例：GetSprites("Anto", 1);
    /// </summary>
    public List<Sprite> GetSprites(string characterName, int index)
    {
        // 这里通过简单的 Switch 或映射逻辑将字符串/索引转为具体的 List
        // 如果变量非常多，建议后续将这些 List 放入一个 Dictionary 中管理
        string targetName = $"{characterName}_{index:D2}";

        switch (characterName.ToLower())
        {
            case "anto": return GetAntoList(index);
            case "hetty": return GetHettyList(index);
            case "alice": return GetAliceList(index);
            default: return null;
        }
    }

    private List<Sprite> GetAntoList(int i) => i switch
    {
        1 => Thumbnail_Anto_01,
        2 => Thumbnail_Anto_02,
        3 => Thumbnail_Anto_03,
        4 => Thumbnail_Anto_04,
        5 => Thumbnail_Anto_05,
        6 => Thumbnail_Anto_06,
        7 => Thumbnail_Anto_07,
        8 => Thumbnail_Anto_08,
        9 => Thumbnail_Anto_09,
        10 => Thumbnail_Anto_10,
        _ => null
    };

    private List<Sprite> GetHettyList(int i) => i switch
    {
        1 => Thumbnail_Hetty_01,
        2 => Thumbnail_Hetty_02,
        3 => Thumbnail_Hetty_03,
        4 => Thumbnail_Hetty_04,
        5 => Thumbnail_Hetty_05,
        6 => Thumbnail_Hetty_06,
        7 => Thumbnail_Hetty_07,
        8 => Thumbnail_Hetty_08,
        9 => Thumbnail_Hetty_09,
        10 => Thumbnail_Hetty_10,
        _ => null
    };

    private List<Sprite> GetAliceList(int i) => i switch
    {
        1 => Thumbnail_Alice_01,
        2 => Thumbnail_Alice_02,
        3 => Thumbnail_Alice_03,
        4 => Thumbnail_Alice_04,
        5 => Thumbnail_Alice_05,
        6 => Thumbnail_Alice_06,
        7 => Thumbnail_Alice_07,
        8 => Thumbnail_Alice_08,
        9 => Thumbnail_Alice_09,
        10 => Thumbnail_Alice_10,
        _ => null
    };
}
