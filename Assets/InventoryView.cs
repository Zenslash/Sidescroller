using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> inventorySlots;
    private List<GameObject> containerSlots;
    private int _inventorySlotActive;
    private int _containerSlotActive;

    public GameObject SlotPrefab;
    public GameObject PlayerInventoryBox;
    public GameObject OpenedContainerBox;
    public int MaxSlots;
    public int InventorySlotActive
    {
        get { return _inventorySlotActive; }
        set { }
    }
    public int ContainerSlotActive
    {
        get { return _containerSlotActive; }
        set { }
    }

    public void Create( )
    {

        inventorySlots = new List<GameObject>();
        containerSlots = new List<GameObject>();
        for (int i = 0; i < MaxSlots; i++)
        {
            Transform inventory = PlayerInventoryBox.transform;
            Transform container = OpenedContainerBox.transform;
            SlotController slot;

            GameObject objectSlot = Instantiate(SlotPrefab, inventory);
            slot = objectSlot.GetComponent<SlotController>();
            slot.SlotIndex = i;
            slot.Panel = SlotPanel.Inventory;

            inventorySlots.Add(objectSlot);

            objectSlot = Instantiate(SlotPrefab, container);
            slot = objectSlot.GetComponent<SlotController>();
            slot.SlotIndex = i;
            slot.Panel = SlotPanel.Container;

            containerSlots.Add(objectSlot);


        }
    }


    

}
