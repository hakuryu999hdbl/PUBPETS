using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BarCounterManager : MonoBehaviour
{
    [Header("摄像头/客人动画器")]
    public Animator mainCamera;
    public Animator Queues_Guest;



    void Start()
    {
        //mainCamera.SetInteger("ChangeView", 2);//摄像头朝向女荷官

        InitAllGuestSkin(); // 游戏一开始，初始化 5 个皮肤
        StartCoroutine(StartCountdown());//开启营业
    }

    /// <summary>
    /// 321倒计时
    /// </summary>
    #region
    [Header("321倒计时")]
    public TMP_Text startText;

    IEnumerator StartCountdown()
    {
        startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(1);//手动SE音频替换
        startText.text = "3"; yield return new WaitForSeconds(1.2f);
        startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(1);//手动SE音频替换
        startText.text = "2"; yield return new WaitForSeconds(1.2f);
        startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(1);//手动SE音频替换
        startText.text = "1"; yield return new WaitForSeconds(1.2f);
        startText.gameObject.SetActive(true); AudioManager_2.SoundPlay(0);//手动SE音频替换
        startText.text = "Go!"; yield return new WaitForSeconds(1.2f);
        startText.text = "";

        StartGame();
    }

    void StartGame()
    {
        Items_Button.SetActive(true);
        Items_Work.SetActive(true);

        GenerateNewCustomer();//顾客提要求（生成配方）

        
        StartDialog();//随机抽取对话
    }

    #endregion

    /// <summary>
    /// 客人逐步上前
    /// </summary>
    #region
    int LeaveGuestNumber = 1;
    public void Guest_Move()
    {
        OverDialog();//目前要求消失
        

        Queues_Guest.SetTrigger("Move");
        Debug.Log("Move");


    }

    [Header("客人逐步上前")]
    public List<Sprite> GuestSkin;
    public SpriteRenderer Guest_1, Guest_2, Guest_3, Guest_4, Guest_5;
    public void ChangeLeaveGuestSkin()
    {
        List<Sprite> tempList = new List<Sprite>(GuestSkin);
        Sprite newSprite = GetRandomSprite(tempList);

        switch (LeaveGuestNumber)
        {
            case 1:
                Guest_1.sprite = newSprite;
                break;
            case 2:
                Guest_2.sprite = newSprite;
                break;
            case 3:
                Guest_3.sprite = newSprite;
                break;
            case 4:
                Guest_4.sprite = newSprite;
                break;
            case 5:
                Guest_5.sprite = newSprite;
                break;
        }

        // 下一位客人将离开
        LeaveGuestNumber++;

        // 超出就从1重新开始（循环）
        if (LeaveGuestNumber > 5)
            LeaveGuestNumber = 1;

        StartDialog();//随机抽取对话
    }

    private Sprite GetRandomSprite(List<Sprite> pool)
    {
        if (pool.Count == 0) return null;

        int index = Random.Range(0, pool.Count);
        Sprite chosen = pool[index];
        pool.RemoveAt(index); // 避免重复
        return chosen;
    }


    public void InitAllGuestSkin()
    {
        List<Sprite> tempList = new List<Sprite>(GuestSkin); // 克隆可用皮肤列表

       
        Guest_1.sprite = GetRandomSprite(tempList);
        Guest_2.sprite = GetRandomSprite(tempList);
        Guest_3.sprite = GetRandomSprite(tempList);
        Guest_4.sprite = GetRandomSprite(tempList);
        Guest_5.sprite = GetRandomSprite(tempList);
    } // 给 5 位客人各分配一个不重复皮肤


    #endregion

    /// <summary>
    /// 随机显示顾客要求
    /// </summary>
    #region

    [Header("顾客要求列表")]
    public List<GameObject> Diagol = new List<GameObject>();
    private GameObject currentDisplayedDialogue; // 当前显示的对话框



    void StartDialog()
    {

        
        int randomIndex = Random.Range(0, Diagol.Count);
        currentDisplayedDialogue = Diagol[randomIndex];
        currentDisplayedDialogue.SetActive(true);



    }// 随机选择一个对话框并显示

    void OverDialog()
    {
        foreach (var diagol in Diagol)
        {
            diagol.SetActive(false);
        }
    }// 关闭所有对话框

    #endregion




    /// <summary>
    /// 按顺序点击物品
    /// </summary>
    #region
    [Header("按顺序点击物品")]
    public GameObject Items_Button;
    public GameObject Items_Work;



   

    [System.Serializable]
    public class DrinkIngredient
    {
        public string id;
        public Sprite icon;
        public GameObject button;
    }

    public List<DrinkIngredient> allIngredients;
    public Transform hintPanel; // 显示提示栏图标的父物体
    public GameObject hintIconPrefab;

    private List<string> currentRecipe = new List<string>();
    private int currentIndex = 0;
    public void GenerateNewCustomer()
    {
        // 从 allIngredients 中随机抽取 2~5 个不重复的配料 ID
        int count = Random.Range(2, 6); // 随机数量 2~5
        currentRecipe = allIngredients
            .OrderBy(x => Random.value)         // 洗牌
            .Take(count)                        // 取前 count 个
            .Select(i => i.id)                  // 只取 id
            .ToList();

        currentIndex = 0;

        // 清空提示栏 UI
        foreach (Transform child in hintPanel)
            Destroy(child.gameObject);

        // 生成新提示栏
        foreach (string id in currentRecipe)
        {
            var ingredient = allIngredients.Find(i => i.id == id);
            if (ingredient == null || ingredient.icon == null)
            {
                Debug.LogWarning($"未找到或未设置 icon：{id}");
                continue;
            }

            var icon = Instantiate(hintIconPrefab, hintPanel);
            icon.GetComponent<Image>().sprite = ingredient.icon;
        }
    }


    // 这个函数绑定到每个物品按钮上
    public void OnClickIngredient(string id)
    {
        if (id == currentRecipe[currentIndex])
        {
            // 正确 → 隐藏当前提示图标
            hintPanel.GetChild(currentIndex).gameObject.SetActive(false);
            currentIndex++;

            if (currentIndex >= currentRecipe.Count)
            {
                Debug.Log("调酒成功！");
                GenerateNewCustomer();
                Guest_Move();//下一位客人

                AudioManager_2.SoundPlay(2);//手动SE音频替换
            }

            AudioManager_2.SoundPlay(4);//手动SE音频替换

        }
        else
        {
            Debug.Log("按错了，重头来！");
            //GenerateNewCustomer(); // 重置

            AudioManager_2.SoundPlay(5);//手动SE音频替换
        }
    }

    #endregion
}
