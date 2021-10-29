using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlotController : MonoBehaviour
{

    public delegate void InventoryEventHandler(InventoryChangedArgs args);
    public static event InventoryEventHandler InventorySlotChanged;
    public SlotPanel Panel;
    public int SlotIndex;

    #region AAAAAAAAAAAAAAAAAWHYWTFUIDONOTWORK
    public bool WORKUFOOL;
    private void Update()
    {
        if (WORKUFOOL)
        {
            Go();
            WORKUFOOL = false;
        }
    }

    #endregion

    public void Go()
    {
        InventorySlotChanged.Invoke(new InventoryChangedArgs(SlotIndex, Panel));
        Debug.Log("Go3");
    }
}
