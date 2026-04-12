using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blackjack_Game
{
    public class Guest_Trigger : MonoBehaviour
    {
        public BarCounterManager barCounterManager;
        public void ChangeSkin()
        {
            barCounterManager.ChangeLeaveGuestSkin();
        }
        public void FinishWine() 
        {
            //动画帧事件不稳定，我用那边Invoke触发
            //barCounterManager.MakeWineSuccess();
        }

    }
}