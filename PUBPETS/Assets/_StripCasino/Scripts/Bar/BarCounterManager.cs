using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarCounterManager : MonoBehaviour
{
    [Header("摄像头/客人动画器")]
    public Animator mainCamera;
    public Animator Queues_Guest;



    void Start()
    {
        mainCamera.SetInteger("ChangeView", 2);//摄像头朝向女荷官


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
        startText.gameObject.SetActive(true);
        startText.text = "3"; yield return new WaitForSeconds(1.2f);
        startText.gameObject.SetActive(true);
        startText.text = "2"; yield return new WaitForSeconds(1.2f);
        startText.gameObject.SetActive(true);
        startText.text = "1"; yield return new WaitForSeconds(1.2f);
        startText.gameObject.SetActive(true);
        startText.text = "Go!"; yield return new WaitForSeconds(1.2f);
        startText.text = "";

        StartGame();
    }

    void StartGame()
    {
        Items_Button.SetActive(true);
        Items_Work.SetActive(true);

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
        OverLittleItem();

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

        StartDialog();//切换要求
    }

    private Sprite GetRandomSprite(List<Sprite> pool)
    {
        if (pool.Count == 0) return null;

        int index = Random.Range(0, pool.Count);
        Sprite chosen = pool[index];
        pool.RemoveAt(index); // 避免重复
        return chosen;
    }
    #endregion

    /// <summary>
    /// 按顺序点击物品
    /// </summary>
    #region
    [Header("按顺序点击物品")]
    public GameObject Items_Button;
    public GameObject Items_Work;
    public List<GameObject> ItemWork;

    public void LittleItem_1()
    {
        ItemWork[0].SetActive(true);
        ItemWork[1].SetActive(true);
        ItemWork[2].SetActive(true);
    }
    public void LittleItem_2()
    {
        ItemWork[3].SetActive(true);
        ItemWork[4].SetActive(true);
    }
    public void LittleItem_3()
    {
        ItemWork[0].SetActive(true);
        ItemWork[3].SetActive(true);
        ItemWork[5].SetActive(true);
        ItemWork[6].SetActive(true);
    }

    void OverLittleItem()
    {
        foreach (var Items in ItemWork)
        {
            Items.SetActive(false);
        }
    }
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

        // 随机选择一个对话框并显示
        int randomIndex = Random.Range(0, Diagol.Count);
        currentDisplayedDialogue = Diagol[randomIndex];
        currentDisplayedDialogue.SetActive(true);

        switch (randomIndex)
        {
            case 0:
                LittleItem_1();
                break;
            case 1:
                LittleItem_2();
                break;
            case 2:
                LittleItem_3();
                break;
        }

    }

    void OverDialog()
    {
        foreach (var diagol in Diagol)
        {
            diagol.SetActive(false);
        }
    }

    #endregion

}
