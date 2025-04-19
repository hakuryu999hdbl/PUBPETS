using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Blackjack_Game
{
    public class ShopSlot : MonoBehaviour
    {
        public Image iconImage;
        public Text priceText;

        //public Text CurrentNumber;

        public Button mainButton;

        public GameObject BuyButton;

        private ShopItemData currentItem;
        private ShopManager shopManager;



        public void Setup(ShopItemData item, ShopManager manager)
        {


            currentItem = item;//把ShopItemData带到这个Slot里
            shopManager = manager;//建立和Manager相互联系

            //显示自身当前数量
            //int currentCount = PlayerPrefs.GetInt(currentItem.itemKey, 0); // 没有就默认0
            //CurrentNumber.text = currentCount.ToString();

            iconImage.sprite = item.icon;
            priceText.text = item.currentPrice.ToString();

            mainButton.onClick.RemoveAllListeners();
            mainButton.onClick.AddListener(() =>
            {
                shopManager.HideAllDiagol(); // 先隐藏所有
            currentItem.diagols.SetActive(true);

                shopManager.ShowDetail(currentItem);//告诉shopManager目前选中的商品

            AudioManager_2.SoundPlay(5); // 播放打开音效

            //显示Buy按钮
            shopManager.HideAllBuyButton();
                BuyButton.SetActive(true);
            });
        }

        public void Buy()
        {
            shopManager.BuyItem();
        }
        public void UpdatePrice()
        {
            priceText.text = currentItem.currentPrice.ToString();
        }//价格上涨
    }
}