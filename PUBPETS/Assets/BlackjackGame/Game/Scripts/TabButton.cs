using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace Blackjack_Game
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public TabGroup tabGroup;
        public GameObject window;

        [HideInInspector]
        public Image bg;

        [Space]
        public UnityEvent onTabSelected;
        public UnityEvent onTabDeselected;


        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        private void Awake()
        {
            bg = GetComponent<Image>();
            tabGroup.Subscribe(this);
        }

        public void Select()
        {
            if (window)
                window.SetActive(true);
            onTabSelected?.Invoke();
        }

        public void Deselect()
        {
            if (window)
                window.SetActive(false);
            onTabDeselected?.Invoke();
        }
    }
}
