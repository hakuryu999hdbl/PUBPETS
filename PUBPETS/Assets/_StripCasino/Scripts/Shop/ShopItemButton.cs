using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Blackjack_Game
{
    public class ShopItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Tooltip("0..3 对应四个槽位")]
        public int slotIndex;
        public ShopManagerSkin manager;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (manager != null) manager.ShowIntroForSlot(slotIndex);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (manager != null) manager.HideAllIntros();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (manager != null) manager.TryBuySlot(slotIndex);
        }
    }
}