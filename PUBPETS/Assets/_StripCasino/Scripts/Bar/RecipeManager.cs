using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Blackjack_Game
{
    public class RecipeManager : MonoBehaviour
    {
        /// <summary>
        /// 配方商店
        /// </summary>
        #region

        private void Start()
        {
            RefreshRecipeShopUI();//开始刷新
        }



        [Header("配方商店")]
        public GameObject RecipePanel;//配方商店界面

        public void BuyRecipe(int Wine_Number)
        {
            // 找到对应条目
            var item = shopItems.Find(x => x.wineNumber == Wine_Number);
            if (item == null)
            {
                Debug.LogWarning("未找到商店条目 Wine_Number=" + Wine_Number);
                return;
            }

            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();

            // 已解锁就直接返回（防重复点）
            if (data.unlockedDrinkNames.Contains(item.drinkName))
            {
                Debug.Log("已解锁：" + item.drinkName);
                RefreshRecipeShopUI();
                return;
            }

            // 钱够不够：你项目现在 balance 是 SaveData 里有的
            if (data.balance < item.cost)
            {
                Debug.Log("钱不够，需：" + item.cost);
                // 可在这里播放拒绝SE / 弹提示
                AudioManager_2.SoundPlay(4);
                return;
            }

            // ✅ 扣钱：用 BalanceManager 统一做（会更新UI+写存档）
            BalanceManager.ChangeBalance(-item.cost);

            // ✅ 关键：重新 Load 一次最新存档，避免被旧 data 覆盖
            data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();



            // 解锁
            data.unlockedDrinkNames.Add(item.drinkName);

            // 存档
            SaveManager.SaveGame(data);



            // 刷商店UI + 刷解锁图标（如果你做了解锁图标列表）
            RefreshRecipeShopUI();

            AudioManager_2.SoundPlay(3); // 成功音效
            Debug.Log("购买并解锁：" + item.drinkName);

        }


        //public bool UnlockRecipe(string drinkName)
        //{
        //    SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
        //
        //    if (data.unlockedDrinkNames == null)
        //        data.unlockedDrinkNames = new List<string>();
        //
        //    if (data.unlockedDrinkNames.Contains(drinkName))
        //        return false;
        //
        //    data.unlockedDrinkNames.Add(drinkName);
        //
        //    SaveManager.SaveGame(data);              // ✅只传 data
        //    //RefreshUnlockedRecipesFromSave();
        //    return true;
        //}//存入酒品


        [Header("配方商店条目")]
        public List<RecipeShopItem> shopItems = new List<RecipeShopItem>();

        [System.Serializable]
        public class RecipeShopItem
        {
            public int wineNumber;        // 按钮传入用（1~7）
            public string drinkName;      // 存档key：例如 "魔女之吻"
            public float cost;            // 售价
            public GameObject rowGO;      // 这一行UI（整行隐藏用）
            public Text costText;     // 价格文本（可选）
        }
        public void RefreshRecipeShopUI()
        {
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);

            if (data.unlockedDrinkNames == null)
                data.unlockedDrinkNames = new List<string>();

            foreach (var item in shopItems)
            {
                bool unlocked = data.unlockedDrinkNames.Contains(item.drinkName);

                // 已解锁：整行隐藏（你要的“不显示”）
                if (item.rowGO != null)
                    item.rowGO.SetActive(!unlocked);

                // 如果要动态显示价格
                //if (item.costText != null)
                //    item.costText.text = ((int)item.cost).ToString();
            }
        }//更新酒品

        public void OpenRecipeShop()
        {
            RecipePanel.SetActive(true);
            RefreshRecipeShopUI();
        }//打开商店




        //检测是否解锁所有酒品

        //bool allUnlocked = IsAllRecipesUnlocked();
        //
        //bool IsAllRecipesUnlocked()
        //{
        //    SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
        //
        //    if (data.unlockedDrinkNames == null)
        //        return false;
        //
        //    return data.unlockedDrinkNames.Count >= 10;//这是特殊酒总数
        //}











        #endregion
    }
}