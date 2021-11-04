using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public List<ItemSlot> Slots;




    public int MaxSlots
    {
        get
        {
            return Slots.Count;
        }
        set
        {
            if (Slots.Count < value)
            {
                for (int i = 0; i < value - Slots.Count; i++)
                {
                    Slots.Add(new ItemSlot());
                }
            }
            else
            {
                int count = Slots.Count - value;
                int i;
                for (i= 0; i < count; i++)
                {
                    ItemSlot emptySlot = Slots.Find(Sl => Sl.IsEmpty); //remove empty slots
                    if (emptySlot != null)
                    {
                        Slots.Remove(emptySlot);
                    }
                    else
                    {
                        break;
                    }

                }
                count -= i;
                for (i = 0; i < count; i++) //remove not empty slots after droping items in it
                {
                    Slots[MaxSlots - i].Drop();
                }
                Slots.RemoveRange(MaxSlots - count, count);

            }
        }
    }

}


