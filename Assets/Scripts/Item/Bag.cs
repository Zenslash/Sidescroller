using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Please do not swear :( 
[CreateAssetMenu(fileName = "Bag", menuName = "Survival/Items/Bag")]
<<<<<<< Updated upstream
public class Bag : Item
=======
public class Bag : Item,IWearable
>>>>>>> Stashed changes
{
    private int slots;
    /// <summary>
    /// for 3d object of item
    /// </summary>
    [SerializeField] private GameObject bagPrefab;
    
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
    /// <summary>
    /// using for item use;
    /// </summary>
    //public void Consume()
    //{
    //    MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
    //    MyBagScript.AddSlots(slots);
    //}

    public object Equip()
    {
        throw new System.NotImplementedException();
    }
}
