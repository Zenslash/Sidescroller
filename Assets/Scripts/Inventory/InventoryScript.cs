using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }
    }

    // For debugging
    [SerializeField]
    private Item[] items;

    // Backpack is detached from inventory
    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]); // We place the backpack in our inventory
        bag.Initialization(16); // Create backpack
        bag.Consume(); 
    }

    public void Start()
    {
        
    }
    public void Update()
    {
        
    }
}
