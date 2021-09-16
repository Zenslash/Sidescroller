using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Please do not swear :( 
[CreateAssetMenu(fileName = "Bag", menuName = "Survival/Items/Bag")]
public class Bag : Item, IConsumable
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
    public void Consume()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();  // 11.1 - 14 minute
        MyBagScript.AddSlots(slots);
    }
}
