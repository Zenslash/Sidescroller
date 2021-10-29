using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    
    private int _amount;
    private ItemBase item;

    

    public ItemBase GetItem
    {
        get { return item; }
    }
    public bool IsEmpty
    {
        get { return item is null; }
    }
    public int Amount
    {
        get
        {
            return _amount;
        }
        set
        {
            if (item is null)
                return;
            if (value > 0 && value < item.MaxStack)
                _amount = value;
        }
    }


    public void Equip(ItemBase item, int amount ) //
    {
        this.item = item;
        this.Amount = amount;
    }

    public void Drag(ItemSlot slot)
    {
        if (slot.item.name == item.name && item != null) //Stack one type of item
        {
            int transferAmount = Amount + slot.Amount;
            if (transferAmount > item.MaxStack)
            {
                Amount = item.MaxStack;
                slot.Amount = transferAmount - item.MaxStack;
            }
            else
            {
                Amount = transferAmount;
                slot.Discard();
            }

        } 
        else if (item is null) // move item to this empty slot
        {
            Equip(slot.item, slot.Amount);
            slot.Discard();
        }
        else //Swap items
        {
            int swapAm = slot.Amount;
            ItemBase swapItm = slot.item;
            slot.Equip(item, Amount);
            this.Equip(swapItm, swapAm);
        }
    }

    /// <summary>
    /// Delete item in slot
    /// </summary>
    public void Discard()
    {
        item = null;
        _amount = 0;
    }


    public void Drop()
    {

        //TODO Implement Drop in real world.
        item = null; 

    }

}
