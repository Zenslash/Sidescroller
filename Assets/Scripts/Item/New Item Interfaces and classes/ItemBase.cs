using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase 
{
    public GameObject Model;
    public Sprite InventorySprite;
    public abstract int Quantity { get; set; }

}
