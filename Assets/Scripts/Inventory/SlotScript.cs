using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerClickHandler
{
    private Stack<Item> items = new Stack<Item>();

    [SerializeField]
    private Image icon;

    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        return true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            Debug.Log("LEFT but");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

        }
    }
}
