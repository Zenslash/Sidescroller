using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : Items, IUseable
{
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;


    // the bag has its own functionality, for example, creating slots  (P.S. What the hell it?)
    public BagScript MyBagScript { get; set; }
    public int Slots 
    { 
        get
        {
            return slots;
        }
    }
    public void Initialization(int slots)
    {
        this.slots = slots;
    }
}
