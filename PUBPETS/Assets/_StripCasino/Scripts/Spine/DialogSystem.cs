using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;

    private Dictionary<int, TextAsset> textAssets = new Dictionary<int, TextAsset>();


    public int index;
    public float textSpeed;
    bool textFinished;//是否完成打字
    bool cancelTyping;//取消打字
    List<string> textList = new List<string>();

    [Header("这是哪个动画需要的对话")]
    public int animation_number;

    [Header("对话，背景，角色")]
    public GameObject TextButton;


    private void OnEnable()
    {
        //读取textSpeed

        textSpeed = PlayerPrefs.GetFloat("TextSpeed");

        Invoke("Read", 0.1f);

    }//一开始不会产生空白，OnEnable会在Start之前，Awake之后被调用

    public void ForceEndDialogue()
    {
        // 清除当前对话状态
        textList.Clear();
        index = 0;

        // 设置 textFinished 为 true，以便退出正在进行的协程
        textFinished = true;

        // 将对话系统 UI 隐藏
        gameObject.SetActive(false);

        //Debug.Log("对话已强制结束并重置");


    }//强制关闭对话

    void Read()
    {
        // Clear the existing dictionary to avoid key conflicts
        textAssets.Clear();

        switch (PlayerPrefs.GetInt("language"))
        {
            case 0:
                textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Japanese/J_CG_01"));
                textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Japanese/J_CG_02"));
                break;
            case 1:
                textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_CG_01"));
                textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Simplified_Chinese/C1_CG_02"));
                break;
            case 2:
                textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_CG_01"));
                textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Traditional_Chinese/C2_CG_02"));
                break;
            case 3:
                textAssets.Add(1001, Resources.Load<TextAsset>("TXT_English/E_CG_01"));
                textAssets.Add(1002, Resources.Load<TextAsset>("TXT_English/E_CG_02"));
                break;
            case 4:
                textAssets.Add(1001, Resources.Load<TextAsset>("TXT_Korean/K_CG_01"));
                textAssets.Add(1002, Resources.Load<TextAsset>("TXT_Korean/K_CG_02"));
                break;
        }






        // 使用字典查找相应的 TextAsset
        if (textAssets.TryGetValue(animation_number, out TextAsset selectedText))
        {
            GetTextFormFile(selectedText);
        }
        else
        {
            Debug.LogError("No TextAsset found for animation_number: " + animation_number);
        }

        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    public void ShowText()
    {
        if (textFinished && !cancelTyping)
        {
            if (index >= textList.Count) // 添加边界检查
            {
                gameObject.SetActive(false);
                index = 0;

                ChangeStory();//结束位置触发
                Debug.Log("对话已结束");
                return;
            }

            if (gameObject.activeSelf)
            {
                StartCoroutine(SetTextUI());
            }
        }
        else if (!textFinished)
        {
            cancelTyping = !cancelTyping;
        }

    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear(); index = 0;//首先将列表内的字符清空

        var lineDate = file.text.Split('\n');//以回车切割每一段

        foreach (var line in lineDate)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        if (index >= textList.Count)
        {
            Debug.LogWarning("index 超出 textList 范围");
            yield break;
        }

        textFinished = false;
        textLabel.text = "";

        //判断一整行的字符是
        Text text = textLabel;
        switch (textList[index].Trim().ToString())
        {
            //字的颜色
            case "BG":
                text.color = Color.white;
                index++;
                break;





            //case "Girl":
            //    text.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);//粉色
            //    index++;
            //    break;

            case "MAN":
                text.color = new Color(0.0f, 0.68f, 0.93f, 1.0f);//蓝色(市民群众)
                index++;
                break;
            case "DarkRed":
                text.color = new Color(0.8f, 0.2f, 0.2f, 1.0f); // 深红色(梦魔)（女特工）
                index++;
                break;
            case "LightRed":
                text.color = new Color(1.0f, 0.2f, 0.5f, 1.0f); //浅红色(菲西莉亚)
                index++;
                break;
            case "Green":
                text.color = new Color(0.0f, 1.0f, 0.0f, 1.0f); // 绿色（魔族女干部）
                index++;
                break;
            case "LightBlue":
                text.color = new Color(0.68f, 0.85f, 0.9f, 1.0f); // 浅蓝色（艾莉丝）
                index++;
                break;
            case "Gold":
                text.color = new Color(1.0f, 0.84f, 0.0f, 1.0f); // 金色（叛变战姬大队长）
                index++;
                break;
            case "Yellow":
                text.color = new Color(1.0f, 1.0f, 0.0f, 1.0f); // 黄色（莱拉）
                index++;
                break;
            case "Orange":
                text.color = new Color(1.0f, 0.5f, 0.0f, 1.0f); // 橙色(播种母体)
                index++;
                break;
            case "Purple":
                text.color = new Color(0.7f, 0.3f, 0.7f, 1.0f); // 紫色 (女记者)
                index++;
                break;
            case "Gray":
                text.color = new Color(0.7f, 0.75f, 0.8f, 1.0f); // 亮灰色(牧者)（政府特工）(研究员)
                index++;
                break;



            //case "Over":
            //    ChangeStory();//通常对话结束
            //    index++;
            //    break;
            //
            //
            //case "ReStart":
            //    //Spine_FrameEvents.ReStart();//教程结束回主菜单
            //    index++;
            //    break;





        }


        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }

        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }


    //快进按钮触发在这里
    public void ChangeStory()
    {
       



    }
}
