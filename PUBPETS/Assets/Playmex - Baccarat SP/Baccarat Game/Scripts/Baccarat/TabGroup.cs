using UnityEngine;
using System.Collections.Generic;
namespace Baccarat_Game
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtons;
        public Color tabIdle;
        public Color tabHover;
        public Color tabActive;
        public Color tabDeactive;

        [HideInInspector]
        public TabButton tab;

        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
                tabButtons = new List<TabButton>();

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            if (tab == null || button != tab)
                button.bg.color = tabHover;
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            tab?.Deselect();

            tab = button;
            tab.Select();
            ResetTabs();
            button.bg.color = tabActive;
        }

        public void ResetTabs()
        {

            foreach (TabButton button in tabButtons)
            {
                if (tab == button)
                    continue;
                button.bg.color = tabIdle;
            }
        }

        public void ResetDefault()
        {
            foreach (TabButton button in tabButtons)
            {
                button.Deselect();
                button.bg.color = tabIdle;
            }
        }
    }
}
