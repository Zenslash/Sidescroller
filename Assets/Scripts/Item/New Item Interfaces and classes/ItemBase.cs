using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase: ScriptableObject
{
    public GameObject Model;
    public Sprite InventorySprite;
    public int Quantity { get; set; }

    //List<Shootable> shootables;

    public void find()
    {
        
    }

}
