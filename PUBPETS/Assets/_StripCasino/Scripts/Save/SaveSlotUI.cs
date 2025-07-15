using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Blackjack_Game
{
    public class SaveSlotUI : MonoBehaviour
    {
        public string slotName; // "CurrentPlayer1", "CurrentPlayer2", "CurrentPlayer3"

        public Text nameText, timeText, moneyText, antoLvText, hettyLvText, aliceLvText;
        public Image thumbnail;


        public Sprite defaultThumbnail, Thumbnail_1;


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
                moneyText.text = $"Money: {data.balance}";
                antoLvText.text = $"Anto Lv {data.antoProgress}";
                hettyLvText.text = $"Hetty Lv {data.hettyProgress}";
                aliceLvText.text = $"Alice Lv {data.aliceProgress}";

                thumbnail.sprite = Thumbnail_1; // 以后可以换成 data.thumbnail

                X_Button.SetActive(true);
            }
            else
            {
                //这个槽位无存档

                nameText.text = "Unnamed";
                timeText.text = "--------------------";
                moneyText.text = "Money: 0";
                antoLvText.text = "Anto Lv -";
                hettyLvText.text = "Hetty Lv -";
                aliceLvText.text = "Alice Lv -";

                thumbnail.sprite = defaultThumbnail; // 以后可以换成 data.thumbnail

                X_Button.SetActive(false);

            }
        }

        public void OnLoadClicked()
        {
            if (SaveManager.Exists(slotName))
            {
                //点击读取存档


                GameFlowData.nextAVGId = "StartWork_01";//开启经营AVG
            }
            else
            {
                // 新建存档
                SaveData newData = new SaveData(slotName);

                newData.saveName = slotName;//记住档的名字
                newData.balance = 1000;//初始给与1000


                SaveManager.SaveGame(newData);

                GameFlowData.nextAVGId = "StartStory_01";//开启开头剧情介绍
            }

            GameFlowData.CurrentPlayer = slotName;//临时储存当前是哪个档

            // 跳转游戏主场景AVG
            UIManager.instance.LoadingScene_Spine();
        }//点击按钮

        public void OnDeleteClicked()
        {
            if (SaveManager.Exists(slotName))
            {
                SaveManager.DeleteGame(slotName);
                Refresh(); // UI刷新
            }
        }
    }

   
}
