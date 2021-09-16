using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Survival/Item", order = 51)]
public class Item : ScriptableObject
{
    // abstract because the parent class
    public string ItemName;
    public string Description;
    public GameObject Model;

    [SerializeField]
    private Sprite icon;
    [SerializeField] 
    private int stackSize; 

    private SlotScript slot; // for easier removal of an item

    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    protected SlotScript Slot  // no one should be able to change from the outside
    {
        get
        {
            return slot;
        }
        set  // set if the element has been moved
        {
            slot = value;
        }
    }


}
