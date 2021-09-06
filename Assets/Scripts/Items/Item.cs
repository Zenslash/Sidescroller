using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Survival/Item", order = 51)]
public class Item : ScriptableObject
{
    public string ItemName;
    public string Description;
    public Sprite Icon;
    public GameObject Model;
}
