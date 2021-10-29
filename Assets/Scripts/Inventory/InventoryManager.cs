using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemContainer PlayerInventory;
    public ItemContainer OpenedContainer;
    public InventoryView View; //TODO Подписать слоты на itemChange


    

    private ItemSlot activeSlot;
    private void Start()
    {
        View = FindObjectOfType<InventoryView>();
        SlotController.InventorySlotChanged += ItemChange;
        View.Create();
    }


  
    public void ToggleInventory()
    {
        //TODO Implement
    }

    public void ItemChange(InventoryChangedArgs args)
    {
        Debug.Log(args.panel + " " + args.SlotIndex);
        //TODO Implement
    }
}
