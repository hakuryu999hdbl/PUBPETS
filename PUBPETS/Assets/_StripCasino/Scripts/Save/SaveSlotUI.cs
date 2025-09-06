using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Blackjack_Game
{
    public class SaveSlotUI : MonoBehaviour
    {
        public string slotName; // "CurrentPlayer1", "CurrentPlayer2", "CurrentPlayer3"
        public string saveName;//玩家当前储存的这个存档名称

        public Text nameText, timeText, moneyText;
        public Image thumbnail;


        public Sprite defaultThumbnail, Thumbnail_City,
            Thumbnail_Anto_01, Thumbnail_Anto_02, Thumbnail_Anto_03;

        public Image FillImage_Anto, FillImage_Hetty, FillImage_Alice;

        public GameObject Icon_All;//三人柱状图，用于没有存档不显示

        public GameObject X_Button;
        // Start is called before the first frame update
        void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (SaveManager.Exists(slotName))
            {
                //这个槽位有存档

                SaveData data = SaveManager.LoadGame(slotName);
                nameText.text = data.saveName;
                timeText.text = data.saveTime;
                //moneyText.text = $"Money: {data.balance}";
                moneyText.text = $"{data.balance}";

                //未来可以做当前进度最高
                switch (data.antoProgress) 
                {
                    case 2:
                        thumbnail.sprite = Thumbnail_Anto_01;
                        break;
                    case 3:
                        thumbnail.sprite = Thumbnail_Anto_02;
                        break;
                    case 4:
                        thumbnail.sprite = Thumbnail_Anto_03;
                        break;
                    default:
                        thumbnail.sprite = Thumbnail_City;
                        break;
                }

                FillImage_Anto.fillAmount = data.antoProgress / 10f;
                FillImage_Hetty.fillAmount = data.hettyProgress / 10f;
                FillImage_Alice.fillAmount = data.aliceProgress / 10f;

                X_Button.SetActive(true);
            }
            else
            {
                //这个槽位无存档

                nameText.text = "Unnamed";
                timeText.text = "--------------------";
                moneyText.text = "0";

                thumbnail.sprite = defaultThumbnail; // 以后可以换成 data.thumbnail

                X_Button.SetActive(false);

                Icon_All.SetActive(false);//三人柱状图，用于没有存档不显示
            }
        }

        public void OnLoadClicked()
        {
            if (SaveManager.Exists(slotName))
            {
                //点击读取存档

                // 先加载存档数据
                SaveData data = SaveManager.LoadGame(slotName);

                if (data.antoProgress == 0)
                {
                    GameFlowData.nextAVGId = "StartStory_01";//开启开头剧情介绍
                }
                else 
                {
                    GameFlowData.nextAVGId = "StartWork_01";//开启经营AVG

                
                }
                GameFlowData.CurrentPlayer = slotName;//临时储存当前是哪个档

                // 跳转游戏主场景AVG
                UIManager.instance.LoadingScene_Spine();

            }
            else
            {
                //新建存档
                UIManager.instance.SaveNameMenu.SetActive(true);

                
            }

         
        }//点击按钮

        public void OnDeleteClicked()
        {
            if (SaveManager.Exists(slotName))
            {
                SaveManager.DeleteGame(slotName);
                Refresh(); // UI刷新
            }
        }//被删除
    }

   
}
