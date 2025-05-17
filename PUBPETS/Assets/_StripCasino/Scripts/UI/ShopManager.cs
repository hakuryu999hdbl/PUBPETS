using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Blackjack_Game
{
    public class ShopManager : MonoBehaviour
    {

        [Header("商品配置")]
        public List<ShopItemData> allItems;

        [Header("商品槽位")]
        public ShopSlot[] slots; // 4个槽位预制体引用

        void Start()
        {
            RefreshShop();

            UpdateInventoryUI(); // 👈 游戏开始更新一次物品栏
        }

        public void RefreshShop()
        {
            List<ShopItemData> candidates = new List<ShopItemData>(allItems);
            for (int i = 0; i < slots.Length; i++)
            {
                int rand = Random.Range(0, candidates.Count);
                ShopItemData item = candidates[rand];
                item.ResetPrice();
                slots[i].Setup(item, this);
                candidates.RemoveAt(rand);
            }
        }

        public void HideAllDiagol()
        {
            foreach (ShopItemData item in allItems)
            {
                if (item.diagols != null)
                    item.diagols.SetActive(false);
            }
        } //隐藏所有Diagol

        public void HideAllBuyButton()
        {
            foreach (ShopSlot slot in slots)
            {
                if (slot.BuyButton != null)
                    slot.BuyButton.SetActive(false);
            }
        }//隐藏所有购买按钮

        [Header("详情窗口")]
        private ShopItemData selectedItem;

        public void ShowDetail(ShopItemData item)
        {
            selectedItem = item;
        }

        public void BuyItem()
        {

            if (PlayerPrefs.GetFloat("BalanceKey") >= selectedItem.currentPrice)
            {
                // 扣钱
                BalanceManager.ChangeBalance(-selectedItem.currentPrice);

                // 提升价格
                Debug.Log("购买成功：" + selectedItem.itemName);
                selectedItem.IncreasePrice();




                // 增加该物品数量
                string key = selectedItem.itemKey;
                int currentCount = PlayerPrefs.GetInt(key, 0); // 没有就默认0
                currentCount++;
                PlayerPrefs.SetInt(key, currentCount);

                //价格上涨
                foreach (ShopSlot slot in slots)
                {
                    slot.UpdatePrice();
                }



                //查看库存
                //for (int i = 1; i <= 7; i++)
                //{
                //    Debug.Log($"Item_{i} 数量：" + PlayerPrefs.GetInt($"Item_{i}", 0));
                //}

                UpdateInventoryUI();


                //RefreshShop(); // 可选：刷新商品

                AudioManager_2.SoundPlay(3); // 播放打开音效
            }
            else
            {
                AudioManager_2.SoundPlay(4); // 播放打开音效
            }
        }



        public GameObject Item_1, Item_2, Item_3, Item_4, Item_5, Item_6, Item_7, Item_8;

        public Text Item_1_Number, Item_2_Number, Item_3_Number, Item_4_Number, Item_5_Number, Item_6_Number, Item_7_Number, Item_8_Number;

        public void UpdateInventoryUI()
        {
            Item_1_Number.text = PlayerPrefs.GetInt("Item_1", 0).ToString(); Item_1.SetActive(PlayerPrefs.GetInt("Item_1", 0) > 0); 
            Item_2_Number.text = PlayerPrefs.GetInt("Item_2", 0).ToString(); Item_2.SetActive(PlayerPrefs.GetInt("Item_2", 0) > 0); 
            Item_3_Number.text = PlayerPrefs.GetInt("Item_3", 0).ToString(); Item_3.SetActive(PlayerPrefs.GetInt("Item_3", 0) > 0); 
            Item_4_Number.text = PlayerPrefs.GetInt("Item_4", 0).ToString(); Item_4.SetActive(PlayerPrefs.GetInt("Item_4", 0) > 0); 
            Item_5_Number.text = PlayerPrefs.GetInt("Item_5", 0).ToString(); Item_5.SetActive(PlayerPrefs.GetInt("Item_5", 0) > 0); 
            Item_6_Number.text = PlayerPrefs.GetInt("Item_6", 0).ToString(); Item_6.SetActive(PlayerPrefs.GetInt("Item_6", 0) > 0); 
            Item_7_Number.text = PlayerPrefs.GetInt("Item_7", 0).ToString(); Item_7.SetActive(PlayerPrefs.GetInt("Item_7", 0) > 0);
            Item_8_Number.text = PlayerPrefs.GetInt("Item_8", 0).ToString(); Item_8.SetActive(PlayerPrefs.GetInt("Item_8", 0) > 0);
        }

    }
}