using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest_Trigger : MonoBehaviour
{
    public BarCounterManager barCounterManager;
    public void ChangeSkin() 
    {
        barCounterManager.ChangeLeaveGuestSkin();
    }
}
